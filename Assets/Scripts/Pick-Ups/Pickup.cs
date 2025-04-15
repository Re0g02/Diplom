using UnityEngine;

public class Pickup : MonoBehaviour, ICollectible
{
    protected bool hasBeenCollected = false;
    public bool HasBeenCollected { get => hasBeenCollected; }

    public virtual void Collect()
    {
        hasBeenCollected = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerStats>())
        {
            Destroy(gameObject);
        }
    }
}
