using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.TopDown.Runtime
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private CharacterAnimations animations;
        
        [SerializeField, HideInInspector] private NavMeshAgent agent;
        private float agentSpeed;
        
        private Vector3 moveDirection;
        private Vector3 lastMoveDirection;
        
#if UNITY_EDITOR
        private void OnValidate() {
            SetupReferences();
        }
#endif // UNITY_EDITOR
        
        private void Awake() {
            SetupReferences();

            agentSpeed = agent.speed;
            animations.OnAttackStateChanged += OnAttackStateChanged;
        }

        private void SetupReferences() {
            if (agent == null) {
                agent = GetComponent<NavMeshAgent>();
            }
        }

        public void Attack() {
            animations.Attack();
        }
        
        public void SetMoveDirection(Vector3 direction) {
            if (Mathf.Abs(direction.magnitude) > 0.0f) {
                lastMoveDirection = moveDirection;
            }
            moveDirection = direction;
        }

        private void Update() {
            var velocity = moveDirection * agent.speed * Time.deltaTime;
            var speed = (moveDirection * agent.speed).magnitude;
            animations.SetMotion(speed, speed > 0.0 ? moveDirection : lastMoveDirection);
            agent.Move(velocity);
        }

        private void OnAttackStateChanged(bool attackState) {
            agent.speed = attackState ? 0.0f : agentSpeed;
            Debug.Log($"[Player] Character::OnAttackStateChanged: state={attackState} speed={agent.speed}");
        }
    }
}
