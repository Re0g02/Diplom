using UnityEngine;

public class LevelSelector : MonoBehaviour
{

    public static LevelSelector instance;
    private LevelDataScriptableObject levelStats;
    [SerializeField] private LevelDataScriptableObject[] level;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SelectLevel(level[Random.Range(0,level.Length)]);
    }

    public static LevelDataScriptableObject GetLevelData()
    {
        if (instance && instance.levelStats)
        {
            return instance.levelStats;
        }
        return null;
    }

    public void SelectLevel(LevelDataScriptableObject level)
    {
        levelStats = level;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}


