using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    
    public Text nameText;                    
    public Text descriptionText;                    
    public Image cardArtworkShower;                    
    public Text costText; 
    
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
        nameText.text = cardName;
        descriptionText.text = cardDescription;
        costText.text = cardCost.ToString();
        cardArtworkShower.sprite = cardArtwork;
        if (cardType == CardTypes.CHAMPION)
        {
            GetComponent<ChampionCard>().AssignChampionValues();
        }
    }
}
