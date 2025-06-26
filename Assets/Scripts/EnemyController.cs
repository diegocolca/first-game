using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 move;
    public Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float range_agro = 15f;
    public float speed = 5f;
    private bool playerIsAlive = true;

    [Header("Ataque")]
    public float attackRange = 3f;
    public int attackDamage = 10;
    public float attackCooldown = 2f;
    private float lastAttackTime;
    private bool isAttacking = false;
    private bool isDead = false;

    private HealthPlayer playerHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player != null)
        {
            playerHealth = player.GetComponent<HealthPlayer>();

            if (GameManager.Instance != null && GameManager.Instance.IsPlayerDead)
            {
                playerIsAlive = false;
                animator.SetBool("has_a_target", false);
            }
            else if (playerHealth != null)
            {
                playerHealth.onDeath += OnPlayerDeath;
            }
        }
    }

    void Update()
    {
        if (!playerIsAlive || isDead) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown && !isAttacking)
            {
                StartCoroutine(PerformAttack());
            }

            move = Vector2.zero;
            animator.SetBool("has_a_target", false);
        }
        else
        {
            Follow_Player();
        }
    }

    public void Follow_Player()
    {
        float distance_player = Vector2.Distance(transform.position, player.position);

        if (distance_player < range_agro)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            move = new Vector2(direction.x, direction.y);

            if (player.position.x < transform.position.x)
                spriteRenderer.flipX = true;
            else if (player.position.x > transform.position.x)
                spriteRenderer.flipX = false;

            animator.SetBool("has_a_target", true);
        }
        else
        {
            move = Vector2.zero;
            animator.SetBool("has_a_target", false);
        }

        rb.MovePosition(rb.position + move * speed * Time.deltaTime);
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack_Velociraptor");

        yield return new WaitForSeconds(0.5f); // sincronizar con la animacion :v

        if (player != null && Vector2.Distance(transform.position, player.position) < attackRange)
        {
            if (playerHealth == null)
                playerHealth = player.GetComponent<HealthPlayer>();

            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage);
        }

        lastAttackTime = Time.time;
        isAttacking = false;
    }

    
    public void ApplyAttackDamage()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) < attackRange)
        {
            if (playerHealth == null)
            {
                playerHealth = player.GetComponent<HealthPlayer>();
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }


    private void OnPlayerDeath()
    {
        playerIsAlive = false;
        move = Vector2.zero;
        animator.SetBool("has_a_target", false);
    }

    public void OnDeath()
    {
        isDead = true;
        move = Vector2.zero;
        animator.SetBool("has_a_target", false);
    }
}
