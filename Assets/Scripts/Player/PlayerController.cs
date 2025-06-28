using UnityEngine;

public class Move : MonoBehaviour
{
    public Joystick movementJoystick;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public float speed = 5f;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f;

    private bool isTakingDamage = false;
    private bool isDead = false;
    private float knockbackTimer;

    private bool movementBlocked = false;

    private HealthPlayer healthPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthPlayer = GetComponent<HealthPlayer>();
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (!isTakingDamage && !movementBlocked)
        {
            Movement();
        }
        else if (isTakingDamage)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0)
            {
                isTakingDamage = false;
                animator.SetBool("take_damage", false);
            }
        }
    }

    public void Movement()
    {
        if (isDead) return;

        Vector2 direction = movementJoystick.Direction;

        if (direction.magnitude > 0.1f)
        {
            rb.linearVelocity = direction * speed;

            if (direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        animator.SetFloat("Speed", direction.magnitude);
    }

    public void TakeDamage(Vector2 sourcePosition)
    {
        if (isDead || isTakingDamage) return;

        isTakingDamage = true;
        knockbackTimer = knockbackDuration;

        animator.SetBool("take_damage", true);

        Vector2 knockbackDir = ((Vector2)transform.position - sourcePosition).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
    }

    public void OnDeath()
    {
        isDead = true;
        animator.SetTrigger("Death_Player");
        animator.SetBool("isDead", true);

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;

        // Cancelar ataque si esta atacando
        PlayerAttack attack = GetComponent<PlayerAttack>();
        if (attack != null)
        {
            attack.CancelAttack();
        }
    }

    public void BlockMovement()
    {
        movementBlocked = true;
    }

    public void UnblockMovement()
    {
        movementBlocked = false;
    }
}
