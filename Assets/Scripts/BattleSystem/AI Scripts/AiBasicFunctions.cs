using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AiBasicFunctions : MonoBehaviour
{
    public void DrawOneCard()
    {
        // Just draws one card. Because that is what it does! and the ai is stupid

        if (GetComponent<AiSystem>().battleSystem.GetComponent<DrawCards>().enemyPlayingDeck.Count > 0)
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

    public void LastingSupCardUseFunction()
    {
        if (GetAIPlayedLastingSupportCards().Count == 0)
        {
            return;
        }

        for (var i = 0; i < GetAIPlayedLastingSupportCards().Count; i++)
        {
            GetAIPlayedLastingSupportCards()[i].GetComponent<SupportCard>()
                .SupportFunction(null, GetAIPlayedChampionCards());
        }
    }

    public void HandleManaCost(GameObject playingCard)
    {
        // Mana cost
        GetComponent<AiSystem>().battleSystem.GetComponent<BattleSystem>()
            .ManaCostHandlerEnemy(playingCard.transform.GetComponent<Card>().cardCost);
    }

    public void ResetChampCardActions()
    {
        // Figure out which AICardZones have cards in them and attack for each card out on the board. 
        for (int i = 0; i < GetAIPlayedChampionCards().Count; i++)
        {
            GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>().hasBeenPlaced = false;
            GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>().hasAttacked = false;
        }
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
    
    public List<GameObject> GetAIPlayedChampionCards()
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
    
    public List<int> AvailableDropZonesIndexes()
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
    
    public GameObject GetRandomDropzone()
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
    
    public GameObject GetRandomAIPlayedChampionCard() // this function is used pick the champion getting the support card. can be done better.
    {
        var aiChampionCards = GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards();

        if (aiChampionCards.Count <= 0) return null;

        var random = new System.Random();
        var index = random.Next(aiChampionCards.Count);

        return aiChampionCards[index]; // a random champ for specific
    }
}
