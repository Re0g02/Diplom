using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/PassiveItem Data")]
public class PassiveItemDataScriptableObject : ItemDataScriptableObject
{
    [SerializeField] private PassiveItem.Modifier baseStats;
    [SerializeField] private PassiveItem.Modifier[] growth;
    public PassiveItem.Modifier BaseStats { get => baseStats; }
    public PassiveItem.Modifier GetLevelData(int level)
    {
        if (level - 2 < growth.Length)
        {
            return growth[level - 2];
        }
        return new PassiveItem.Modifier();
    }
}