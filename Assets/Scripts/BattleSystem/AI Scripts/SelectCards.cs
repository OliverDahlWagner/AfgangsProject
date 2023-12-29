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


        if (GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count == 0)
        {
            var bestChampCard = new GameObject();
            
            for (int i = 0; i < cardsSorted.Count; i++)
            {
                if (cardsSorted[i].GetComponent<Card>().cardType == CardTypes.CHAMPION)
                {

                    if (bestChampCard.GetComponent<Card>() == null)
                    {
                        bestChampCard = cardsSorted[i].gameObject;
                    }
                    else
                    {
                        if (bestChampCard.GetComponent<Card>().GetValue() < cardsSorted[i].GetComponent<Card>().GetValue())
                        {
                            bestChampCard = cardsSorted[i].gameObject;
                        }
                    }
                    
                }
                
            }

            cardsSorted.Remove(bestChampCard);
            chosenCards.Add(bestChampCard);
            manaLimit -= bestChampCard.GetComponent<Card>().cardCost;
        }

        for (int i = 0; i < cardsSorted.Count; i++)
        {
            manaLimit -= cardsSorted[i].GetComponent<Card>().cardCost;
            if (manaLimit >= 0)
            {
                if (cardsSorted[i].GetComponent<Card>().cardType == CardTypes.CHAMPION)
                {
           
                        Debug.Log("added a champ");
                        chosenCards.Add(cardsSorted[i]);
                    
                }
                
                if (cardsSorted[i].GetComponent<Card>().cardType == CardTypes.SUPPORT)
                {
                    chosenCards.Add(cardsSorted[i]);
                    Debug.Log("added a sup");
                }
            }
        }

        for (int i = 0; i < chosenCards.Count; i++)
        {
            Debug.Log(chosenCards[i].name);
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
        Debug.Log(spotsLeft + "  spot lefttttttttttttttttttttttttttttttttttttt");
        return chosenCards;
    }

    public List<GameObject>
        CardsSortedByValue(List<GameObject> playableCards) // a away to sort the card by their value.
    {
        return playableCards.OrderByDescending(card => card.GetComponent<Card>().GetValue()).ToList();
    }
}