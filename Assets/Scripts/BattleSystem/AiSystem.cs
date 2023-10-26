using System.Collections.Generic;
using UnityEngine;

public class AiSystem : MonoBehaviour
{
    public GameObject battleSystem;


    public void Awake()
    {
        battleSystem = GameObject.Find("Battle System");
    }

    public void playCards() // this is the function enemyturn uses
    {
        LastingSupCardUseFunction();
        DrawOneCard();
        ResetChampCardActions();

        var theChosenCard = ChosenCard();
        if (theChosenCard == null)
        {
            Debug.Log("no cards on hand or no mana for card");
            ChooseCardForAttack();
            return;
        }

        if (theChosenCard.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            HandleChampionCard(theChosenCard);
        }

        if (theChosenCard.GetComponent<Card>().cardType == CardTypes.SUPPORT)
        {
            HandleSupportCard(theChosenCard);
        }

        ChooseCardForAttack();
    }

    private void HandleChampionCard(GameObject playingCard)
    {
        Debug.Log("played champion card " + playingCard.name);

        // If there are avaiable dropzones & MANA, play card in a random dropzone.
        var dropZone = GetRandomDropzone();

        if (dropZone.transform.childCount < 1)
        {
            playingCard.transform.SetParent(dropZone.transform, false);

            playingCard.transform.GetComponent<ChampionCard>().isOnBoard = true;
            playingCard.transform.GetComponent<ChampionCard>().hasBeenPlaced = true;

            GetComponent<BattleSystem>().enemyPlayedCards.Add(playingCard);

            HandleManaCost(playingCard);
        }
    }

    private void HandleSupportCard(GameObject playingCard)
    {
        Debug.Log("played Support card " + playingCard.name);

        if (playingCard.GetComponent<SupportCard>().supCardType == SupCardTypes.INSTANT)
        {
            HandleManaCost(playingCard);

            // will just need to place the the card    for animations
            var dropzone = GetRandomDropzone();
            playingCard.transform.SetParent(dropzone.transform, false);

            if (GetAIPlayedChampionCards().Count != 0)
            {
                playingCard.GetComponent<SupportCard>()
                    .SupportFunction(playingCard.GetComponent<SupportCard>().supportEffect, null,
                        GetAIPlayedChampionCards());
            }

            Destroy(playingCard);
        }

        if (playingCard.GetComponent<SupportCard>().supCardType == SupCardTypes.LASTING)
        {
            HandleManaCost(playingCard);

            // will just need to place the the card    for animations
            var dropzone = GetRandomDropzone();
            playingCard.transform.SetParent(dropzone.transform, false);

            if (GetAIPlayedChampionCards().Count != 0)
            {
                playingCard.GetComponent<SupportCard>()
                    .SupportFunction(playingCard.GetComponent<SupportCard>().supportEffect, null,
                        GetAIPlayedChampionCards());
            }
        }

        if (playingCard.GetComponent<SupportCard>().supCardType == SupCardTypes.SPECIFIC)
        {
            HandleManaCost(playingCard);

            // will just need to place the the card    for animations
            var champCard = GetRandomAIPlayedChampionCard(); // change to card
            // so for animation, just move to the champ card position then destroy

            if (champCard == null)
            {
                Destroy(playingCard);
                return;
            }

            playingCard.GetComponent<SupportCard>()
                .SupportFunction(playingCard.GetComponent<SupportCard>().supportEffect, champCard, null);

            Destroy(playingCard);
        }
    }

    private List<int> AvailableDropZonesIndexes()
    {
        var dropZones = GetComponent<DrawCards>().enemyDropZones;

        // Find available dropzones
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < dropZones.Count; i++)
        {
            if (dropZones[i].transform.childCount < 1)
            {
                availableIndexes.Add(i);
            }
        }

        return availableIndexes;
    }

    private GameObject GetRandomDropzone()
    {
        if (AvailableDropZonesIndexes().Count == 0)
        {
            return null;
        }

        var random = new System.Random();
        var dropZones = GetComponent<DrawCards>().enemyDropZones;
        int indexDropZone = 0;

        indexDropZone = random.Next(AvailableDropZonesIndexes().Count);
        var dropZone =
            dropZones[
                AvailableDropZonesIndexes()[
                    indexDropZone]]; // ok so it chooses a random index and only if empty will play a card

        return dropZone;
    }

    private List<GameObject> GetAIPlayedChampionCards()
    {
        var listOfChamps = new List<GameObject>(); // Initialize the list
        var dropZones = GetComponent<DrawCards>().enemyDropZones;

        foreach (var dropZone in dropZones)
        {
            if (dropZone.transform.childCount > 0 &&
                dropZone.transform.GetChild(0).gameObject.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                listOfChamps.Add(dropZone.transform.GetChild(0).gameObject);
            }
        }

        return listOfChamps;
    }

    private GameObject GetRandomAIPlayedChampionCard()
    {
        var aiChampionCards = GetAIPlayedChampionCards();

        if (aiChampionCards.Count <= 0) return null;

        var random = new System.Random();
        var index = random.Next(aiChampionCards.Count);

        return aiChampionCards[index]; // a random champ for specific
    }

    private List<GameObject> GetAIPlayedLastingSupportCards()
    {
        var listOfLastingSupportCards = new List<GameObject>(); // Initialize the list
        var dropZones = GetComponent<DrawCards>().enemyDropZones;

        foreach (var dropZone in dropZones)
        {
            if (dropZone.transform.childCount > 0 &&
                dropZone.transform.GetChild(0).gameObject.GetComponent<Card>().cardType == CardTypes.SUPPORT &&
                dropZone.transform.GetChild(0).gameObject.GetComponent<SupportCard>().supCardType ==
                SupCardTypes.LASTING)
            {
                listOfLastingSupportCards.Add(dropZone.transform.GetChild(0).gameObject);
            }
        }

        return listOfLastingSupportCards;
    }

    private GameObject ChosenCard()
    {
        var cardsOnHand = GetComponent<DrawCards>().enemyHand; // this is a list of gameobjects

        if (cardsOnHand.Count == 0 || AvailableDropZonesIndexes().Count == 0) // no card to place or nowhere to place
        {
            return null;
        }

        var playingCard = SelectBestCardToPlay(GetComponent<DrawCards>().enemyHand); // no more random

        if (playingCard == null)
        {
            return null;
        }

        var theChosenCardCopy = playingCard;
        var thePosition = new Vector3(playingCard.transform.position.x, playingCard.transform.position.y, 0);
        var theChosenCard = Instantiate(theChosenCardCopy, thePosition, Quaternion.identity);

        GetComponent<DrawCards>().enemyHand.Remove(playingCard);
        Destroy(playingCard);
        return theChosenCard;
    }

    private GameObject SelectBestCardToPlay(List<GameObject> aiHand)
    {
        if (GetAIPlayedChampionCards().Count > 1 &&
            battleSystem.GetComponent<BattleSystem>().playerPlayedCards.Count <= 1)
        {
            return null; // the ai is ahead and can charge mana
        }

        GameObject bestCard = null;
        var bestCardValue = -1;

        foreach (var card in aiHand)
        {
            if (card.GetComponent<Card>().cardCost <= battleSystem.GetComponent<BattleSystem>().enemyAva.currentMana)
            {
                if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
                {
                    var currentCardValue = GetCardValue(card);
                    if (currentCardValue > bestCardValue)
                    {
                        bestCard = card;
                        bestCardValue = currentCardValue;
                    }
                }

                if (card.GetComponent<Card>().cardType ==
                    CardTypes.SUPPORT) // currently no good way to rate supcard. maybe we just give them a value attribute
                    // that we see fit ... OR MAYBE the mana cost just reflect the RATING of the card (I like that)
                {
                    if (GetAIPlayedChampionCards().Count >
                        1) // just so it wont play a support without any thing to support (A LOT more can be added here)
                    {
                        var currentCardValue = card.GetComponent<Card>().cardCost;
                        if (currentCardValue > bestCardValue)
                        {
                            bestCard = card;
                            bestCardValue = currentCardValue;
                        }
                    }
                }
            }
        }

        return bestCard;
    }

    private int GetCardValue(GameObject card)
    {
        const int cardCostWeight = -1;
        const int cardHealthWeight = 1;
        const int cardPowerWeight = 2;

        return card.GetComponent<Card>().cardCost * cardCostWeight +
               card.GetComponent<ChampionCard>().cardHealth * cardHealthWeight +
               card.GetComponent<ChampionCard>().cardPower * cardPowerWeight;
    }

    private void ChooseCardForAttack() // well it di
    {
        // nothing to attack with
        if (GetAIPlayedChampionCards().Count == 0)
        {
            return;
        }

        // Figure out which AICardZones have cards in them and attack for each card out on the board. 
        for (int i = 0; i < GetAIPlayedChampionCards().Count; i++)
        {
            if (GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>().hasBeenPlaced == false)
            {
                Attack(GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>().cardPower);
            }
        }
    }


    private GameObject threatCard = new GameObject(); // maybe set this as the target card. (so when we attacking starts this be set as the biggest threat)
                                                        // if destroyed set to null, then new highest threat. now just need a way to determine threat  
    
    // All that this attack does is prioritise to attack cards first. However the cards attacked are random.
    private void Attack(int damageAmount)
    {
        // Figuring out which playerzones has cards in them, if any.
        var playerCards = GetComponent<BattleSystem>().playerPlayedCards;
        var attackableCards = new List<GameObject>();

        foreach (var card in playerCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                attackableCards.Add(card);
            }
        }

        if (attackableCards.Count == 0) // attack the player
        {
            GetComponent<BattleSystem>().playerAva.GetComponent<Avatar>().TakeDamage(damageAmount);
            GetComponent<BattleSystem>().PlayerLost();
            return;
        }

        // ATTACK A RANDOM AVAILABLE CARD (for now random really hard to fina a way to make it chose)
        var random = new System.Random();
        var targetCard = attackableCards[random.Next(0, attackableCards.Count)];

        targetCard.GetComponent<ChampionCard>().TakeDamage(damageAmount);
    }

    private void DrawOneCard()
    {
        // Just draws one card. Because that is what it does! and the ai is stupid

        if (battleSystem.GetComponent<DrawCards>().enemyPlayingDeck.Count > 0)
        {
            GetComponent<DrawCards>().OnClick(false);

            var newEnemyHand = new List<GameObject>();
            foreach (Transform child in GetComponent<DrawCards>().EnemyArea.transform)
            {
                newEnemyHand.Add(child.gameObject);
            }

            GetComponent<DrawCards>().enemyHand = newEnemyHand;
        }
    }

    private void LastingSupCardUseFunction()
    {
        if (GetAIPlayedLastingSupportCards().Count == 0)
        {
            return;
        }

        for (var i = 0; i < GetAIPlayedLastingSupportCards().Count; i++)
        {
            var supportEffect = GetAIPlayedLastingSupportCards()[i].GetComponent<SupportCard>().supportEffect;
            GetAIPlayedLastingSupportCards()[i].GetComponent<SupportCard>()
                .SupportFunction(supportEffect, null, GetAIPlayedChampionCards());
        }
    }

    private void HandleManaCost(GameObject playingCard)
    {
        // Mana cost
        battleSystem.GetComponent<BattleSystem>()
            .ManaCostHandlerEnemy(playingCard.transform.GetComponent<Card>().cardCost);
    }

    private void ResetChampCardActions()
    {
        // Figure out which AICardZones have cards in them and attack for each card out on the board. 
        for (int i = 0; i < GetAIPlayedChampionCards().Count; i++)
        {
            GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>().hasBeenPlaced = false;
            GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>().hasAttacked = false;
        }
    }
}