using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DrawCards : MonoBehaviour
{
    private List<int> playerDeck;
    public List<GameObject> playerPlayingDeck;
    public List<GameObject> playerHand;

    private List<int> enemyDeck;
    public List<GameObject> enemyPlayingDeck;

    public List<GameObject> theCards; // if we do it like this all the cards will be in there. ON THE BUTTON
    // could maybe in future be made to a game-manager or a deck/hand manager (the button object in general)

    public GameObject PlayerArea;
    public GameObject EnemyArea;

    private String playerTag = "PlayerCard";
    private String enemyTag = "EnemyCard";


    private int playerHandsize = 4;

    void Awake()
    {
        // Depending on how we make the game, the way we get the deck will probably change
        playerDeck = new List<int>
        {
            1, 1, 1, 2, 2, 2 // Example playerDeck IDs of cards
        };

        enemyDeck = new List<int>
        {
            2, 2, 2, 1, 1, 1 // Example enemyDeck IDs of cards
        };


        playerPlayingDeck = GetMatchingCards(playerPlayingDeck);
        enemyPlayingDeck = GetMatchingCards(enemyPlayingDeck);

        Shuffle(playerPlayingDeck);
        Shuffle(enemyPlayingDeck);
        
        DrawCard(playerPlayingDeck, 4, PlayerArea, playerTag);
        DrawCard(enemyPlayingDeck, 4, EnemyArea, enemyTag);

        playerHandsize = PlayerArea.transform.childCount;
        
        foreach (Transform child in PlayerArea.transform)
        {
            playerHand.Add(child.gameObject);
        }
    }

    private void Update()
    {
        if (playerHandsize != PlayerArea.transform.childCount)
        {
            playerHand.Clear();
            foreach (Transform child in PlayerArea.transform)
            {
                playerHand.Add(child.gameObject);
            }

            playerHandsize = PlayerArea.transform.childCount;
        }

    }

    public void OnClick()  // in future maybe make a player and a enemy draw function. to avoid needing to have references 
    {
        DrawCard(playerPlayingDeck, 1, PlayerArea, playerTag);
        /*DrawCard(enemyPlayingDeck, 1, EnemyArea, enemyTag);*/
    }


    // Function to get the GameObjects that match the IDs in playingDeck   (the shuffle could be placed in here)
    private List<GameObject> GetMatchingCards(List<GameObject> givenDeck)
    {
        givenDeck = new List<GameObject>();

        foreach (int id in playerDeck)
        {
            GameObject card = theCards.Find(c => c.GetComponent<ThisCard>().cardId == id);
            if (card != null)
            {
                givenDeck.Add(card);
            }
        }

        return givenDeck;
    }

    // need to give the cards a shuffle after they have been placed in the playing decks. because the are placed in order by id
    private static void Shuffle(List<GameObject> aList)
    {
        var random = new System.Random();

        var n = aList.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (aList[k], aList[n]) = (aList[n], aList[k]);
        }
    }


    // this function can be used for the starting draw. And a single draw
    private void DrawCard(List<GameObject> theDeck, int cardAmount, GameObject playerArea, String cardTag)
    {
        if (theDeck.Count == 0) // so we dont take cards that does exits. and wont fuck up
        {
            Debug.Log("out of cards");
            return;
        }

        for (var i = 0; i < cardAmount; i++)
        {
            var theCard = theDeck[0];

            GameObject dealtCard = Instantiate(theDeck[0], Vector3.zero, Quaternion.identity);
            dealtCard.transform.SetParent(playerArea.transform, false);
            dealtCard.tag = cardTag;
            /*Debug.Log(dealtCard.GetComponent<ThisCard>().cardId + " --- the instantiated cards id");*/

            theDeck.Remove(
                theCard); // this removes the first card from the list. so if we use index zero we always get the next card
        }
    }
    
    
}


// these are ways the playing deck of the player/enemy

/*
foreach (var x in enemyPlayingDeck)
{
    Debug.Log(x + " ----- Before card from enemyPlayingDeck "); 
}
*/

/*
foreach (var x in enemyPlayingDeck)
{
    Debug.Log(x + " ----- AFTER card from enemyPlayingDeck");
}
*/