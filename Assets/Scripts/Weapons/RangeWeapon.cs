using UnityEngine;

[RequireComponent(typeof(ILauncher))]
public class RangeWeapon : Weapon
{
    private CameraHandler _cameraHandler;
    public Transform projectileSpawnPosition;
    private ILauncher _launcher;

    private void Start()
    {
        _cameraHandler = FindObjectOfType<CameraHandler>();
        projectileSpawnPosition = transform.Find("ProjectileSpawnPosition");
        _launcher = GetComponent<ILauncher>();
    }

    private void Update()
    {
        WeaponAiming();
        Debug.DrawLine(transform.position, _cameraHandler.targetLook.position, Color.black);

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void WeaponAiming()
    {
        Vector3 direction = _cameraHandler.targetLook.position - transform.position;
        direction.Normalize();
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    private void Fire()
    {
        _launcher.Launch(this);
    }
}
