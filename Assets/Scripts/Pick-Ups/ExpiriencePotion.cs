using UnityEngine;

public class ExpiriencePotion : Pickup, ICollectible
{
    [SerializeField] private int _expirienceGiven;

    public void Collect()
    {
        var player = FindFirstObjectByType<PlayerStats>();
        player.IncreaseExperience(_expirienceGiven);
    }
}
