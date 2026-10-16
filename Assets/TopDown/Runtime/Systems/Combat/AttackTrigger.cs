using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.TopDown
{
    public class AttackTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject owner;
        [SerializeField, Min(1.0f)] private float damage = 10.0f;
        
        [SerializeField, HideInInspector] private Collider collider;

        public void Perform() {
            collider.enabled = true;
            UniTask.WaitForSeconds(0.2f)
            .ContinueWith(() => {
                collider.enabled = false;
            })
            .Forget();
        }
        
        private void Awake() {
            if (collider == null) {
                collider = GetComponent<Collider>();
            }
            
            collider.enabled = false;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject == owner) {
                return;
            }

            if (other.TryGetComponent(out IDamageable damageable)) {
                damageable.TakeDamage(owner, damage);
            }
        }
    }
}
