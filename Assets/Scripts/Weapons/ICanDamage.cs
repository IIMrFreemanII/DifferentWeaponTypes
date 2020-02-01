using Enemies;

namespace Weapons
{
    public interface ICanDamage
    {
        float Damage { get; set; }
        void ApplyDamage(ITarget target, float damage);
    }
}
