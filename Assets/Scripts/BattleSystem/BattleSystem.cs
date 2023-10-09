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

    private void PlayerTurn()
    {
        ResetCardActions(playerDropZones);
        IncreaseMana(playerAva);

        Debug.Log("Player Turn");
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

    private void ResetCardActions(List<GameObject> dropZoneList)
    {
        foreach (GameObject zone in dropZoneList)
        {
            if (zone.transform.childCount != 0)
            {
                zone.transform.GetChild(0).gameObject.GetComponent<ThisCard>().hasAttacked = false;
                zone.transform.GetChild(0).gameObject.GetComponent<ThisCard>().hasBeenPlaced = false;
            }
        }
    }

    // Will probably be put together with the first one at some point.
    public void ManaCostHandlerEnemy(int manaCost)
    {
        enemyAva.currentMana -= manaCost;
        enemyAva.SetCurrentMana(enemyAva.currentMana);
    }

    public void PlayerWon() // function is fired after every attack at the enemy avatar
    {
        if(enemyAva.currentHP >= 1)
        {
            Debug.Log("enemy not dead yet");
            return;
        }
        // Lock interaction with board and cards.
        // when the player lock function is made for enemy turn just use it here;

        state = BattleState.WON;
        Debug.Log("Player Won");
    }
    
    public void PlayerLost() // this function should be fired at the end of every enemy attacks at the player avatar
    {
        if(playerAva.currentHP >= 1)
        {
            Debug.Log("player not dead yet");
            return;
        }
        // Lock interaction with board and cards.
        // when the player lock function is made for enemy turn just use it here;

        state = BattleState.LOST;
        Debug.Log("Player Lost!! u suck");
    }
    
}