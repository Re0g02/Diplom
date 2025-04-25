using UnityEngine;

public class HealItem : Pickup
{
    [SerializeField] private int _healthToHeal;

    public override void Collect()
    {
        if (hasBeenCollected) return;
        else base.Collect();
        var player = FindFirstObjectByType<PlayerStats>();
        player.RestoreHealth(_healthToHeal);
    }
}
