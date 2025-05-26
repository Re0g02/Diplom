using System.Collections.Generic;
using UnityEngine;

public class FallingItem : MonoBehaviour
{
    [SerializeField] private List<Sprite> itemList;
    [SerializeField] private float spawnInterval = 3;
    [SerializeField] private float spawnCD = 0.01f;
    [SerializeField] private GameObject itemPrefab;
    private float maxOffset = 12f;
    private float curentOffset;
    private float currentCD;

    void Start()
    {
        Time.timeScale = 1;
        currentCD = spawnCD;
        curentOffset = -maxOffset;
    }

    void Update()
    {
        if (currentCD <= 0)
        {
            var item = Instantiate(itemPrefab, new Vector3(curentOffset + Random.Range(-1.2f, 1.2f), 7, 0), Quaternion.identity);
            item.GetComponent<SpriteRenderer>().sprite = itemList[Random.Range(0, itemList.Count)];
            item.GetComponent<Rigidbody2D>().linearVelocityY = -Random.Range(2.8f,3.2f);
            Destroy(item, 5f);
            currentCD = spawnCD;
            curentOffset += spawnInterval;
        }
        if (curentOffset > maxOffset) curentOffset = -maxOffset;
        currentCD -= Time.deltaTime;
    }
}
