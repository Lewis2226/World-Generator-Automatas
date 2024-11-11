using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public string sceneName;

    public void ResetLevel()
    {
        SceneManager.LoadScene(sceneName);
    }
}
