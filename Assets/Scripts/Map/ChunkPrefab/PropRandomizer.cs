using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{

    [SerializeField] private List<GameObject> _propSpawnPoints;
    [SerializeField] private List<GameObject> _propPrefabs;
    void Start()
    {
        SpawnRandomProps();
    }

    void SpawnRandomProps()
    {
        foreach (var spawnPoint in _propSpawnPoints)
        {
            var propIndex = Random.Range(0, _propPrefabs.Count);
            var prop = Instantiate(_propPrefabs[propIndex], spawnPoint.transform.position, Quaternion.identity);
            prop.transform.parent = spawnPoint.transform;
        }
    }
}