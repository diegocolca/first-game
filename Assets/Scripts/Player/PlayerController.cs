using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public Joystick movementJoystick;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Dash Settings")]
    private bool canDash = true;
    private bool isDashing;
    public float dashPower = 24f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private Transform trailOrigin;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f;

    private bool isTakingDamage = false;
    private bool isDead = false;
    private float knockbackTimer;

    private bool movementBlocked = false;

    private HealthPlayer healthPlayer;
    private Vector2 lastMoveDirection = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthPlayer = GetComponent<HealthPlayer>();

        if (dashTrail != null)
        {
            dashTrail.sortingLayerID = spriteRenderer.sortingLayerID;
            dashTrail.sortingOrder = spriteRenderer.sortingOrder + 1;
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;
        if (isDashing) return;

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
        UpdateAttackPointDirection();
    }

    public void UpdateAttackPointDirection()
    {
        PlayerAttack attack = GetComponent<PlayerAttack>();
        if (attack != null)
        {
            attack.attackPoint.localPosition = new Vector2(lastMoveDirection.x * 0.5f, lastMoveDirection.y * 0.5f); 
        }
    }

    public void Movement()
    {
        if (isDead) return;

        Vector2 direction = movementJoystick.Direction;

        if (direction.magnitude > 0.1f)
        {
            rb.linearVelocity = direction * speed;
            lastMoveDirection = direction.normalized;

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
        GetComponent<Collider2D>().enabled = false; // Desactivar colision al morir
    }

    public void BlockMovement()
    {
        movementBlocked = true;
    }

    public void UnblockMovement()
    {   
        movementBlocked = false;
    }

    public IEnumerator Dash()
    {

        if (!canDash) yield break;
        canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", true);
        dashTrail.emitting = true;
        Vector2 dashDirection = movementJoystick.Direction.normalized;
        if (dashDirection == Vector2.zero)
            dashDirection = new Vector2(transform.localScale.x, 0f);
            Debug.Log("Dashing in direction: " + dashDirection);

        if (trailOrigin != null)
        {
            // Offset the trail origin behind the player
            Vector2 dashDir = dashDirection; // Already normalized
            float trailOffset = -0.5f; // Adjust as needed
            trailOrigin.localPosition = new Vector3(dashDir.x * trailOffset, dashDir.y * trailOffset, 0f);
        }

        rb.linearVelocity = dashDirection * dashPower;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        if (dashTrail != null)
            dashTrail.emitting = false;
        animator.SetBool("isDashing", false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        Debug.Log("Dash completed");
    }
}
