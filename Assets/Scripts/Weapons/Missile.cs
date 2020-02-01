using System.Collections;
using Enemies;
using Extensions;
using ObjectPools;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Missile : Projectile, ICanDamage
    {
        private Rigidbody _rb;
        private MeshCollider _collider;
        private Transform _targetToHit;
        private Vector3 _directionToHit;

        private float _timeToFly = 0.25f;
        private float _timer;
        private bool _isAiming;
        
        public float Damage { get; set; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponentInChildren<MeshCollider>();
            _rb.isKinematic = true;
            _collider.enabled = false;
        }

        private void OnValidate()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponentInChildren<MeshCollider>();
        }

        private void FixedUpdate()
        {
            
            if (!_isAiming) return;
            
            _timer += Time.fixedDeltaTime;

            if (_timer >= _timeToFly)
            {
                MissileAiming(_targetToHit);
                MoveMissile();
            }
        }

        private void MoveMissile()
        {
            Vector3 rbSpeed = transform.forward * 30f;
            _rb.velocity = Vector3.MoveTowards(_rb.velocity, rbSpeed, 1.5f);
        }

        private void MissileAiming(Transform target)
        {
            Vector3 direction;
            if (target != null)
            {
                direction = target.position - transform.position;
            }
            else
            {
                direction = _directionToHit;
            }

            if (direction == Vector3.zero)
            {
                direction = transform.forward;
            }
            
            direction.Normalize();
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Quaternion smoothLookRotation = Quaternion.Lerp(_rb.rotation, lookRotation, Time.deltaTime * 8);
            _rb.MoveRotation(smoothLookRotation);
        }

        public void Launch(float damage, float newSpeed, Transform target, Ray ray, Transform parent = null)
        {
            speed = newSpeed;
            Damage = damage;
            transform.SetParent(parent);
            _targetToHit = target;

            if (_targetToHit == null)
            {
                print("target is null");
                _directionToHit = ray.direction;
            }

            _isAiming = true;
            _rb.isKinematic = false;
            _collider.enabled = true;
            _rb.AddForce(transform.forward * speed, ForceMode.Impulse);

            StartCoroutine(DieWithDelay(10f));
        }

        public void ApplyDamage(ITarget target, float damage)
        {
            target.TakeDamage(damage);
            Die();
        }
        
        private void Die()
        {
            _timer = 0;
            _directionToHit = Vector3.zero;
            _targetToHit = null;
            _isAiming = false;
            
            _rb.isKinematic = true;
            _collider.enabled = false;
            MissilePool.Instance.ReturnToPool(this);
        }
        
        private IEnumerator DieWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Die();
        }

        private void OnCollisionEnter(Collision other)
        {
            other.gameObject.HandleComponent<ITarget>(target => ApplyDamage(target, Damage));
        }
    }
}