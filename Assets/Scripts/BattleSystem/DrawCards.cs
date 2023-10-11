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
    public List<GameObject> playerDropZones;
    private int playerHandCount;
    private int playerDropZonesChildren = 0;
    private int playerManaInPlay = 0;

    private List<int> enemyDeck;
    public List<GameObject> enemyPlayingDeck;
    public List<GameObject> enemyHand;
    public List<GameObject> enemyDropZones;

    public List<GameObject> theCards;

    public GameObject PlayerArea;
    public GameObject EnemyArea;

    private String playerTag = "PlayerCard";
    private String enemyTag = "EnemyCard";


    private int playerHandsize = 4;
    private int enemyHandsize = 4;

    void Awake()
    {
        // Depending on how we make the game, the way we get the deck will probably change
        playerDeck = new List<int>
        {
            1, 1, 1, 2, 2, 2, 3, 3 // Example playerDeck IDs of cards
        };

        enemyDeck = new List<int>
        {
            2, 2, 2, 1, 1, 1 // Example enemyDeck IDs of cards
        };


        playerPlayingDeck = GetMatchingCards(playerPlayingDeck, true);
        enemyPlayingDeck = GetMatchingCards(enemyPlayingDeck, false);

        Shuffle(playerPlayingDeck);
        Shuffle(enemyPlayingDeck);

        DrawCard(playerPlayingDeck, 4, PlayerArea, playerTag);
        DrawCard(enemyPlayingDeck, 4, EnemyArea, enemyTag);

        playerHandsize = PlayerArea.transform.childCount;
        enemyHandsize = EnemyArea.transform.childCount;

        foreach (Transform child in PlayerArea.transform)
        {
            playerHand.Add(child.gameObject);
        }

        foreach (Transform child in EnemyArea.transform)
        {
            enemyHand.Add(child.gameObject);
        }
    }

    private void Update()
    {
        UpdatePlayerHand();
    }


    public void OnClick(bool playerDrawing) // The playerDrawing is true when button is clicked.
    {
        if (playerDrawing)
        {
            DrawCard(playerPlayingDeck, 1, PlayerArea, playerTag);
        }
        else
        {
            DrawCard(enemyPlayingDeck, 1, EnemyArea, enemyTag);
        }
    }


    // Function to get the GameObjects that match the IDs in playingDeck   (the shuffle could be placed in here)
    private List<GameObject> GetMatchingCards(List<GameObject> givenDeck, bool isPlayer)
    {
        givenDeck = new List<GameObject>();

        if (isPlayer)
        {
            foreach (int id in playerDeck)
            {
                GameObject card = theCards.Find(c => c.GetComponent<Card>().cardId == id);
                if (card != null)
                {
                    givenDeck.Add(card);
                }
            }

            return givenDeck;
        }

        foreach (int id in enemyDeck)
        {
            GameObject card = theCards.Find(c => c.GetComponent<Card>().cardId == id);
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

    private void UpdatePlayerHand()
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
}