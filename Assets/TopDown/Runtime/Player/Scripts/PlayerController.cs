using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.TopDown.Runtime
{
    public class PlayerController : MonoBehaviour, InputActionsMap.IGameplayActions
    {
        [SerializeField] private PlayerCharacter character;
        
        [SerializeField, HideInInspector] private PlayerInput playerInput;
        private Camera currentCamera;
        private InputActionsMap input;

#if UNITY_EDITOR

        private void OnValidate() {
            SetupReferences();
        }
#endif // UNITY_EDITOR
        
        private void Awake() {
            SetupReferences();

            currentCamera = playerInput.camera;
            input = new InputActionsMap();
            
            input.Gameplay.AddCallbacks(this);
        }

        private void SetupReferences() {
            if (playerInput == null) {
                playerInput = GetComponent<PlayerInput>();
            }
        }
        
        public void OnMove(InputAction.CallbackContext context) {
            var inputValue = context.ReadValue<Vector2>();
            var direction = Vector3.zero;
            if (inputValue.magnitude > 0.0f) {
                if (Mathf.Abs(inputValue.x) >= Mathf.Abs(inputValue.y)) {
                    direction.x = inputValue.x > 0 ? 1 : -1;
                }
                else if (Mathf.Abs(inputValue.x) < Mathf.Abs(inputValue.y)) {
                    direction.z = inputValue.y > 0 ? 1 : -1;
                }
            }
            //var direction = new Vector3(inputValue.x, 0.0f, inputValue.y).normalized;
            character.SetMoveDirection(direction);
        }

        public void OnAttack(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Started) {
                character.Attack();
            }
        }
    }
}
