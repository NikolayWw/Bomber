using UnityEngine;

namespace Code.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 MoveAxis { get; }
        bool AttackPress { get; }
        void SetMoveAxis(Vector2 direction);
        void SetAttack(bool isAttack);
    }
}