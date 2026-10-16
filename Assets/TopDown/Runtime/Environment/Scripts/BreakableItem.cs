using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.TopDown
{
    public class BreakableItem : MonoBehaviour, IDamageable
    {
        [SerializeField, Min(1.0f)] private float defaultHealth = 10.0f;

        private float _health;
        public float Health {
            get => _health;
            private set {
                _health = Mathf.Clamp(value, 0, defaultHealth);
                OnHealthChange ?.Invoke(_health);
            }
        }
        
        public event UnityAction<float> OnHealthChange; 

        public void TakeDamage(GameObject instigator, float damage) {
            if (instigator == gameObject) {
                return;
            }
            
            Health -= damage;
        }

        private void Awake() {
            OnHealthChange += HandleDeath;
        }

        private void Start() {
            _health = defaultHealth;
        }

        private void HandleDeath(float health) {
            if (health <= 0.0f) {
                Destroy(gameObject);
            }
        }
    }
}
