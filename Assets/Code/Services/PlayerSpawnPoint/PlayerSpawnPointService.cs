using System;
using Unity.Netcode;
using UnityEngine;

namespace Code.Services.PlayerSpawnPoint
{
    public class PlayerSpawnPointService : MonoBehaviour, IPlayerSpawnPointService
    {
        private Vector2[] _positions;
        private int _currentIndex;
        public bool Initialized { get; private set; }

        public Action OnInitialized { get; set; }

        public void SetPositions(Vector2[] positions)
        {
            _positions = positions;
            Initialized = true;
            OnInitialized?.Invoke();
        }

        public Vector2 GetPosition()
        {
            Vector2 position = _positions[_currentIndex];
            _currentIndex++;
            _currentIndex %= _positions.Length;
            return position;
        }
    }
}