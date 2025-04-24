using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    public float speed;
    public int health;
    public int damage;
    public float attackRange;
    public bool isEnemy;

    protected Transform target;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D unitCollider;
    private bool isDead = false; // Оставляем только эту переменную

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        unitCollider = GetComponent<Collider2D>();

        spriteRenderer.flipX = isEnemy;
    }

    protected virtual void Update()
    {
        if (isDead) return;


        if (target == null)
        {
            FindTarget();
            Move(); // Если цели нет – двигаемся
        }

        // Привязываем `isAttacking` к наличию цели
        if (animator != null)
            animator.SetBool("isAttacking", target != null);
    }

    protected void Move()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime * (isEnemy ? -1 : 1));
    }

    protected void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (var hit in hits)
        {
            UnitBase unit = hit.GetComponent<UnitBase>();
            if (unit != null && unit.isEnemy != isEnemy)
            {
                target = unit.transform;
                return;
            }
        }
    }

    public virtual void TakeDamage(int dmg)
    {
        if (isDead) return;

        health -= dmg;
        if (health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        isDead = true;
        target = null; // Обнуляем цель, чтобы юнит не атаковал после смерти

        if (animator != null)
            animator.SetTrigger("death");

        unitCollider.enabled = false;
        speed = 0;
        this.enabled = false;

        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    protected abstract void Attack();

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
