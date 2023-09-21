using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ThisCard : MonoBehaviour
{
    public int cardIdd;
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private Sprite cardArtwork;
    [SerializeField] private int cardCost;
    [SerializeField] private int cardPower;
    [SerializeField] private int cardHealth;

    public Text nameText;
    public Text descriptionText;
    public Image cardArtworkShower;
    public Text costText;
    public Text powerText;
    public Text healthText;

    private void Start()
    {
        Debug.Log("start is run");
       // Clone the card from the database
        /*CopyCardData(CardDataBase.cardList[thisCardId], thisCard);*/

        AssignValuesToCard();
    }

    private void Update()
    {
        AssignValuesToCard();


        /*if (cardHealth == 0)   // here is where we need this kind of checks
        {
            Destroy(gameObject);
        }*/
    }

    [ContextMenu("Take Damage")]
    public void TakeSomeDamage()
    {
        cardHealth -= 1; // Reduce the health of the specific card instance. now it works

        if (cardHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void AssignValuesToCard()
    {
        nameText.text = cardName;
        descriptionText.text = cardDescription;
        costText.text = cardCost.ToString();
        powerText.text = cardPower.ToString();
        healthText.text = cardHealth.ToString();
    }


}