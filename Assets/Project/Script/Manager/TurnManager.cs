using System;
using System.Collections;
using Controllers;
using GameLogic.Services;
using UnityEngine;

namespace Project.Script.Manager
{
    public class TurnManager
    {
        public event Action<bool> TurnCompleted;

        private readonly GameService _gameService;
        private readonly GameController _gameController;

        public bool IsResolvingTurn { get; private set; }

        public TurnManager(GameService gameService, GameController gameController)
        {
            _gameService = gameService;
            _gameController = gameController;
            IsResolvingTurn = false;
        }

        public IEnumerator PlayTurn(Vector2Int from, Vector2Int to)
        {
            if (IsResolvingTurn)
                yield break;

            IsResolvingTurn = true;

            var isValid = _gameService.IsValidMovement(from.x, from.y, to.x, to.y);

            if (!isValid)
            {
                yield return PlayInvalidSwap(from, to);
                FinishTurn(false);
                yield break;
            }

            yield return PlayValidSwap(from, to);
            FinishTurn(true);
        }

        private IEnumerator PlayInvalidSwap(Vector2Int from, Vector2Int to)
        {
            yield return _gameController.AnimateSwap(from, to);
            yield return _gameController.AnimateSwap(to, from);
        }
        
        private IEnumerator PlayValidSwap(Vector2Int from, Vector2Int to)
        {
            yield return _gameController.AnimateSwap(from, to);
            var step = _gameService.SwapTile(from, to);

            while (step != null)
            {
                yield return _gameController.AnimateBoardSequence(step);
                step = _gameService.ResolveNextStep();
            }
        }
        
        private void FinishTurn(bool validMovement)
        {
            IsResolvingTurn = false;
            TurnCompleted?.Invoke(validMovement);
        }
    }
}