public class BootsPassiveItem : PassiveItem
{
    protected override void ApplyModifier()
    {
        playerStats.CurrentMoveSpeed *= 1 + passiveItemStats.multipler / 100f;
    }
}