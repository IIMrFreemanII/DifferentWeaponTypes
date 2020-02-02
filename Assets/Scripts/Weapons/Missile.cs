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
        
        private MissileProps _missileProps;
        
        private float _timeToFly;
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
            // missile speed
            Vector3 rbSpeed = transform.forward * _missileProps.speed;
            // maxDistanceDelta = missile acceleration 
            _rb.velocity = Vector3.MoveTowards(_rb.velocity, rbSpeed, _missileProps.acceleration);
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
            // rotation speed
            Quaternion smoothLookRotation = Quaternion.Lerp(_rb.rotation, lookRotation, Time.deltaTime * _missileProps.rotationSpeed);
            _rb.MoveRotation(smoothLookRotation);
        }

        public void Launch(Transform target, Ray ray, MissileProps missileProps, Transform parent = null)
        {
            _missileProps = missileProps;
            speed = _missileProps.speed;
            Damage = _missileProps.damage;
            transform.SetParent(parent);
            _targetToHit = target;
            
            _timeToFly = _missileProps.timeToFly;

            if (_targetToHit == null)
            {
                print("target is null");
                _directionToHit = ray.direction;
            }

            _isAiming = true;
            _rb.isKinematic = false;
            _collider.enabled = true;
            _rb.AddForce(transform.forward * _missileProps.startForce, ForceMode.Impulse);

            // time to destroy
            StartCoroutine(DieWithDelay(_missileProps.timeToDestroy));
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
            _missileProps = null;
            
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