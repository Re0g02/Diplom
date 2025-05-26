using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SceneSelect(string name)
    {
        if (Time.timeScale != 0)
            StartCoroutine(Scene(name));
        else SceneManager.LoadScene(name);
    }

    public void ExitGame()
    {
        StartCoroutine(Exit());
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(0.5f);
        // EditorApplication.isPlaying = false;
        Application.Quit(1);
    }

    IEnumerator Scene(string name)
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(name);
    }
}
