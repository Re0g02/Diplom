using UnityEngine;

public class Portal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerStats>())
            GameManager.instance.GameWin();
    }
}
