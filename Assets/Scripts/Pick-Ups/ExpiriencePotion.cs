using System.Collections;
using UnityEngine;

public class ExpiriencePotion : Pickup
{
    [SerializeField] private int _expirienceGiven;
    [SerializeField] private GameObject expSound;
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
            Destroy(Instantiate(expSound, transform.position, Quaternion.identity), 0.5f);
            var player = FindFirstObjectByType<PlayerStats>();
            player.IncreaseExperience(_expirienceGiven);
            Destroy(gameObject);
        }
    }
}
