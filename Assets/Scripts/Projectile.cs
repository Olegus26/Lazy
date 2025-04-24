using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;
    private Transform target;
    private int damage;
    private bool isEnemyProjectile;

    public void Initialize(Transform target, int damage, bool isEnemy)
    {
        this.target = target;
        this.damage = damage;
        this.isEnemyProjectile = isEnemy;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            target.GetComponent<UnitBase>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
