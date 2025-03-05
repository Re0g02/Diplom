using UnityEngine;

public class HealItem : Pickup, ICollectible
{
    [SerializeField] private int _healthToHeal;

    public void Collect()
    {
        var player = FindFirstObjectByType<PlayerStats>();
        player.Heal(_healthToHeal);
    }
}
