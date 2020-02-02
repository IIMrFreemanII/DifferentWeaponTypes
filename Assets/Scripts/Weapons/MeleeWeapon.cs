using Enemies;
using Extensions;
using UnityEngine;
using Weapons.WeaponProperties;

namespace Weapons
{
    [RequireComponent(typeof(BoxCollider))]
    public class MeleeWeapon : Weapon
    {
        [SerializeField] private MeleeWeaponProps meleeWeaponProps = null;
        private Animator _animator = null;
        private BoxCollider _collider = null;
        
        private void Awake()
        {
            characterInput = transform.parent.parent.GetComponent<CharacterInput>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<BoxCollider>();
            _collider.enabled = false;
        }
        private void OnEnable()
        {
            characterInput.Fire += Fire;
        }

        private void OnDisable()
        {
            characterInput.Fire -= Fire;
        }

        protected override void Fire()
        {
            _animator.Play(meleeWeaponProps.animationAttackName);
        }

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.HandleComponent<ITarget>(target => ApplyDamage(target, meleeWeaponProps.damage));
        }
        
        private void ApplyDamage(ITarget target, float damage)
        {
            target.TakeDamage(damage);
        }

        public void EnableCollider() => _collider.enabled = true;
        public void DisableCollider() => _collider.enabled = false;
    }
}