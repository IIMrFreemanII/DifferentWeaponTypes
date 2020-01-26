using Extensions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Bullet : Projectile, IHaveDamage
{
    private Rigidbody _rb;
    private CapsuleCollider _collider;
    public float Damage { get; set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        
        _collider.isTrigger = true;
        _rb.isKinematic = true;
    }

    public void Launch(float damage, float speed, Vector3 spawnPosition, Quaternion rotation, Transform parent = null)
    {
        this.speed = speed;
        Damage = damage;
        transform.position = spawnPosition;
        transform.rotation = rotation;
        transform.SetParent(parent);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.HandleComponent<ITarget>(target =>
        {
            target.TakeDamage(Damage);
            Die();
        });
        
        Die();
        // if (other.CompareTag("Enemy"))
        // {
        //     ITarget target = other.transform.GetComponent<ITarget>();
        //     
        //     if (target != null)
        //     {
        //         ApplyDamage(target, Damage);
        //     }
        // }
        // else
        // {
        //     Die();
        // }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}