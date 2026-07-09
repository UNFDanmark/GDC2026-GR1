using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneThing : MonoBehaviour
{
    public void SwitchScene(int buildIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(buildIndex);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
