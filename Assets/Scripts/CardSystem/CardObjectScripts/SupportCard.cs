using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCard : MonoBehaviour
{
    public int supportEffect;

    private GameObject battleSystem;
    private void Start()
    {
        battleSystem = GameObject.Find("Battle System");
    }

    public void SupportFunction(int supportEffectInt) // ok, now support cards work. but dont know how they should work.
                                                      // should they be on the playing field for a couple of rounds(depending on card).
                                                      // or should they just deliver the buff and be removed(then it can a stack buffs kinda game).
                                                      // Maybe a mix of both (some cards are instant and some have a lasting effect)
    {
        switch (supportEffectInt)
        {
            case 1:
                DoubleHealth(battleSystem.GetComponent<BattleSystem>().playerPlayedCards);
                break;
            case 2:
                HealTwo(battleSystem.GetComponent<BattleSystem>().playerPlayedCards);
                break;
            case 3:
                DoubleDamage(battleSystem.GetComponent<BattleSystem>().playerPlayedCards);
                break;
            default:
                Debug.Log("Support function is not working");
                break;
        }
    }

    private void DoubleHealth(List<GameObject> listOfPlayedCards)
    {
        Debug.Log("Double Health");
        foreach (GameObject card in listOfPlayedCards)
        {
            card.GetComponent<ChampionCard>().cardHealth *= 2;
            card.GetComponent<ChampionCard>().AssignChampionValues();
        }
    }
    
    private void HealTwo(List<GameObject> listOfPlayedCards)
    {
        Debug.Log("Heal 2");
        foreach (GameObject card in listOfPlayedCards)
        {
            card.GetComponent<ChampionCard>().cardHealth += 2;
            card.GetComponent<ChampionCard>().AssignChampionValues();
        }
    }
    
    private void DoubleDamage(List<GameObject> listOfPlayedCards)
    {
        Debug.Log("Double Damage");
        foreach (GameObject card in listOfPlayedCards)
        {
            card.GetComponent<ChampionCard>().cardPower *= 2;
            card.GetComponent<ChampionCard>().AssignChampionValues();
        }
    }
    
    
    
    
    
}