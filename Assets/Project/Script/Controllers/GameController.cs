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
        [Header("Controller")]
        [SerializeField] private BoardController boardController;
        
        [Header("Views")]
        [SerializeField] private HudView _hudView;
        
        private HudController _hudController;

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
            StopAllCoroutines();
            Reset();
        }

        public IEnumerator AnimateSwap(Vector2Int from, Vector2Int to)
        {
            yield return boardController.SwapTiles(from.x, from.y, to.x, to.y).WaitForCompletion();
        }

        public IEnumerator AnimateBoardSequence(BoardSequence boardSequence)
        {
            boardController.AnimateActiveSpecial(boardSequence.ActivatedSpecial);
            yield return boardController.ClearTiles(boardSequence.MatchedPosition, boardSequence.ActivatedSpecial).WaitForCompletion();
            yield return boardController.CreateSpecialTile(boardSequence.AddedSpecialTiles).WaitForCompletion();
            boardController.StartPendingSpecials(boardSequence.PendingSpecials);
            yield return boardController.MoveTiles(boardSequence.MovedTiles).WaitForCompletion();
            yield return boardController.RefillTiles(boardSequence.AddedTiles).WaitForCompletion();
        }
    }
}
