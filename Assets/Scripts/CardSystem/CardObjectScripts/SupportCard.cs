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
                DoubleHealth(listOfPlayedCards);
                break;
            case 2:
                HealTwo(theCard);
                break;
            case 3:
                DoubleDamage(listOfPlayedCards);
                break;
            default:
                Debug.Log("Support function is not working");
                break;
        }
    }

    public void DoubleHealth(List<GameObject> listOfPlayedCards)
    {
        foreach (GameObject card in listOfPlayedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                card.GetComponent<ChampionCard>().cardHealth *= 15;
                card.GetComponent<ChampionCard>().AssignChampionValues();
            }
        }
    }
    
    public void HealTwo(GameObject champCard)
    {
        if (champCard.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            champCard.GetComponent<ChampionCard>().cardHealth += 2;
            champCard.GetComponent<ChampionCard>().AssignChampionValues();
        }
    }

    public void DoubleDamage(List<GameObject> listOfPlayedCards)
    {
        foreach (GameObject card in listOfPlayedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                card.GetComponent<ChampionCard>().cardPower *= 2;
                card.GetComponent<ChampionCard>().AssignChampionValues();
            }
        }

        roundCounter--;
        AssignRoundsLeftValues();
        LastingSupCardHelperFunction();
    }
    
    
    
    
    
    
}