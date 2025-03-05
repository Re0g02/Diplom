using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    [SerializeField] private float health;
    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0) {
            Kill();
         }
    }

    private void Kill(){
        Destroy(gameObject);
    }
}
