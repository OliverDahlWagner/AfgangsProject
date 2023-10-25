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

        var theChosenCard = ChosenCard();
        if (theChosenCard == null)
        {
            Debug.Log("no cards on hand or no mana for card");
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
        if (cardsOnHand.Count == 0)
        {
            return null;
        }

        // Pick random card on hand
        var random = new System.Random();
        int index = random.Next(0, cardsOnHand.Count);

        // this is the chosen card
        var playingCard = cardsOnHand[index];

        // CardCost
        var cardCost = playingCard.transform.GetComponent<Card>().cardCost;

        if (battleSystem.GetComponent<BattleSystem>().enemyAva.currentMana < cardCost)
        {
            return null;
        }

        var theChosenCardCopy = playingCard;
        var thePosition = new Vector3(playingCard.transform.position.x, playingCard.transform.position.y, 0);
        var theChosenCard = Instantiate(theChosenCardCopy, thePosition, Quaternion.identity);

        GetComponent<DrawCards>().enemyHand.RemoveAt(index);
        Destroy(playingCard);
        return theChosenCard;
    }

    private void ChooseCardForAttack()
    {
        // nothing to attack with
        if (GetAIPlayedChampionCards().Count == 0)
        {
            return;
        }

        // Figure out which AICardZones have cards in them and attack for each card out on the board. 
        for (int i = 0; i < GetAIPlayedChampionCards().Count; i++)
        {
            Attack(GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>().cardPower);
        }
    }

    // All that this attack does is prioritise to attack cards first. However the cards attacked are random.
    private void Attack(int damageAmount)
    {
        // Figuring out which playerzones has cards in them, if any.
        var playerCards = GetComponent<BattleSystem>().playerPlayedCards;
        List<GameObject> attackableCards = new List<GameObject>();
        foreach (var card in playerCards)
        {
            if (card.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                attackableCards.Add(card);
            }
        }

        if (attackableCards.Count > 0)
        {
            // ATTACK A RANDOM AVAILABLE CARD
            var random = new System.Random();

            GameObject targetCard = attackableCards[random.Next(0, attackableCards.Count)];

            ChampionCard thisCardComponent = targetCard.GetComponent<ChampionCard>();


            // Call the TakeDamage method on the ThisCard component.
            if (thisCardComponent.cardHealth > 0)
            {
                thisCardComponent.TakeDamage(damageAmount);
            }
            else
            {
                // Dont know why, but it seems to hit cards that are destroyed? Therefore i added this for now
                GetComponent<BattleSystem>().playerAva.GetComponent<Avatar>().TakeDamage(damageAmount);
                GetComponent<BattleSystem>().PlayerLost();
            }
        }
        else
        {
            // ATTACK THE PLAYER CUZ NO PLAYERCARDS ON BOARD'
            GetComponent<BattleSystem>().playerAva.GetComponent<Avatar>().TakeDamage(damageAmount);
            GetComponent<BattleSystem>().PlayerLost();
        }
        //   int index = random.Next(playerZones.Count);
    }

    private void DrawOneCard()
    {
        // Just draws one card. Because that is what it does! and the ai is stupid
        GetComponent<DrawCards>().OnClick(false);

        var newEnemyHand = new List<GameObject>();
        foreach (Transform child in GetComponent<DrawCards>().EnemyArea.transform)
        {
            newEnemyHand.Add(child.gameObject);
        }

        GetComponent<DrawCards>().enemyHand = newEnemyHand;
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
}