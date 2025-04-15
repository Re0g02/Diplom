using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private EnemyStats enemy;
    private Transform playerTransform;
    private Vector2 knockbackVelocity;
    private float knockbackDuration;

    void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        enemy = GetComponent<EnemyStats>();
    }

    void Update()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position,
                                                    playerTransform.position,
                                                    enemy.currentMoveSpeed * Time.deltaTime);
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
