using UnityEngine;

public class BossMovement : EnemyMovement
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootCD;
    [SerializeField] private Transform emitter;
    private float currentCD;

    void Start()
    {
        base.Start();
        currentCD = shootCD;
    }

    protected override void Update()
    {
        if (knockbackDuration > 0)
        {
            enemyRB.MovePosition(enemyRB.position + knockbackVelocity * Time.fixedDeltaTime);
            knockbackDuration -= Time.deltaTime;
        }
        else
        {
            if ((playerTransform.position - transform.position).magnitude <= 8 && currentCD <= 0)
            {
                if (currentCD <= 0) Shoot();
            }

            if ((playerTransform.position - transform.position).magnitude >= 7)
            {
                enemyRB.MovePosition(Vector2.MoveTowards(enemyRB.position,
                                                        playerTransform.position,
                                                        enemy.currentMoveSpeed * Time.fixedDeltaTime));
            }
        }

        if ((transform.position.x < playerTransform.position.x && transform.localScale.x > 0) ||
        (transform.position.x > playerTransform.position.x && transform.localScale.x < 0))
        {
            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        currentCD -= Time.deltaTime;
    }

    private void Shoot()
    {
        var dir = playerTransform.position - emitter.position;
        var obj = Instantiate(projectilePrefab, emitter.position, Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
        Destroy(obj, 5f);
        currentCD = shootCD;
    }
}
