using UnityEngine;

namespace Weapons
{
    public abstract class Launcher : MonoBehaviour
    {
        public Transform projectileSpawnTransform;
        
        public abstract void Launch(RangeWeapon weapon);
        private void OnValidate()
        {
            projectileSpawnTransform = transform.GetComponentInChildren<ProjectileSpawnPosition>().transform;
        }
    }
}
