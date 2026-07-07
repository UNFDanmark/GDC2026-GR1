using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneThing : MonoBehaviour
{
    public void SwitchScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
