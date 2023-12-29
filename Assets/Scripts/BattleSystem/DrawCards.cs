using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
    private int maxHandSize = 7;

    private List<int> enemyDeck;
    public List<GameObject> enemyPlayingDeck;
    public List<GameObject> enemyHand;
    public List<GameObject> enemyDropZones;

    public List<GameObject> theCards;

    public GameObject PlayerArea;
    public GameObject EnemyArea;

    private String playerTag = "PlayerCard";
    private String enemyTag = "EnemyCard";

    public int playerHandsize = 4;
    private int enemyHandsize = 4;

    public Text playerCardCount;
    public Text enemyCardCount;

    void Awake()
    {
        // Depending on how we make the game, the way we get the deck will probably change
        playerDeck = new List<int>
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 1, 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105,
            106, 107, 108, 109, 110
        };

        enemyDeck = new List<int>
        {
            1, 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 1, 2, 3, 4, 5, 6, 7, 8, 9, 101, 102, 103, 104, 105,
            106, 107, 108, 109, 110
        };



        playerPlayingDeck = GetMatchingCards(playerDeck);
        enemyPlayingDeck = GetMatchingCards(enemyDeck);

        Shuffle(playerPlayingDeck);
        Shuffle(enemyPlayingDeck);

        DrawCard(playerPlayingDeck, 4, PlayerArea, playerTag);
        DrawCard(enemyPlayingDeck, 4, EnemyArea, enemyTag);
        
        enemyHandsize = EnemyArea.transform.childCount;

        foreach (Transform child in EnemyArea.transform)
        {
            enemyHand.Add(child.gameObject);
        }
    }

    private void Update()
    {
        SetDeckCounts();
        UpdatePlayerHand();
    }

    public void OnClick(bool playerDrawing) // The playerDrawing is true when button is clicked.
    {
        if (playerDrawing)
        {
            if (GetComponent<BattleSystem>().playerCanDraw)
            {
                if (playerPlayingDeck.Count > 0 && playerHandsize < maxHandSize)
                {
                    DrawCard(playerPlayingDeck, 1, PlayerArea, playerTag);
                    GetComponent<BattleSystem>().playerCanDraw = false;
                }
            }
        }
        else
        {
            if (enemyPlayingDeck.Count > 0 && enemyHandsize < maxHandSize)
            {
                DrawCard(enemyPlayingDeck, 1, EnemyArea, enemyTag);
            }
        }
    }


    // Function to get the GameObjects that match the IDs in playingDeck   (the shuffle could be placed in here)
    private List<GameObject> GetMatchingCards(List<int> cardIdList)
    {
        var list = new List<GameObject>();
        
        foreach (int id in cardIdList)
        {
            GameObject card = theCards.Find(c => c.GetComponent<Card>().cardId == id);
            if (card != null)
            {
                list.Add(card);
            }
        }

        return list;
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
        if (theDeck.Count == 0)
        {
            return;
        }

        for (var i = 0; i < cardAmount; i++)
        {
            var theCard = theDeck[0];

            GameObject dealtCard = Instantiate(theCard, Vector3.zero, Quaternion.identity);
            dealtCard.transform.SetParent(playerArea.transform, false);
            dealtCard.tag = cardTag;

            if (dealtCard.tag == "EnemyCard")
            {
                dealtCard.GetComponent<Card>().SetBackSideTrue();
            }

            if (dealtCard.tag == "PlayerCard")
            {
                playerHand.Add(dealtCard);
                playerHandsize = playerHand.Count;
            }

            theDeck.Remove(theCard); // this removes the first card from the list. so if we use index zero we always get the next card
        }
    }

    private void SetDeckCounts()
    {
        var playerDeckCount = playerPlayingDeck.Count;
        playerCardCount.text = playerDeckCount.ToString();
        var enemyDeckCount = enemyPlayingDeck.Count;
        enemyCardCount.text = enemyDeckCount.ToString();
    }

    private void UpdateEnemyHand()
    {
        if (enemyHandsize != EnemyArea.transform.childCount)
        {
            enemyHand.Clear();
            foreach (Transform child in EnemyArea.transform)
            {
                enemyHand.Add(child.gameObject);
            }

            enemyHandsize = EnemyArea.transform.childCount;
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