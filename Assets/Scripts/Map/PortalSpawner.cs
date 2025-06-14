using UnityEngine;

public class PortalSpawner : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;

    void Start()
    {
        int cell = Random.Range(0, 8) * 20;
        if (Random.Range(0, 2) == 1) Instantiate(portalPrefab, new Vector3(cell, 160, 0), Quaternion.identity);
        else Instantiate(portalPrefab, new Vector3(160, cell, 0), Quaternion.identity);
    }
}
