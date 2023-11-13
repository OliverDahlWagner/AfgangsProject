using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum SupCardTypes
{
    INSTANT,
    LASTING,
    SPECIFIC
}
public class SupportCard : MonoBehaviour
{
    public SupCardTypes supCardType;
    public int supportEffect;
    public int roundCounter;

    public Image yellowContainer;
    public Text roundsLeftText;
    

    private GameObject battleSystem;
    private void Start()
    {
        battleSystem = GameObject.Find("Battle System");

        if (supCardType != SupCardTypes.LASTING)
        {
            yellowContainer.enabled = false;
            roundsLeftText.enabled = false;
        }
        AssignRoundsLeftValues();
    }
    
    public void AssignRoundsLeftValues()
    {
        roundsLeftText.text = roundCounter.ToString();
    }

    private void LastingSupCardHelperFunction()
    {
        if (roundCounter == 0)
        {
            if (battleSystem.GetComponent<BattleSystem>().state == BattleState.ENEMYTURN)
            {
                Destroy(gameObject);
            }
            if (battleSystem.GetComponent<BattleSystem>().state == BattleState.PLAYERTURN)
            {
                battleSystem.GetComponent<BattleSystem>().UpdatePLayerHand(this.GameObject());
                Destroy(gameObject);
            }
        }
    }

    private float GetAudioLevel(int amountOfCards)
    {
        var audioScale = 1.0f / amountOfCards;
        
        return audioScale;
    }
    
    
    ///////////////////////////////////
    /// ///////////////////////////////////
    /// ///////////////////////////////////   all support happens under here


    public void SupportFunction(int supportEffectInt, GameObject theCard, List<GameObject> listOfPlayedCards) 
                                                      // ok, now support cards work. but dont know how they should work.
                                                      // should they be on the playing field for a couple of rounds(depending on card).
                                                      // or should they just deliver the buff and be removed(then it can a stack buffs kinda game).
                                                      // Maybe a mix of both (some cards are instant and some have a lasting effect)
    {
        switch (supportEffectInt)
        {
            case 1:
                InstantDoubleHealth(listOfPlayedCards);
                break;
            case 2:
                SpecificHealTwo(theCard);
                break;
            case 3:
                InstantDoubleDamage(listOfPlayedCards);
                break;
            case 4:
                LastingPlusOnePower(listOfPlayedCards);
                break;
            case 5:
                LastingPlusTwoHealth(listOfPlayedCards);
                break;
            case 6:
                SpecificPowerPlusTwo(theCard);
                break;
            default:
                Debug.Log("Support function is not working");
                break;
        }
    }

    public void InstantDoubleHealth(List<GameObject> listOfPlayedCards)
    {
        foreach (GameObject card in listOfPlayedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                card.GetComponent<ChampionCard>().cardHealth *= 2;
                card.GetComponent<ChampionCard>().AssignChampionValues();
                card.GetComponent<ChampionCard>().PlayGetBuffEffect(GetAudioLevel(listOfPlayedCards.Count));
            }
        }
    }
    
    public void SpecificHealTwo(GameObject champCard)
    {
        if (champCard.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            champCard.GetComponent<ChampionCard>().cardHealth += 2;
            champCard.GetComponent<ChampionCard>().AssignChampionValues();
            champCard.GetComponent<ChampionCard>().PlayGetBuffEffect(GetAudioLevel(1));
        }
    }
    
    public void SpecificPowerPlusTwo(GameObject champCard)
    {
        if (champCard.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            champCard.GetComponent<ChampionCard>().cardPower += 2;
            champCard.GetComponent<ChampionCard>().AssignChampionValues();
            champCard.GetComponent<ChampionCard>().PlayGetBuffEffect(GetAudioLevel(1));
        }
    }

    public void LastingPlusOnePower(List<GameObject> listOfPlayedCards)
    {
        foreach (GameObject card in listOfPlayedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                card.GetComponent<ChampionCard>().cardPower += 1;
                card.GetComponent<ChampionCard>().AssignChampionValues();
                card.GetComponent<ChampionCard>().PlayGetBuffEffect(GetAudioLevel(listOfPlayedCards.Count));
            }
        }

        roundCounter--;
        AssignRoundsLeftValues();
        LastingSupCardHelperFunction();
    }
    
    public void InstantDoubleDamage(List<GameObject> listOfPlayedCards)
    {
        foreach (GameObject card in listOfPlayedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                card.GetComponent<ChampionCard>().cardPower *= 2;
                card.GetComponent<ChampionCard>().AssignChampionValues();
                card.GetComponent<ChampionCard>().AssignChampionValues();
                card.GetComponent<ChampionCard>().PlayGetBuffEffect(GetAudioLevel(listOfPlayedCards.Count));
            }
        }
    }
    
    public void LastingPlusTwoHealth(List<GameObject> listOfPlayedCards)
    {
        foreach (GameObject card in listOfPlayedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                card.GetComponent<ChampionCard>().cardHealth += 2;
                card.GetComponent<ChampionCard>().AssignChampionValues();
                card.GetComponent<ChampionCard>().AssignChampionValues();
                card.GetComponent<ChampionCard>().PlayGetBuffEffect(GetAudioLevel(listOfPlayedCards.Count));
            }
        }

        roundCounter--;
        AssignRoundsLeftValues();
        LastingSupCardHelperFunction();
    }
    
    
    
    
    
    
}