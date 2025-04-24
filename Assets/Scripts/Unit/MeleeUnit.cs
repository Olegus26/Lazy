using UnityEngine;

public class MeleeUnit : UnitBase
{
    protected override void Attack()
    {
        if (target != null)
        {
            target.GetComponent<UnitBase>().TakeDamage(damage);
        }
    }
}
