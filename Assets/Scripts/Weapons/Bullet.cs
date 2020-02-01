using System.Collections;
using Enemies;
using Extensions;
using ObjectPools;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class Bullet : Projectile, ICanDamage
    {
        private Rigidbody _rb;
        private CapsuleCollider _collider;
        public float Damage { get; set; }
        private Vector3 _lastPosition;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        
            _rb.isKinematic = false;
        }

        public void Launch(float damage, float newSpeed, Vector3 spawnPosition, Quaternion rotation, Transform parent = null)
        {
            gameObject.SetActive(true);
            speed = newSpeed;
            Damage = damage;
            transform.position = spawnPosition;
            transform.rotation = rotation;
            transform.SetParent(parent);
            
            _rb.isKinematic = false;
            _rb.AddForce(transform.forward * speed, ForceMode.Impulse);

            StartCoroutine(DieWithDelay(5f));
        }

        private void Update()
        {
            Vector3 currentPosition = transform.position + (transform.forward * 0.1f);
            if (Physics.Linecast(_lastPosition, currentPosition, out RaycastHit hit, LayerMask.GetMask("Default", "Enemy")))
            {
                hit.collider.gameObject.HandleComponent<ITarget>(target => ApplyDamage(target, Damage));
                print(hit.collider.gameObject.name);
                Die();
            }
            
            _lastPosition = transform.position;
        }

        public void ApplyDamage(ITarget target, float damage)
        {
            target.TakeDamage(Damage);
        }

        private void Die()
        {
            _rb.isKinematic = true;
            BulletPool.Instance.ReturnToPool(this);
        }
        
        private IEnumerator DieWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _rb.isKinematic = true;
            BulletPool.Instance.ReturnToPool(this);
        }
    }
}