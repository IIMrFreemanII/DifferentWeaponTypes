using UnityEngine;
using Weapons.WeaponProperties;

namespace Weapons
{
    [RequireComponent(typeof(Launcher))]
    public class RangeWeapon : Weapon
    {
        private CameraHandler _cameraHandler;
        private Launcher _launcher;

        public RangeWeaponProps rangeWeaponProps;

        private void Awake()
        {
            _cameraHandler = FindObjectOfType<CameraHandler>();
            _launcher = GetComponent<Launcher>();
            characterInput = transform.parent.parent.GetComponent<CharacterInput>();
        }

        private void OnEnable()
        {
            characterInput.Fire += Fire;
        }

        private void OnDisable()
        {
            characterInput.Fire -= Fire;
        }

        private void Update()
        {
            WeaponAiming();
            Debug.DrawLine(transform.position, _cameraHandler.targetLook.position, Color.black);
        }

        private void WeaponAiming()
        {
            Vector3 direction = _cameraHandler.targetLook.position - transform.position;
            direction.Normalize();
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = lookRotation;
        }

        protected override void Fire()
        {
            _launcher.Launch(this);
        }
    }
}