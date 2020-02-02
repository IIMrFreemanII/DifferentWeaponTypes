using UnityEngine;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        protected CharacterInput characterInput;
        protected abstract void Fire();
    }
}