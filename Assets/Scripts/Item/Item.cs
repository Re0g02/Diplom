using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected int currentLevel = 1;
    protected int maxLevel = 1;
    protected ItemDataScriptableObject.Evolution[] evolutionData;
    protected PlayerInventory inventory;
    protected PlayerStats playerStats;

    public int CurrentLevel { get => currentLevel; }

    public virtual void Initialise(ItemDataScriptableObject stats)
    {
        maxLevel = stats.MaxLevel;
        evolutionData = stats.EvolutionData;
        inventory = FindFirstObjectByType<PlayerInventory>();
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    public virtual bool CanLevelUp()
    {
        return currentLevel <= maxLevel;
    }

    public virtual bool DoLevelUp()
    {
        if (evolutionData == null) return true;
        foreach (ItemDataScriptableObject.Evolution e in evolutionData)
        {
            if (e.condition == ItemDataScriptableObject.Evolution.Condition.auto)
                AttemptEvolution(e);
        }
        return true;
    }

    public virtual void OnEquip()
    {
    }

    public virtual void OnUnequip()
    {
    }

    public virtual ItemDataScriptableObject.Evolution[] CanEvolve()
    {
        List<ItemDataScriptableObject.Evolution> possibleEvolutions = new List<ItemDataScriptableObject.Evolution>();
        foreach (ItemDataScriptableObject.Evolution e in evolutionData)
        {
            if (CanEvolve(e)) possibleEvolutions.Add(e);
        }
        return possibleEvolutions.ToArray();
    }

    public virtual bool CanEvolve(ItemDataScriptableObject.Evolution evolution, int levelUpAmount = 1)
    {
        if (evolution.evolutionLevel > currentLevel + levelUpAmount)
        {
            return false;
        }
        foreach (ItemDataScriptableObject.Evolution.Config c in evolution.catalysts)
        {
            Item item = inventory.Get(c.itemType);
            if (!item || item.currentLevel < c.level)
            {
                return false;
            }
        }
        return true;
    }

    public virtual bool AttemptEvolution(ItemDataScriptableObject.Evolution evolutionData, int levelUpAmount = 1)
    {
        if (!CanEvolve(evolutionData, levelUpAmount))
            return false;
        bool consumePassives = (evolutionData.consumes & ItemDataScriptableObject.Evolution.Consumption.passives) > 0;
        bool consumeWeapons = (evolutionData.consumes & ItemDataScriptableObject.Evolution.Consumption.weapons) > 0;

        foreach (ItemDataScriptableObject.Evolution.Config c in evolutionData.catalysts)
        {
            if (c.itemType is PassiveItemDataScriptableObject && consumePassives) inventory.Remove(c.itemType, true);
            if (c.itemType is WeaponDataScriptableObject && consumeWeapons) inventory.Remove(c.itemType, true);
        }
        if (this is PassiveItem && consumePassives) inventory.Remove(((this as PassiveItem).PassiveItemData), true);
        else if (this is Weapon && consumeWeapons) inventory.Remove(((this as Weapon).WeaponData), true);
        inventory.Add(evolutionData.outcome.itemType);
        return true;
    }
}