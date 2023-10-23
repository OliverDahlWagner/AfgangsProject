using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AssetBundlePatching;
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
    public List<GameObject> playerPlayedCards;

    public List<GameObject> enemyDropZones;

    public GameObject playerHand;
    public GameObject enemyHand;

    public Avatar playerAva;
    public Avatar enemyAva;

    public BattleState state;
    public bool isPaused;

    public Button drawCardButton;
    public Button endTurnButton;

    private int totalTurnActions = 3; // just an idea for now. DRAW, PLACE, ATTACK (any combination)

    void Start()
    {
        isPaused = false;
        state = BattleState.START;
        SetupBattle();
    }

    private void Update()
    {
        LockUnlockButtons(state); // this works. the color changes to a lighter grey. We can maybe make a more clear indicator later
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
        ResetCardActions(playerPlayedCards);
        IncreaseMana(playerAva);
        LastingSupCardUseFunction();

        Debug.Log("Player Turn");
    }

    private void EnemyTurn()
    {
        IncreaseMana(enemyAva);
        Debug.Log("Enemy Turn");
        GetComponent<AiSystem>().playCards();
        // now just make it do its thing, then end if conditions is meet


        EndEnemyTurn(); // might not be a need for endenemyturn()   just a if statement that checks win lost state if not then playerturn
    }

    private void EndEnemyTurn()
    {
        Debug.Log("Ended EnemyTurn");
        if (state == BattleState.ENEMYTURN) // wont start a new turn if the player avatar is killed
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
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

    private void ResetCardActions(List<GameObject> playedCards)
    {
        foreach (GameObject card in playedCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                card.GetComponent<ChampionCard>().hasAttacked = false;
                card.GetComponent<ChampionCard>().hasBeenPlaced = false;
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
        if (enemyAva.currentHP >= 1)
        {
            Debug.Log("enemy not dead yet");
            return;
        }
        // Lock interaction with board and cards.
        // when the player lock function is made for enemy turn just use it here;

        state = BattleState.WON;
        GetComponent<WonLostScreenScript>().GameConclusion();
        Debug.Log("Player Won");
    }

    public void PlayerLost() // this function should be fired at the end of every enemy attacks at the player avatar
    {
        if (playerAva.currentHP >= 1)
        {
            Debug.Log("player not dead yet");
            return;
        }
        // Lock interaction with board and cards.
        // when the player lock function is made for enemy turn just use it here;

        state = BattleState.LOST;
        GetComponent<WonLostScreenScript>().GameConclusion();
        Debug.Log("Player Lost!! u suck");
    }

    private void LockUnlockButtons(BattleState battleState) // no need to add locking of cards. they cant move if it ain't player turn anyway
    {
        if (state == BattleState.PLAYERTURN && isPaused == false)
        {
            drawCardButton.interactable = true;
            endTurnButton.interactable = true;
        }
        else
        {
            drawCardButton.interactable = false;
            endTurnButton.interactable = false;
        }
    }

    public void UpdatePLayerHand(GameObject card)
    {
        playerPlayedCards.Remove(card);
        playerPlayedCards.RemoveAll(x => !x);
    }

    private void LastingSupCardUseFunction()
    {
        for (int i = 0; i < playerPlayedCards.Count; i++)   // For loops are goated. fuck for each loops. all my homies hate for each loops
        {
            var card = playerPlayedCards[i];
            if (card.GetComponent<Card>().cardType != CardTypes.CHAMPION && 
                card.GetComponent<Card>().cardType == CardTypes.SUPPORT && 
                card.GetComponent<SupportCard>().supCardType == SupCardTypes.LASTING)
            {
                card.GetComponent<SupportCard>().SupportFunction(
                    card.GetComponent<SupportCard>().supportEffect, null,
                    playerPlayedCards);
            }
        }
    }

}