using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayScript : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }

    public GameObject[] howToPlaySteps;
    
    private int currentStep = 0;

    public void DisplayNextStep()
    {
        howToPlaySteps[currentStep].SetActive(false);
        currentStep++;
        if (currentStep == howToPlaySteps.Length)
        {
            currentStep = 0;
        }
        howToPlaySteps[currentStep].SetActive(true);
    }
    
    public void DisplayPrevStep()
    {
        howToPlaySteps[currentStep].SetActive(false);
        currentStep--;
        if (currentStep < 0)
        {
            currentStep = howToPlaySteps.Length - 1;
        }
        howToPlaySteps[currentStep].SetActive(true);
    }
    

}
