using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public void PlayButton() {
        SceneManager.LoadScene("AwakeScene");
    }

    public void QuitButton() {
        Application.Quit();
    }
}
