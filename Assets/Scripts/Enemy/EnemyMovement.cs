using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] private EnemyScriptableObject enemyStats;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>().transform;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                playerTransform.position,
                                                enemyStats.speed * Time.deltaTime);

        if ((transform.position.x < playerTransform.position.x && transform.localScale.x > 0)||
        (transform.position.x > playerTransform.position.x && transform.localScale.x < 0))
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

    }
}
