using UnityEngine;

public class Magnet : Pickup
{
    private GameObject player;
    private Rigidbody2D itemRB;

    void Start()
    {
        player = FindFirstObjectByType<PlayerStats>().gameObject;
        itemRB = GetComponent<Rigidbody2D>();
    }
    
    public override void Collect()
    {
        if (hasBeenCollected) return;
        else base.Collect();
    }

    void Update()
    {
        if (hasBeenCollected)
        {
            var pullDirection = (player.transform.position - transform.position).normalized;
            itemRB.linearVelocity = (pullDirection * 2500 * Time.deltaTime);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerStats>())
        {
            var objects = FindObjectsByType<ExpiriencePotion>(FindObjectsSortMode.None);
            foreach (var obj in objects) obj.Collect();
            Destroy(gameObject);
        }
    }
}
