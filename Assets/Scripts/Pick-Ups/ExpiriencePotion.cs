using UnityEngine;

public class ExpiriencePotion : Pickup
{
    [SerializeField] private int _expirienceGiven;

    public override void Collect()
    {
        if (hasBeenCollected) return;
        else base.Collect();
        var player = FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(_expirienceGiven);
    }
}
