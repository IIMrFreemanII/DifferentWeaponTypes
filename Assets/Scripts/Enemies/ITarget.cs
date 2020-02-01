namespace Enemies
{
    public interface ITarget
    {
        float Health { get; set; }
        void TakeDamage(float damage);
        void Die();
    }
}
