using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector instance;
    private PlayerScriptableObject characterStats;

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

    public static PlayerScriptableObject GetData()
    {
        return instance.characterStats;
    }

    public void SelectCharacter(PlayerScriptableObject character)
    {
        characterStats = character;
    }

    public void DestroySingleton(){
        instance = null;
        Destroy(gameObject);
    }
}
