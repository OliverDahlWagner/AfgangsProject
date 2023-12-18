using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardValueFunctions : MonoBehaviour
{
    public void ValueTwoCards(GameObject card1, GameObject card2, List<GameObject> bestCardList,
        ref int bestCardComboValue, // ref int is a reference to the variable. It makes sense if you know some Rust (But not really)
        int currentMana)
    {
        if (card1 == card2 || card1 == null || card2 == null)
        {
            return; // same card or null
        }

        var combinedMana = card1.GetComponent<Card>().cardCost + card2.GetComponent<Card>().cardCost;
        if (combinedMana > currentMana)
        {
            return; // too big mana cost
        }

        if (card1.GetComponent<Card>().cardType == CardTypes.CHAMPION &&
            card2.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            var cardValue1 = GetChampCardValue(card1);
            var cardValue2 = GetChampCardValue(card2);

            var comboValue = cardValue1 + cardValue2;

            if (comboValue > bestCardComboValue)
            {
                bestCardComboValue = comboValue; // this is not setting

                if (bestCardList.Count != 0)
                {
                    bestCardList.Clear();
                }

                bestCardList.Add(card1);
                bestCardList.Add(card2);
            }
        }
        
        // this is fine just make sure too place a champion card first
        if (card1.GetComponent<Card>().cardType == CardTypes.CHAMPION &&
            card2.GetComponent<Card>().cardType == CardTypes.SUPPORT)
        {
            var cardValue1 = GetChampCardValue(card1); // the champion card
            var cardValue2 = GetSupportCardValue(card2); // the support card

            var comboValue = cardValue1 + cardValue2;

            if (comboValue > bestCardComboValue)
            {
                bestCardComboValue = comboValue; // this is not setting

                if (bestCardList.Count != 0)
                {
                    bestCardList.Clear();
                }

                bestCardList.Add(card1); // the champion card
                bestCardList.Add(card2); // the support card
            }
            Debug.Log(comboValue + " Card 1: " + card1.name + " Card 2: " + card2.name);
        }
        
        // this is fine just make sure too place a champion card first
        if (card1.GetComponent<Card>().cardType == CardTypes.SUPPORT &&
            card2.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            var cardValue1 = GetSupportCardValue(card1); // the support card
            var cardValue2 = GetChampCardValue(card2); // the champion card

            var comboValue = cardValue1 + cardValue2;

            if (comboValue > bestCardComboValue)
            {
                bestCardComboValue = comboValue; // this is not setting

                if (bestCardList.Count != 0)
                {
                    bestCardList.Clear();
                }

                bestCardList.Add(card2); // the champion card
                bestCardList.Add(card1); // the support card
            }
            Debug.Log(comboValue + " Card 1: " + card1.name + " Card 2: " + card2.name);
        }
        
        // here there will need to be at least one champion card
        if (card1.GetComponent<Card>().cardType == CardTypes.SUPPORT &&
            card2.GetComponent<Card>().cardType == CardTypes.SUPPORT)
        {
            if (GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count < 1)
            {
                return;
            }
            
            var cardValue1 = GetSupportCardValue(card1);
            var cardValue2 = GetSupportCardValue(card2);

            var comboValue = cardValue1 + cardValue2;

            if (comboValue > bestCardComboValue)
            {
                bestCardComboValue = comboValue; // this is not setting

                if (bestCardList.Count != 0)
                {
                    bestCardList.Clear();
                }

                bestCardList.Add(card1);
                bestCardList.Add(card2);
            }
            Debug.Log(comboValue + " Card 1: " + card1.name + " Card 2: " + card2.name);
        }
    }
    
    public int GetChampCardValue(GameObject card)
    {
        const int cardCostWeight = -1;
        const int cardHealthWeight = 1;
        const int cardPowerWeight = 2;

        return card.GetComponent<Card>().cardCost * cardCostWeight +
               card.GetComponent<ChampionCard>().cardHealth * cardHealthWeight +
               card.GetComponent<ChampionCard>().cardPower * cardPowerWeight;
    }
    
    public int GetSupportCardValue(GameObject card)
    {
        const int cardCostWeight = 1;
        const int cardHealthWeight = 2;
        const int cardPowerWeight = 1;

        if (card.GetComponent<SupportCard>().supCardType == SupCardTypes.INSTANT)
        {
            var amountOfCards = GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count;
            var timesFactor = 0;

            switch(amountOfCards) 
            {
                case 0:
                    timesFactor = 0;
                    break;
                case 1:
                    timesFactor = 1;
                    break;
                case 2:
                    timesFactor = 2;
                    break;
                case 3:
                    timesFactor = 3;
                    break;
                default:
                    timesFactor = -1;
                    break;
            }
            
            // can be changed if needed
            return card.GetComponent<Card>().cardCost * cardCostWeight +
                   card.GetComponent<SupportCard>().healthModifierValue * cardHealthWeight +
                   card.GetComponent<SupportCard>().attackModifierValue * cardPowerWeight * timesFactor;
        }
        
        if (card.GetComponent<SupportCard>().supCardType == SupCardTypes.LASTING)
        {
            var amountOfCards = GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count;
            int timesFactor;
            int roundFactor;

            switch(amountOfCards) 
            {
                case 0:
                    timesFactor = 0;
                    roundFactor = 0;
                    break;
                case 1:
                    timesFactor = 1;
                    roundFactor = 1;
                    break;
                case 2:
                    timesFactor = 2;
                    roundFactor = 2;
                    break;
                case 3:
                    timesFactor = 3;
                    roundFactor = 3;
                    break;
                default:
                    timesFactor = -1;
                    roundFactor = -1;
                    break;
            }

            // can be changed if needed
            return card.GetComponent<Card>().cardCost * cardCostWeight +
                   card.GetComponent<SupportCard>().healthModifierValue * cardHealthWeight +
                   card.GetComponent<SupportCard>().attackModifierValue * cardPowerWeight * timesFactor * roundFactor + 5;
        }
        
        // can do something in targeting that picks the strongest card
        if (card.GetComponent<SupportCard>().supCardType == SupCardTypes.SPECIFIC)
        {
            // can be changed if needed
            return card.GetComponent<Card>().cardCost * cardCostWeight +
                   card.GetComponent<SupportCard>().healthModifierValue * cardHealthWeight +
                   card.GetComponent<SupportCard>().attackModifierValue * cardPowerWeight * 3; // times three to be generous
        }

        return -1; // This case sudden happen
    }
}