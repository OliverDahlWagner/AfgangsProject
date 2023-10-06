using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    START,
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerAvatar;
    public GameObject enemyAvatar;

    public Transform playerAvatarSpawn;
    public Transform enemyAvatarSpawn;

    public List<GameObject> playerDropZones;
    public List<GameObject> playerDropZonesChildren;

    public List<GameObject> enemyDropZones;

    public GameObject playerHand;
    public GameObject enemyHand;

    public Avatar playerAva;
    public Avatar enemyAva;

    public BattleState state;

    private int totalTurnActions = 3; // just an idea for now. DRAW, PLACE, ATTACK (any combination)

    void Start()
    {
        state = BattleState.START;
        SetupBattle();

        foreach (GameObject zone in playerDropZones)
        {
            if (zone.transform.childCount == 0)
            {
            }
        }
    }

    private void Update()
    {
        /*foreach (GameObject zone in playerDropZones)
        {
            if (zone.transform.childCount != 0)
            {
                zone.transform.GetChild(0).gameObject.GetComponent<ThisCard>().hasAttacked = true;
                zone.transform.GetChild(0).gameObject.GetComponent<ThisCard>().hasBeenPlaced = true;
                // need to be reset to false when a new round starts (being set to true is already being taken care of)
                // and not in this loop, its just here im writing it (make a function)
            }
        }*/
        
    }


    private void SetupBattle()
    {
        GameObject playerGO = Instantiate(playerAvatar, playerAvatarSpawn);
        playerAva = playerGO.GetComponent<Avatar>();
        playerAva.SetHUD();

        GameObject enemyGO = Instantiate(enemyAvatar, enemyAvatarSpawn);
        enemyAva = enemyGO.GetComponent<Avatar>();
        enemyAva.SetHUD();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void endPlayerTurn()
    {
        Debug.Log("end player turn clicked");
        state = BattleState.ENEMYTURN;
        EnemyTurn();
        // Debug.Log(state.ToString());
    }

    private void
        TurnHandler() // not a turnhandler (he does functions that call functions. not A function that calls functions)
    {
        // and do some coroutines
    }

    private void PlayerTurn()
    {
        IncreaseMana(playerAva);

        Debug.Log("Player Turn");
        
        // Debug.Log(playerHand.transform.childCount);
        // Debug.Log(playerHand.transform.GetChild(1).name); // no need for this shit. we have drawcards script on this gameobject
        //                                                     // go thorgh the playerplayingdeck given some variable that can used 
        //                                                     // when being dragged or attacking
        // Debug.Log(GetComponent<DrawCards>().playerHand.Count);
        // Debug.Log(GetComponent<DrawCards>().playerHand[1].name);
    }

    private void EnemyTurn()
    {
        IncreaseMana(enemyAva);
        Debug.Log("Enemy Turn");
        GetComponent<AiSystem>().playCards();
        // now just make it do its thing, then end if conditions is meet


        EndEnemyTurn();
    }

    private void EndEnemyTurn()
    {
        Debug.Log("Ended EnemyTurn");
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void IncreaseMana(Avatar avatar)
    {
        avatar.currentMana += 1;
        avatar.SetCurrentMana(avatar.currentMana);
    }

    public void ManaCostHandler(int manaCost)
    {

        playerAva.currentMana -= manaCost;
        playerAva.SetCurrentMana(playerAva.currentMana);

        // int totalMana = GetComponent<DrawCards>().playedManaCount;
        //
        // foreach (var dropZone in GetComponent<DrawCards>().playerDropZones)
        // {
        //     // Debug.Log(dropZone.transform.childCount + " indi dropzone count");
        //     if (dropZone.transform.childCount != 0)
        //     {
        //         // manaInPlay = + dropZone.transform.GetChild(0).GetComponent<ThisCard>().cardCost;
        //         totalMana += dropZone.transform.GetChild(0).GetComponent<ThisCard>().cardCost;
        //     }
        // }
        //
        //
        // // Debug.Log(GetComponent<DrawCards>().playedManaCount);
        // // find the diff
        //
        // // set new played mana
        //
        // if (GetComponent<DrawCards>().playedManaCount != totalMana)
        // {
        //     GetComponent<DrawCards>().playedManaCount = totalMana;
        // }



        // Debug.Log(manaInPlay);
        // GetComponent<DrawCards>().playedManaCount = manaInPlay;
        //
        // var titi = GetComponent<DrawCards>().playerDropZones.Count;
        // Debug.Log(titi + " whole dropzone count");
        //
        // foreach (var dropZone in GetComponent<DrawCards>().playerDropZones)
        // {
        //     Debug.Log(dropZone.transform.childCount + " indi dropzone count");
        // }
    }
}