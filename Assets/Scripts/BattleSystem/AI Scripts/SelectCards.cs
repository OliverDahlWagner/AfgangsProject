using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SelectCards : MonoBehaviour
{
 
    public List<GameObject> ChooseOneCardToPlay(List<GameObject> playableCards, int currentMana)
    {
        var bestCardValue = -100;
        var bestCardList = new List<GameObject>();

        foreach (var card in playableCards)
        {
            if (card.GetComponent<Card>().cardCost > currentMana)
            {
                continue; // too big mana cost
            }


            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                var currentCardValue = GetComponent<CardValueFunctions>().GetChampCardValue(card);

                if (bestCardList.Count == 0)
                {
                    bestCardList.Add(card);
                }

                if (currentCardValue > bestCardValue)
                {
                    if (bestCardList.Count != 0)
                    {
                        bestCardList.Clear();
                    }

                    bestCardList.Add(card);
                    bestCardValue = currentCardValue;
                }
            }

            if (card.GetComponent<Card>().cardType == CardTypes.SUPPORT && GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count != 0)
            {
                var currentCardValue = GetComponent<CardValueFunctions>().GetSupportCardValue(card);
                if (currentCardValue > bestCardValue)
                {
                    if (bestCardList.Count != 0)
                    {
                        bestCardList.Clear();
                    }

                    bestCardList.Add(card);
                    bestCardValue = currentCardValue;
                }
            }
        }
        
        Debug.Log(bestCardList.Count + " single list");

        return bestCardList;
    }
    
    public List<GameObject> ChooseTwoCardsToPlay(List<GameObject> playableCards, int currentMana)
    {
        var bestCardComboValue = -100;
        var bestCardList = new List<GameObject>();

        foreach (var card1 in playableCards)
        {
            foreach (var card2 in playableCards)
            {

                GetComponent<CardValueFunctions>().ValueTwoCards(card1, card2, bestCardList, ref bestCardComboValue, currentMana);
                
            }
        }

        Debug.Log(bestCardList.Count + " double list");
        
        return bestCardList;
    }
    
    
}
