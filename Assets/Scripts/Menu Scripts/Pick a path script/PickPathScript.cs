using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickPathScript : MonoBehaviour
{
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
    
    public void LearnAboutTheBoard()
    {
        SceneManager.LoadScene("Scenes/Learning Paths/Learn About the board");
    }
    
    public void LearnAboutTheCards()
    {
        SceneManager.LoadScene("Scenes/Learning Paths/Learn About the cards");
    }
    

    
    
}
