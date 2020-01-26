using UnityEngine;

[CreateAssetMenu(menuName = "Weapon Properties/property")]
public class WeaponProps : ScriptableObject
{
    public string weaponName;
    public float damage;

    public float projectileSpeed;
}
