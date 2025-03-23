using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreen : MonoBehaviour
{
    public void PlayButton() {
        SceneManager.LoadScene("Garage");
    }

    public void QuitButton() {
        Application.Quit();
    }
}
