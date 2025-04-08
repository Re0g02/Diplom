using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _chunkPrefabs;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _chunkCheckRadius;
    [SerializeField] private LayerMask _terrainMask;
    [SerializeField] private GameObject _ChunksContainer;
    [HideInInspector] public GameObject currentChunk;


    [Header("Optimization")]
    [SerializeField] private float _maxOptimizationDistance;
    [SerializeField] private List<GameObject> spawnedChunks = new List<GameObject>();
    [SerializeField] private float OptimizationDuration;
    private float OptimizationCd = 0;
    private Vector3 playerLastPosition;

    void Start()
    {
        playerLastPosition = _player.transform.position;
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
            SpawnChunk(currentChunk.transform.Find("Static Points/" + direction).position);
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
    void SpawnChunk(Vector3 chunkSpawnPosition)
    {
        var randomChunkIndex = UnityEngine.Random.Range(0, _chunkPrefabs.Count);
        var LastestChunk = Instantiate(_chunkPrefabs[randomChunkIndex], chunkSpawnPosition, quaternion.identity);
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