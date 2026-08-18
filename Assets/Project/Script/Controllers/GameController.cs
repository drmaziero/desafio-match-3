using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Models;
using UnityEngine;
using Views;

namespace Controllers
{
    public class GameController : MonoBehaviour
    {
        public event Action TurnCompleted;
        public event Action InvalidMovementCompleted;
        
        [Header("Controller")]
        [SerializeField] private BoardController boardController;

        [SerializeField] private TileInputController inputController;
        
        [Header("Views")]
        [SerializeField] private HudView _hudView;
        
        private HudController _hudController;
        
        private void Awake()
        {
            boardController.TileCreated += OnTileCreated;
            boardController.OutOfBoard += OnOutBoard;
        }
        
        private void OnDestroy()
        {
            boardController.TileCreated -= OnTileCreated;
            boardController.OutOfBoard -= OnOutBoard;
        }

        public void Init(List<List<Tile>> board)
        {;
            boardController.CreateBoard(board);
        }

        public void Reset()
        {
            boardController.ClearBoard();
        }
        
        private void OnDisable()
        {
            Reset();
        }
        
        private void OnTileCreated(TileViewController tileController)
        {
            inputController.RegisterTileViewController(tileController);
        }
        
        private void OnOutBoard()
        {
           inputController.ResetPosition();
        }


        public void AnimateInvalidSwap(Vector2Int from, Vector2Int to)
        {
            StartCoroutine(AnimateInvalidSwapCoroutine(from, to));
        }

        private IEnumerator AnimateInvalidSwapCoroutine(Vector2Int from, Vector2Int to)
        {
            yield return boardController.SwapTiles(from.x, from.y, to.x, to.y).WaitForCompletion();
            yield return boardController.SwapTiles(to.x, to.y, from.x, from.y).WaitForCompletion();
            InvalidMovementCompleted?.Invoke();
        }

        public void AnimateValidSwap(Vector2Int from, Vector2Int to, List<BoardSequence> boardSequences)
        {
            StartCoroutine(AnimateValidSwapCoroutine(from, to, boardSequences));
        }

        private IEnumerator AnimateValidSwapCoroutine(Vector2Int from, Vector2Int to,
            IEnumerable<BoardSequence> boardSequences)
        {
            yield return boardController.SwapTiles(from.x, from.y, to.x, to.y).WaitForCompletion();

            foreach (var boardSequence in boardSequences)
            {
                yield return boardController.ClearTiles(boardSequence.MatchedPosition).WaitForCompletion();
                yield return boardController.CreateSpecialTile(boardSequence.AddedSpecialTiles).WaitForCompletion();
                yield return boardController.MoveTiles(boardSequence.MovedTiles).WaitForCompletion();
                yield return boardController.RefillTiles(boardSequence.AddedTiles).WaitForCompletion();
            }
            
            TurnCompleted?.Invoke();
        }
    }
}
