using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    private class Drops
    {
        public string name;
        public GameObject prefab;
        public float dropRate;
    }

    [SerializeField] private List<Drops> drops;
    private bool isQuiting = false;

    void OnApplicationQuit()
    {
        isQuiting = true;
    }

    void OnDestroy()
    {
        if (!isQuiting)
        {
            var randomNumber = UnityEngine.Random.Range(0f, 100f);
            var posibleDrops = new List<Drops>();

            foreach (var rate in drops)
            {
                if (randomNumber <= rate.dropRate)
                {
                    posibleDrops.Add(rate);
                }
            }
            if (posibleDrops.Count > 0)
            {
                var drops = posibleDrops[UnityEngine.Random.Range(0, posibleDrops.Count)].prefab;
                Instantiate(drops, transform.position, quaternion.identity);
            }
        }
    }
}
