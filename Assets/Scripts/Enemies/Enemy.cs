using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, ITarget
    {
        [SerializeField] private float health = 100f;

        public float Health
        {
            get => health;
            set => health = value;
        }

        public void TakeDamage(float damage)
        {
            print($"{gameObject.name} got {damage} damage.");
        
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }
        
            print($"I have: {Health} health.");
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
