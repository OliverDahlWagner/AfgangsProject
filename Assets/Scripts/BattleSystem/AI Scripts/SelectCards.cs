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

            if (card.GetComponent<Card>().cardType == CardTypes.SUPPORT &&
                GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count != 0)
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
                GetComponent<CardValueFunctions>()
                    .ValueTwoCards(card1, card2, bestCardList, ref bestCardComboValue, currentMana);
            }
        }

        Debug.Log(bestCardList.Count + " double list");

        return bestCardList;
    }

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


        // Custom sorting algorithm to prioritize champion types
        chosenCards.Sort((card1, card2) =>
        {
            var type1 = card1.GetComponent<Card>().cardType;
            var type2 = card2.GetComponent<Card>().cardType;

            // Prioritize champion types
            if (type1 == CardTypes.CHAMPION && type2 != CardTypes.CHAMPION)
            {
                return -1; // card1 comes first
            }

            if (type1 != CardTypes.CHAMPION && type2 == CardTypes.CHAMPION)
            {
                return 1; // card2 comes first
            }
   
            return 0; // maintain the existing order
            
        });
        
        
        // to ensure that their are champs
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