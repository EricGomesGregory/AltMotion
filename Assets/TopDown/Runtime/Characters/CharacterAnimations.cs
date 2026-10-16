using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.TopDown
{
    public class CharacterAnimations : MonoBehaviour
    {
        private static readonly int AnimPropertySpeed = Animator.StringToHash("Speed");
        private static readonly int AnimPropertyHorizontal = Animator.StringToHash("Horizontal");
        private static readonly int AnimPropertyVertical = Animator.StringToHash("Vertical");
        private static readonly int AnimPropertyAttack = Animator.StringToHash("Attack");

        [SerializeField, HideInInspector] private Animator animator;

        public event UnityAction<bool> OnAttackStateChanged; 
        
        private Vector2 lastDirection;
        
        public void SetMotion(float speed, Vector3 direction) {
            lastDirection.x = direction.x;
            lastDirection.y = direction.z;
            animator.SetFloat(AnimPropertySpeed, speed);
            
            animator.SetFloat(AnimPropertyHorizontal, direction.x);
            animator.SetFloat(AnimPropertyVertical, direction.z);
        }

        public void Attack() {
            animator.SetFloat(AnimPropertyHorizontal, lastDirection.x);
            animator.SetFloat(AnimPropertyVertical, lastDirection.y);
            
            animator.SetTrigger(AnimPropertyAttack);
        }

        public void AttackStarted() {
            OnAttackStateChanged?.Invoke(true);
        }

        public void AttackEnded() {
            OnAttackStateChanged?.Invoke(false);
        }
        
#if UNITY_EDITOR

        private void OnValidate() {
            SetupReferences();
        }
#endif // UNITY_EDITOR
        
        private void Awake() {
            SetupReferences();
        }

        private void SetupReferences() {
            if (animator == null) {
                animator = GetComponent<Animator>();
            }
        }
    }
}
