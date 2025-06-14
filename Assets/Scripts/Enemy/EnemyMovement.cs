using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    protected EnemyStats enemy;
    protected Transform playerTransform;
    protected Vector2 knockbackVelocity;
    protected float knockbackDuration;
    protected Rigidbody2D enemyRB;

    protected void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        enemy = GetComponent<EnemyStats>();
        enemyRB = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (knockbackDuration > 0)
        {

            enemyRB.MovePosition(enemyRB.position + knockbackVelocity * Time.fixedDeltaTime);
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            enemyRB.MovePosition(Vector2.MoveTowards(enemyRB.position,
                                                    playerTransform.position,
                                                    enemy.currentMoveSpeed * Time.fixedDeltaTime));
        }

        if ((transform.position.x < playerTransform.position.x && transform.localScale.x > 0) ||
        (transform.position.x > playerTransform.position.x && transform.localScale.x < 0))
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    public void Knockback(Vector2 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}
