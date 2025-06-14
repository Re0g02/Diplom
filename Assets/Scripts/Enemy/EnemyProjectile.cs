using UnityEngine;


public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float projectileMS;
    [SerializeField] private float projectileRS;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = FindFirstObjectByType<PlayerStats>().transform;
    }

    void Update()
    {
        MoveAndRotate();
    }

    void MoveAndRotate()
    {
        if (playerTransform == null)
        {
            return;
        }

        transform.position += transform.right * projectileMS * Time.deltaTime;

        RotateZTowardsTarget();
    }

    void RotateZTowardsTarget()
    {
        Vector3 direction = playerTransform.position - transform.position;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, projectileRS * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var playerStats = collision.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(100f);
            Destroy(gameObject);
        }
    }
}
