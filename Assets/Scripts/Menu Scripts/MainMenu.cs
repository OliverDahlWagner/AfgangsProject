using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void HowToPlay()
    {
        SceneManager.LoadScene("Scenes/Pick a learning path");
    }
    
    public void Options()
    {
        SceneManager.LoadScene("Scenes/Options");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}