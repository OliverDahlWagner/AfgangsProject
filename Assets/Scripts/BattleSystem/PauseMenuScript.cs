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
    }
    
    public void EndPause()    // I think there will need to be some modifications. when there is add animations to the AI
    {
        GetComponent<BattleSystem>().isPaused = false;
        pauseScreen.gameObject.SetActive(false);
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
