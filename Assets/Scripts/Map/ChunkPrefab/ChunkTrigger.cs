using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    [SerializeField] private GameObject targetMap;
    private MapController mapControllerScript;

    void Start()
    {
        mapControllerScript = FindFirstObjectByType<MapController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            mapControllerScript.currentChunk = targetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            if (mapControllerScript.currentChunk == targetMap)
            {
                mapControllerScript.currentChunk = null;
            }
        }
    }
}
