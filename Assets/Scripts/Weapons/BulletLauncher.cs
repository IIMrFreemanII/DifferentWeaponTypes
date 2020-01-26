using UnityEngine;

public class BulletLauncher : MonoBehaviour, ILauncher
{
    [SerializeField] private Bullet bulletScript;
    
    public void Launch(RangeWeapon weapon)
    {
        Bullet bullet = Instantiate(bulletScript, weapon.projectileSpawnPosition.position, weapon.transform.rotation);
        bullet.Launch(weapon.weaponProperties.damage, weapon.weaponProperties.projectileSpeed, weapon.projectileSpawnPosition.position, weapon.transform.rotation);
    }
}
