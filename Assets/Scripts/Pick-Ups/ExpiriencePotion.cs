using UnityEngine;

public class ExpiriencePotion : Pickup
{
    [SerializeField] private int _expirienceGiven;

    public override void Collect()
    {
        if (hasBeenCollected) return;
        else base.Collect();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerStats>())
        { 
            var player = FindFirstObjectByType<PlayerStats>();
            player.IncreaseExperience(_expirienceGiven);
            Destroy(gameObject);
        }
    }
}
