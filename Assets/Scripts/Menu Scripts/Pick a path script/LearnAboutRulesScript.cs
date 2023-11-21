using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LearnAboutRulesScript : MonoBehaviour
{
    public void BackToPickAPath()
    {
        SceneManager.LoadScene("Scenes/Pick a learning path");
    }
}
