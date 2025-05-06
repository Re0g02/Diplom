using UnityEngine;

public class PassiveItem : Item
{
    [SerializeField] private PassiveItemDataScriptableObject passiveItemData;
    [SerializeField] private PlayerDataScriptableObject.Stats currentBoosts;
    public PassiveItemDataScriptableObject PassiveItemData { get => passiveItemData; }
    public PlayerDataScriptableObject.Stats CurrentBoosts { get => currentBoosts; private set => currentBoosts = value; }

    [System.Serializable]
    public struct Modifier
    {
        public string name;
        public string description;
        public PlayerDataScriptableObject.Stats boosts;
    }

    public virtual void Initialise(PassiveItemDataScriptableObject data)
    {
        base.Initialise(data);
        this.passiveItemData = data;
        CurrentBoosts = data.BaseStats.boosts;
    }

    public virtual PlayerDataScriptableObject.Stats GetBoosts()
    {
        return CurrentBoosts;
    }

    public override bool DoLevelUp()
    {
        base.DoLevelUp();
        if (!CanLevelUp())
        {
            return false;
        }

        currentBoosts += passiveItemData.GetLevelData(++currentLevel).boosts;
        return true;
    }
}