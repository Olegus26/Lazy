using UnityEngine;

public class RangedUnit : UnitBase
{
    public GameObject projectilePrefab; // Префаб стрелы

    protected override void Attack()
    {
        if (target != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Initialize(target, damage, isEnemy);
        }
    }
}
