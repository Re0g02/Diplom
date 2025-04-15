using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private InvetntoryManager playerInventory;

    private void Start()
    {
        playerInventory = FindFirstObjectByType<InvetntoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>())
        {
            OpenChest();
            Destroy(gameObject);
        }
    }

    private void OpenChest()
    {
        var evolvingWeaponList = playerInventory.GetPossibleEvolutions();
        if (evolvingWeaponList.Count == 0) return;
        
        var evolvingWeapon = evolvingWeaponList[Random.Range(0, evolvingWeaponList.Count)];
        playerInventory.EvolveWeapon(evolvingWeapon);
    }
}
