using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LearnAboutCardsScript : MonoBehaviour
{
    [SerializeField] private Button generalButton;
    [SerializeField] private Button championButton;
    [SerializeField] private Button supportButton;

    [SerializeField] private GameObject generalContent;
    [SerializeField] private GameObject championContent;
    [SerializeField] private GameObject supportContent;

    [SerializeField] private Button generalSupportButton;
    [SerializeField] private Button instantSupportButton;
    [SerializeField] private Button lastingSupportButton;
    [SerializeField] private Button specificSupportButton;

    [SerializeField] private GameObject generalSupportContent;
    [SerializeField] private GameObject instantSupportContent;
    [SerializeField] private GameObject lastingSupportContent;
    [SerializeField] private GameObject specificSupportContent;

    private void Start()
    {
        generalButton.GetComponent<Image>().color = Color.grey;
        generalSupportButton.GetComponent<Image>().color = Color.grey;
    }

    public void BackToPickAPath()
    {
        SceneManager.LoadScene("Scenes/Pick a learning path");
    }

    public void GeneralButtonPressed()
    {
        generalContent.SetActive(true);
        championContent.SetActive(false);
        supportContent.SetActive(false);
        generalButton.GetComponent<Image>().color = Color.grey;
        championButton.GetComponent<Image>().color = Color.white;
        supportButton.GetComponent<Image>().color = Color.white;
    }

    public void ChampionButtonPressed()
    {
        generalContent.SetActive(false);
        championContent.SetActive(true);
        supportContent.SetActive(false);
        generalButton.GetComponent<Image>().color = Color.white;
        championButton.GetComponent<Image>().color = Color.grey;
        supportButton.GetComponent<Image>().color = Color.white;
    }

    public void SupportButtonPressed()
    {
        generalContent.SetActive(false);
        championContent.SetActive(false);
        supportContent.SetActive(true);
        generalButton.GetComponent<Image>().color = Color.white;
        championButton.GetComponent<Image>().color = Color.white;
        supportButton.GetComponent<Image>().color = Color.grey;
    }

    // --------------------------------------------------

    public void SupportGeneralButtonPressed()
    {
        generalSupportContent.SetActive(true);
        instantSupportContent.SetActive(false);
        lastingSupportContent.SetActive(false);
        specificSupportContent.SetActive(false);
        generalSupportButton.GetComponent<Image>().color = Color.grey;
        instantSupportButton.GetComponent<Image>().color = Color.white;
        lastingSupportButton.GetComponent<Image>().color = Color.white;
        specificSupportButton.GetComponent<Image>().color = Color.white;
    }

    public void SupportInstantButtonPressed()
    {
        generalSupportContent.SetActive(false);
        instantSupportContent.SetActive(true);
        lastingSupportContent.SetActive(false);
        specificSupportContent.SetActive(false);
        generalSupportButton.GetComponent<Image>().color = Color.white;
        instantSupportButton.GetComponent<Image>().color = Color.grey;
        lastingSupportButton.GetComponent<Image>().color = Color.white;
        specificSupportButton.GetComponent<Image>().color = Color.white;
    }

    public void SupportLastingButtonPressed()
    {
        generalSupportContent.SetActive(false);
        instantSupportContent.SetActive(false);
        lastingSupportContent.SetActive(true);
        specificSupportContent.SetActive(false);
        generalSupportButton.GetComponent<Image>().color = Color.white;
        instantSupportButton.GetComponent<Image>().color = Color.white;
        lastingSupportButton.GetComponent<Image>().color = Color.grey;
        specificSupportButton.GetComponent<Image>().color = Color.white;
    }

    public void SupportSpecificButtonPressed()
    {
        generalSupportContent.SetActive(false);
        instantSupportContent.SetActive(false);
        lastingSupportContent.SetActive(false);
        specificSupportContent.SetActive(true);
        generalSupportButton.GetComponent<Image>().color = Color.white;
        instantSupportButton.GetComponent<Image>().color = Color.white;
        lastingSupportButton.GetComponent<Image>().color = Color.white;
        specificSupportButton.GetComponent<Image>().color = Color.grey;
    }
}