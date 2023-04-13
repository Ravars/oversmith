using Oversmith.Scripts.Utils;
using UnityEngine;

namespace Oversmith.Scripts.Managers
{
    public enum GameState
    {
        WaitingPlayers,
        Countdown,
        Play,
        Pause,
        EndGame
    }
    public class GameStateManager : Singleton<GameStateManager>
    {
        public delegate GameState GameStateChangesEvent();
        public GameStateChangesEvent OnGameStateChanges;

        public GameState currentGameState;
        public void ChangeState(GameState newGameState)
        {
            OnGameStateChanges?.Invoke();
        }
    }
}