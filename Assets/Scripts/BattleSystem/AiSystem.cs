using System.Collections.Generic;
using UnityEngine;

public class AiSystem : MonoBehaviour
{

    public GameObject battleSystem;



    public void Awake() 
    {
        battleSystem = GameObject.Find("Battle System");
    }
    public void playCards()
    {
        var cardsOnHand = GetComponent<DrawCards>().enemyHand;
        var dropZones = GetComponent<DrawCards>().enemyDropZones;
        int indexDropZone = 0;


        // Just draws one card. Because that is what it does! and the ai is stupid
        GetComponent<DrawCards>().OnClick(false);




        // Pick random card on hand
        var random = new System.Random();
        int index = random.Next(cardsOnHand.Count);
        var playingCard = cardsOnHand[index];  // can cause a ArgumentOutOfRangeException: Index out of range

        // Find available dropzones
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < dropZones.Count; i++)
        {
            if (dropZones[i].transform.childCount < 1)
            {
                availableIndexes.Add(i);
            }
        }

        // CardCost
        var cardCost = playingCard.transform.GetComponent<ThisCard>().cardCost;

        // If there are avaiable dropzones & MANA, play card in a random dropzone.
        if (availableIndexes.Count > 0 && battleSystem.GetComponent<BattleSystem>().enemyAva.currentMana >= cardCost)
        {
            indexDropZone = random.Next(availableIndexes.Count);
            var dropZone = dropZones[availableIndexes[indexDropZone]];

            if (dropZone.transform.childCount < 1)
            {
                playingCard.transform.SetParent(dropZone.transform, false);

                playingCard.transform.GetComponent<ThisCard>().isOnBoard = true;
                playingCard.transform.GetComponent<ThisCard>().hasBeenPlaced = true;

                // Removes card from hand
                cardsOnHand.RemoveAt(index);
                GetComponent<DrawCards>().enemyHand = cardsOnHand;


                // Mana cost
                battleSystem.GetComponent<BattleSystem>().ManaCostHandlerEnemy(playingCard.transform.GetComponent<ThisCard>().cardCost);




            }
        }
    }

}
