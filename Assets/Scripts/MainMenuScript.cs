using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("NewMovement");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
