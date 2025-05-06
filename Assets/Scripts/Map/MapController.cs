using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapController : MonoBehaviour
{
    private List<GameObject> _chunkPrefabs;
    [SerializeField] private float _chunkCheckRadius;
    [SerializeField] private LayerMask _terrainMask;
    [SerializeField] private GameObject _ChunksContainer;
    [HideInInspector] public GameObject currentChunk;


    [Header("Optimization")]
    [SerializeField] private float _maxOptimizationDistance;
    private List<GameObject> spawnedChunks = new List<GameObject>();
    [SerializeField] private float OptimizationDuration;
    private float OptimizationCd = 0;
    private Vector3 playerLastPosition;
    private GameObject _player;
    private LevelDataScriptableObject levelData;
    private GameObject startingChunk;

    void Start()
    {
        _player = FindFirstObjectByType<PlayerStats>().gameObject;
        playerLastPosition = _player.transform.position;
        levelData = LevelSelector.GetLevelData();
        LevelSelector.instance.DestroySingleton();
        _chunkPrefabs = levelData.LevelPrefabs;
        startingChunk = levelData.StartingChunk;
        SpawnChunk(_player.transform.position, null);
        CheckAndSpawnChunk("Left");
        CheckAndSpawnChunk("Right");
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

        var moveDirection = _player.transform.position - playerLastPosition;
        playerLastPosition = _player.transform.position;

        string directionName = GetDirectionName(moveDirection);
        CheckAndSpawnChunk(directionName);

        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up");
            if (directionName == "Up")
            {
                CheckAndSpawnChunk("UpRight");
                CheckAndSpawnChunk("UpLeft");
            }
        }
        if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
            if (directionName == "Right")
            {
                CheckAndSpawnChunk("UpRight");
                CheckAndSpawnChunk("DownRight");
            }
        }
        if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
            if (directionName == "Left")
            {
                CheckAndSpawnChunk("UpLeft");
                CheckAndSpawnChunk("DownLeft");
            }
        }
        if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
            if (directionName == "Down")
            {
                CheckAndSpawnChunk("DownRight");
                CheckAndSpawnChunk("DownLeft");
            }
        }
    }

    void CheckAndSpawnChunk(string direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Static Points/" + direction).position, _chunkCheckRadius, _terrainMask))
            SpawnChunk(currentChunk.transform.Find("Static Points/" + direction).position, direction);
    }
    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.y > 0.5f)
            {
                return direction.x > 0 ? "UpRight" : "UpLeft";
            }
            else if (direction.y < -0.5f)
            {
                return direction.x > 0 ? "DownRight" : "DownLeft";
            }
            else
            {
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            if (direction.x > 0.5f)
            {
                return direction.y > 0 ? "UpRight" : "DownRight";
            }
            else if (direction.x < -0.5f)
            {
                return direction.y > 0 ? "UpLeft" : "DownLeft";
            }
            else
            {
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 chunkSpawnPosition, string direction)
    {
        GameObject LastestChunk;
        if (currentChunk == null)
        {
            LastestChunk = Instantiate(startingChunk, chunkSpawnPosition, quaternion.identity);
            LastestChunk.GetComponent<AvailableChunks>().SetUpAndDown(startingChunk.GetComponent<AvailableChunks>().GetChunkByDirection("Up"),
                                                                    startingChunk.GetComponent<AvailableChunks>().GetChunkByDirection("Down"));
            currentChunk = LastestChunk;
        }
        else
        {
            var chunkToSpawn = currentChunk.GetComponent<AvailableChunks>().GetChunkByDirection(direction);
            if (chunkToSpawn == null)
            {
                var randomChunkIndex = UnityEngine.Random.Range(0, _chunkPrefabs.Count);
                chunkToSpawn = _chunkPrefabs[randomChunkIndex];
            }
            LastestChunk = Instantiate(chunkToSpawn, chunkSpawnPosition, quaternion.identity);
            LastestChunk.GetComponent<AvailableChunks>().SetUpAndDown(chunkToSpawn.GetComponent<AvailableChunks>().GetChunkByDirection("Up"),
                                                                    chunkToSpawn.GetComponent<AvailableChunks>().GetChunkByDirection("Down"));
        }
        spawnedChunks.Add(LastestChunk);
        LastestChunk.transform.parent = _ChunksContainer.transform;
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