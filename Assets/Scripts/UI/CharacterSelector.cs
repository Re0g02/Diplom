using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    private PlayerDataScriptableObject playerStats;
    private CharacterStatsTable statsTable;
    [SerializeField] private PlayerDataScriptableObject[] characterArray;

    void Awake()
    {
         if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        statsTable = FindFirstObjectByType<CharacterStatsTable>();
        SelectCharacter(characterArray[Random.Range(0, characterArray.Length)]);
    }
    public static PlayerDataScriptableObject GetPlayerData()
    {
        if (instance && instance.playerStats)
        {
            return instance.playerStats;
        }
        return null;
    }

    public void SelectCharacter(PlayerDataScriptableObject player)
    {
        playerStats = player;
        statsTable.SetStatsTableInfo(player);
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
