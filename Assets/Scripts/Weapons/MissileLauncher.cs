using System.Collections;
using Enemies;
using Extensions;
using ObjectPools;
using UnityEngine;

namespace Weapons
{
    public class MissileLauncher : Launcher
    {
        [SerializeField] private Missile _missileToLaunch;
        [SerializeField] private Transform currentTarget;

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
                if (Physics.Raycast(ray, out RaycastHit hit, 25f))
                {
                    hit.collider.gameObject.HandleComponent<ITarget>(target => currentTarget = hit.transform );
                    print(hit.collider.name);
                }
                
                _missileToLaunch.Launch(weapon.weaponProperties.damage, weapon.weaponProperties.projectileSpeed, currentTarget, ray);
                
                _missileToLaunch = null;

                StartCoroutine(LoadMissileWithDelay(0.3f));
            }
        }
    }
}
