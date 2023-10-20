using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WonLostScreenScript : MonoBehaviour
{

    public Image wonLostScreen;
    public Text wonLostText;

    public void GameConclusion()
    {
        wonLostScreen.gameObject.SetActive(true);

        if (GetComponent<BattleSystem>().state == BattleState.WON)
        {
            wonLostText.text = "You Won !!!";
        }
        
        if (GetComponent<BattleSystem>().state == BattleState.LOST)
        {
            wonLostText.text = "You Lost :(";
        }
    }
    

    public void BackToMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
    
    public void PlayAgain()
    {
        SceneManager.LoadScene("Scenes/BattleScene");
    }
    
    
}
