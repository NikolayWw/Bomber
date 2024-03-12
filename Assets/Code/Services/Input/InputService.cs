using UnityEngine;

namespace Code.Services.Input
{
    public class InputService : IInputService
    {
        public Vector2 MoveAxis { get; private set; }

        public bool AttackPress { get; private set; }

        public void SetMoveAxis(Vector2 direction) => 
            MoveAxis = direction;

        public void SetAttack(bool isAttack) => 
            AttackPress = isAttack;
    }
}