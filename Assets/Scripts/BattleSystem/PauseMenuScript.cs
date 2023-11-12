using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    public Image pauseScreen;

    private BattleState previusState;

    public void StartPause()    // I think there will need to be some modifications. when there is add animations to the AI
    {
        GetComponent<BattleSystem>().isPaused = true;
        pauseScreen.gameObject.SetActive(true);
        Time.timeScale = 0f; // Because the animations are based framerate/time set that shit to 0 and it wont move
    }
    
    public void EndPause()    // I think there will need to be some modifications. when there is add animations to the AI
    {
        GetComponent<BattleSystem>().isPaused = false;
        pauseScreen.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
    
    public void Restart()
    {
        SceneManager.LoadScene("Scenes/BattleScene");
    }
}
