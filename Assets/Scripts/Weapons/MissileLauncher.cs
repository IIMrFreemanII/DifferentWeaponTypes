using System.Collections;
using System;
using Enemies;
using Extensions;
using ObjectPools;
using UnityEngine;

namespace Weapons
{
    [Serializable] public class MissileProps
    {
        [HideInInspector] public float damage;
        [HideInInspector] public float speed;
        public float timeToFly;
        public float startForce;
        public float acceleration;
        public float rotationSpeed;
        public float timeToDestroy;
    }
    public class MissileLauncher : Launcher
    {
        [SerializeField] private Missile _missileToLaunch;
        [SerializeField] private Transform currentTarget;
        [SerializeField] private float timeToReload = 0.3f;
        [SerializeField] private float maxDistanceToHit = 25f;
        [SerializeField] private MissileProps missileProps = null;

        private void Awake()
        {
            Destroy(_missileToLaunch.gameObject);
            _missileToLaunch = null;
            LoadMissile();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                LoadMissile();
            }
        }

        private void LoadMissile()
        {
            _missileToLaunch = MissilePool.Instance.Get();
            
            _missileToLaunch.gameObject.SetActive(true);
            _missileToLaunch.transform.SetParent(transform);
            _missileToLaunch.transform.position = projectileSpawnTransform.position;
            _missileToLaunch.transform.rotation = projectileSpawnTransform.rotation;
        }

        private IEnumerator LoadMissileWithDelay(float reloadTime)
        {
            yield return new WaitForSeconds(reloadTime);
            currentTarget = null;
            LoadMissile();
        }
        
        public override void Launch(RangeWeapon weapon)
        {
            if (_missileToLaunch != null)
            {
                Ray ray = new Ray(projectileSpawnTransform.position, projectileSpawnTransform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, maxDistanceToHit))
                {
                    hit.collider.gameObject.HandleComponent<ITarget>(target => currentTarget = hit.transform );
                    print(hit.collider.name);
                }
                
                MissileProps newMissileProps = new MissileProps
                {
                    acceleration = missileProps.acceleration,
                    rotationSpeed = missileProps.rotationSpeed,
                    damage = weapon.rangeWeaponProps.damage,
                    speed = weapon.rangeWeaponProps.projectileSpeed,
                    startForce = missileProps.startForce,
                    timeToDestroy = missileProps.timeToDestroy,
                    timeToFly = missileProps.timeToFly,
                };
                
                _missileToLaunch.Launch(currentTarget, ray, newMissileProps);
                
                _missileToLaunch = null;

                StartCoroutine(LoadMissileWithDelay(timeToReload));
            }
        }
    }
}
