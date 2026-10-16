using System;
using UnityEngine;
using UnityEngine.AI;

namespace Game.TopDown.Runtime
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] private CharacterAnimations animations;
        
        [SerializeField, HideInInspector] private NavMeshAgent agent;
        [SerializeField, HideInInspector] private AttackTrigger attack;
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
            animations.OnAttackTriggered += attack.Perform;
        }

        private void SetupReferences() {
            if (agent == null) {
                agent = GetComponent<NavMeshAgent>();
            }

            if (attack == null) {
                attack = GetComponentInChildren<AttackTrigger>();
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
            var direction = speed > 0.0 ? moveDirection : lastMoveDirection;
            animations.SetMotion(speed, direction);
            agent.Move(velocity);

            attack.transform.localPosition = direction;
        }

        private void OnAttackStateChanged(bool attackState) {
            agent.speed = attackState ? 0.0f : agentSpeed;
        }
    }
}
