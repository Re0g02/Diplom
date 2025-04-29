using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    private PlayerDataScriptableObject playerStats;

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

    public static PlayerDataScriptableObject GetData()
    {
        if (instance && instance.playerStats)
        {
            return instance.playerStats;
        }
        else
        {
            PlayerDataScriptableObject[] players = Resources.FindObjectsOfTypeAll<PlayerDataScriptableObject>();
            if (players.Length > 0)
            {
                return players[Random.Range(0, players.Length)];
            }
        }
        return null;
    }

    public void SelectCharacter(PlayerDataScriptableObject player)
    {
        playerStats = player;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
