using UnityEngine;

public class GrindstonePassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentMight *= 1 + passiveItemStats.multipler / 100f;
    }

}
