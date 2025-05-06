using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneSelect(string name)
    {
        SceneManager.LoadScene(name);
    }
}
