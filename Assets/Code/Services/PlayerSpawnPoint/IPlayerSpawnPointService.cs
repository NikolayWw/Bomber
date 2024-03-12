using System;
using UnityEngine;

namespace Code.Services.PlayerSpawnPoint
{
    public interface IPlayerSpawnPointService : IService
    {
        void SetPositions(Vector2[] positions);
        Vector2 GetPosition();
        bool Initialized { get; }
        Action OnInitialized { get; set; }
    }
}