using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _chunkPrefabs;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _chunkCheckRadius;
    [SerializeField] private LayerMask _terrainMask;
    [HideInInspector] public GameObject currentChunk;
    private PlayerMovement playerMovementScript;

    [Header("Optimization")]
    [SerializeField] private float _maxOptimizationDistance;
    [SerializeField] private List<GameObject> spawnedChunks = new List<GameObject>();
    [SerializeField] private float OptimizationDuration;
    private float OptimizationCd = 0;
    private GameObject LastestChunk;

    void Start()
    {
        playerMovementScript = FindFirstObjectByType<PlayerMovement>();
    }

    void Update()
    {
        ChunkCheker();
        OptimizateChunks();
    }

    void ChunkCheker()
    {

        if (!currentChunk)
        {
            return;
        }

        var staticPoint = "Static Points/";
        staticPoint += playerMovementScript.GetMoveDirection().y > 0 ? "Up" : "";
        staticPoint += playerMovementScript.GetMoveDirection().y < 0 ? "Down" : "";
        staticPoint += playerMovementScript.GetMoveDirection().x > 0 ? "Right" : "";
        staticPoint += playerMovementScript.GetMoveDirection().x < 0 ? "Left" : "";
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(staticPoint).position, _chunkCheckRadius, _terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(staticPoint).position);
        }
    }
    void SpawnChunk(Vector3 chunkSpawnPosition)
    {
        var randomChunkIndex = UnityEngine.Random.Range(0, _chunkPrefabs.Count);
        LastestChunk = Instantiate(_chunkPrefabs[randomChunkIndex], chunkSpawnPosition, quaternion.identity);
        spawnedChunks.Add(LastestChunk);
    }

    void OptimizateChunks()
    {
        OptimizationCd -= Time.deltaTime;
        
        if (OptimizationCd > 0)
            return;
        else
            OptimizationCd = OptimizationDuration;

        foreach (var chunk in spawnedChunks)
        {
            var opDistance = Vector3.Distance(_player.transform.position, chunk.transform.position);
            if (opDistance > _maxOptimizationDistance)
                chunk.SetActive(false);
            else
                chunk.SetActive(true);
        }
    }

}
