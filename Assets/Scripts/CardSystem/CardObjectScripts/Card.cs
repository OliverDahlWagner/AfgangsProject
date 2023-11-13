using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum CardTypes
{
    CHAMPION,
    SUPPORT
}
public class Card : MonoBehaviour
{
    public int cardId;
    public CardTypes cardType;

    [SerializeField] private string cardName;                    
    [SerializeField] private string cardDescription;                   
    [SerializeField] private Sprite cardArtwork;                   
    [SerializeField] public int cardCost;
    
    public TMP_Text nameText;            
    public Text descriptionText;                    
    public Image cardArtworkShower;                    
    public TMP_Text costText; 
    
    private void Start()
    {
        AssignValuesToCard();
    }

    private void Update()
    {
        AssignValuesToCard();
    }
    
    private void AssignValuesToCard()                    
    {
        nameText.SetText(cardName.ToString());
        descriptionText.text = cardDescription;
        costText.SetText(cardCost.ToString());
        cardArtworkShower.sprite = cardArtwork;
        if (cardType == CardTypes.CHAMPION)
        {
            GetComponent<ChampionCard>().AssignChampionValues();
        }
    }
}
