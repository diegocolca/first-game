    using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1.5f;
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    public Transform attackPoint;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Move moveScript;

    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveScript = GetComponent<Move>();
    }

    public void TriggerAttack()
    {
        if (isAttacking || animator.GetBool("isDead")) return;

        isAttacking = true;
        animator.SetTrigger("Attack_Katana_Player");

        // Bloquea el movimiento (desde PlayerController)
        if (moveScript != null)
        {
            moveScript.BlockMovement();
        }
    }

    // Animation Event
    public void ApplyAttackDamage()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll((Vector2)attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            HealthEnemy health = enemy.GetComponent<HealthEnemy>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
                //Debug.Log("Daño aplicado a" + enemy.name);  
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        //if (spriteRenderer == null)
        //    spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;
        // Dibuja un círculo en el editor para visualizar el rango de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)attackPoint.position, attackRange);
    }

    // Animation Event
    public void FinishAttack()
    {
        isAttacking = false;

        if (moveScript != null)
        {
            moveScript.UnblockMovement();
        }
    }

    public void CancelAttack()
    {
        isAttacking = false;
        animator.ResetTrigger("Attack_Katana_Player");

        if (moveScript != null)
        {
            moveScript.UnblockMovement();
        }
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
