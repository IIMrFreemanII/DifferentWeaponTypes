using ObjectPools;

namespace Weapons
{
    public class BulletLauncher : Launcher
    {
        public override void Launch(RangeWeapon weapon)
        {
            Bullet bullet = BulletPool.Instance.Get();
            bullet.Launch(weapon.weaponProperties.damage, weapon.weaponProperties.projectileSpeed, projectileSpawnTransform.position, weapon.transform.rotation);
        }
    }
}
