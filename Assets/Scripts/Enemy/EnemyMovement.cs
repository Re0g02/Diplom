using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private EnemyStats enemy;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
        enemy = GetComponent<EnemyStats>();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                playerTransform.position,
                                                enemy.currentMoveSpeed * Time.deltaTime);

        if ((transform.position.x < playerTransform.position.x && transform.localScale.x > 0) ||
        (transform.position.x > playerTransform.position.x && transform.localScale.x < 0))
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

    }
}
