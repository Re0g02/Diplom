using UnityEngine;

public class ItemDataScriptableObject : ScriptableObject
{
    [SerializeField] protected Sprite icon;
    [SerializeField] protected int maxLevel;
    [SerializeField] protected Evolution[] evolutionData;

    public Sprite Icon { get => icon; }
    public int MaxLevel { get => maxLevel; }
    public Evolution[] EvolutionData { get => evolutionData; }

    [System.Serializable]
    public struct Evolution
    {
        public string name;
        public enum Condition { auto, treasureChest }
        public Condition condition;
        [System.Flags] public enum Consumption { passives = 1, weapons = 2 }
        public Consumption consumes;
        public int evolutionLevel;
        public Config[] catalysts;
        public Config outcome;
        [System.Serializable]
        public struct Config
        {
            public ItemDataScriptableObject itemType;
            public int level;
        }
    }

}
