using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerInventory p = col.GetComponent<PlayerInventory>();
        if (p)
        {
            bool randomBool = Random.Range(0, 2) == 0;
            OpenTreasureChest(p, randomBool);
            Destroy(gameObject);
        }
    }

    public void OpenTreasureChest(PlayerInventory inventory, bool isHigherTier)
    {
        foreach (PlayerInventory.Slot s in inventory.WeaponSlots)
        {
            Weapon w = s.item as Weapon;
            if (w.WeaponData.EvolutionData == null) continue;
            foreach (ItemDataScriptableObject.Evolution e in w.WeaponData.EvolutionData)
            {
                if (e.condition == ItemDataScriptableObject.Evolution.Condition.treasureChest)
                {
                    bool attempt = w.AttemptEvolution(e, 0);
                    if (attempt) return;
                }
            }
        }
    }
}