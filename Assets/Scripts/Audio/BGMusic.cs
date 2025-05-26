using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusic : MonoBehaviour
{
    public static GameObject instance;

    void Start()
    {
        if (BGMusic.instance == null)
        {
            BGMusic.instance = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) Destroy(gameObject);
    }

}
