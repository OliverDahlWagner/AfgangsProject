using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LearnAboutBoardScript : MonoBehaviour
{
    
    [SerializeField] private Button generalButton;
    [SerializeField] private Button avatarButton;
    [SerializeField] private Button handDeckButton;
    [SerializeField] private Button playAreaButton;

    [SerializeField] private GameObject generalContent;
    [SerializeField] private GameObject avatarContent;
    [SerializeField] private GameObject handDeckContent;
    [SerializeField] private GameObject playAreaContent;
    
    [SerializeField] private Button playerHandButton;
    [SerializeField] private Button deckButton;

    [SerializeField] private GameObject playerHandContent;
    [SerializeField] private GameObject deckContent;


    private void Start()
    {
        generalButton.GetComponent<Image>().color = Color.grey;
        playerHandButton.GetComponent<Image>().color = Color.grey;
    }

    public void BackToPickAPath()
    {
        SceneManager.LoadScene("Scenes/Pick a learning path");
    }
    
    public void GeneralButtonPressed()
    {
        generalContent.SetActive(true);
        avatarContent.SetActive(false);
        handDeckContent.SetActive(false);
        playAreaContent.SetActive(false);
        generalButton.GetComponent<Image>().color = Color.grey;
        avatarButton.GetComponent<Image>().color = Color.white;
        handDeckButton.GetComponent<Image>().color = Color.white;
        playAreaButton.GetComponent<Image>().color = Color.white;
    }

    public void AvatarButtonPressed()
    {
        generalContent.SetActive(false);
        avatarContent.SetActive(true);
        handDeckContent.SetActive(false);
        playAreaContent.SetActive(false);
        generalButton.GetComponent<Image>().color = Color.white;
        avatarButton.GetComponent<Image>().color = Color.grey;
        handDeckButton.GetComponent<Image>().color = Color.white;
        playAreaButton.GetComponent<Image>().color = Color.white;
    }

    public void HandDeckButtonPressed()
    {
        generalContent.SetActive(false);
        avatarContent.SetActive(false);
        handDeckContent.SetActive(true);
        playAreaContent.SetActive(false);
        generalButton.GetComponent<Image>().color = Color.white;
        avatarButton.GetComponent<Image>().color = Color.white;
        handDeckButton.GetComponent<Image>().color = Color.grey;
        playAreaButton.GetComponent<Image>().color = Color.white;
    }
    
    public void PlayAreaButtonPressed()
    {
        generalContent.SetActive(false);
        avatarContent.SetActive(false);
        handDeckContent.SetActive(false);
        playAreaContent.SetActive(true);
        generalButton.GetComponent<Image>().color = Color.white;
        avatarButton.GetComponent<Image>().color = Color.white;
        handDeckButton.GetComponent<Image>().color = Color.white;
        playAreaButton.GetComponent<Image>().color = Color.grey;
    }
    
    // --------------------------------------
    
    public void PlayerHandButtonPressed()
    {
        playerHandContent.SetActive(true);
        deckContent.SetActive(false);
        playerHandButton.GetComponent<Image>().color = Color.grey;
        deckButton.GetComponent<Image>().color = Color.white;
    }

    public void PlayerDeckButtonPressed()
    {
        playerHandContent.SetActive(false);
        deckContent.SetActive(true);
        playerHandButton.GetComponent<Image>().color = Color.white;
        deckButton.GetComponent<Image>().color = Color.grey;
    }
    
}
