using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class SelectCards : MonoBehaviour
{
    public List<GameObject> NewWayOfPicking(List<GameObject> playableCards, int currentMana)
    {
        var chosenCards = new List<GameObject>();
        var manaLimit = currentMana;

        var spotsLeft = GetComponent<AiBasicFunctions>().AvailableDropZonesIndexes().Count;

        var cardsSorted = CardsSortedByValue(playableCards);

        
        
        for (int i = 0; i < cardsSorted.Count; i++)
        {
            manaLimit -= cardsSorted[i].GetComponent<Card>().cardCost;
            if (manaLimit >= 0)
            {
                if (cardsSorted[i].GetComponent<Card>().cardType == CardTypes.CHAMPION)
                {
                    spotsLeft -= 1;
                    if (spotsLeft > 0)
                    {
                        chosenCards.Add(cardsSorted[i]);
                    }
                }
                
                if (cardsSorted[i].GetComponent<Card>().cardType == CardTypes.SUPPORT)
                {
                    if (cardsSorted[i].GetComponent<SupportCard>().supCardType == SupCardTypes.INSTANT ||
                        cardsSorted[i].GetComponent<SupportCard>().supCardType == SupCardTypes.SPECIFIC)
                    {
                        chosenCards.Add(cardsSorted[i]);
                    }

                    spotsLeft -= 1;
                    if (cardsSorted[i].GetComponent<SupportCard>().supCardType == SupCardTypes.LASTING)
                    {
                        if (spotsLeft > 0)
                        {
                            chosenCards.Add(cardsSorted[i]);
                        }
                    }
                }
            }
        }
        
        
        
        chosenCards.Sort((card1, card2) =>
        {
            var type1 = card1.GetComponent<Card>().cardType;
            var type2 = card2.GetComponent<Card>().cardType;
            
            if (type1 == CardTypes.CHAMPION && type2 == CardTypes.SUPPORT)
            {
                return -1; 
            }

            if (type1 == CardTypes.SUPPORT && type2 == CardTypes.CHAMPION)
            {
                return 1; 
            }
   
            return 0; 
            
        });
        
        
        var champCount = 0;
        foreach (var card in chosenCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                champCount++;
            }
        }
        if (champCount == 0 && GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count == 0)
        {
            chosenCards.Clear();
        }

        Debug.Log(chosenCards.Count + "count of picked cards");
        return chosenCards;
    }

    public List<GameObject>
        CardsSortedByValue(List<GameObject> playableCards) // a away to sort the card by their value.
    {
        return playableCards.OrderByDescending(card => card.GetComponent<Card>().GetValue()).ToList();
    }
}