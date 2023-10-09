using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        PlayerLost();
        PlayerWon();
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
        RestCardActions(playerDropZones);
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
    }

    private void RestCardActions(List<GameObject> dropZonesList)
    {
        foreach (var zone in dropZonesList.Where(zone => zone.transform.childCount != 0)) // where a card is present
        {
            zone.transform.GetChild(0).gameObject.GetComponent<ThisCard>().hasAttacked = false;
            zone.transform.GetChild(0).gameObject.GetComponent<ThisCard>().hasBeenPlaced = false;
        }
    }

    private void PlayerWon()
    {
        if (state == BattleState.WON)
        {
            Debug.Log("You Won!!! Congrats!");
        }
    }

    private void PlayerLost()
    {
        if (state == BattleState.LOST)
        {
            Debug.Log("You lost! U suck try again");
        }
    }
    
}