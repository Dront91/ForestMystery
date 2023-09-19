using UnityEngine;
using UnityEngine.InputSystem;

namespace MysteryForest
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MovementInput { get; private set; }
        public bool AttackInput { get; private set; }
        public bool ThrowBombInput { get; private set; }
        public bool UseItemInput { get; private set; }
        public bool SwitchBombInput { get; private set; }
        public bool SwitchActiveItemInput { get; private set; }

        public void GetMovementInput(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void GetAttackInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AttackInput = true;

            }
        }
        public void GetThrowBombInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ThrowBombInput = true;
            }
        }

        public void GetSwitchBombInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                SwitchBombInput = true;
            }
        }

        public void GetUseItemInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                UseItemInput = true;
            }
        }

        public void ResetInputState()
        {
            AttackInput = false;
            ThrowBombInput = false;
            UseItemInput = false;
            SwitchBombInput = false;
            SwitchActiveItemInput = false;
        }
    }
}
