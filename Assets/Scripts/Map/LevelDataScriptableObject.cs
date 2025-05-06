using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level Data")]
public class LevelDataScriptableObject : ScriptableObject
{
    [SerializeField] private String levelName;
    [SerializeField] private String levelDescription;
    [SerializeField] private Sprite levelPreview;
    [SerializeField] private List<GameObject> chunkPrefabs;
    [SerializeField] private GameObject startingChunk;
    public List<GameObject> LevelPrefabs { get => chunkPrefabs; }
    public GameObject StartingChunk { get => startingChunk; }

}
