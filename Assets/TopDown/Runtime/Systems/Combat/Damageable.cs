using UnityEngine;

namespace Game.TopDown
{
    public interface IDamageable
    {
        public void TakeDamage(GameObject instigator, float damage);
    }
}
