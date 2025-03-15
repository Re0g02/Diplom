using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private float pullSpeed;
    private PlayerStats player;
    private CircleCollider2D magnetCollider;

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>();
        magnetCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        magnetCollider.radius = player.CurrentMagnet;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out ICollectible collectible))
        {
            var itemRB = collision.GetComponent<Rigidbody2D>();
            var pullDirection = (transform.position - collision.transform.position).normalized;

            itemRB.AddForce(pullDirection * pullSpeed);
            collectible.Collect();
        }
    }
}
