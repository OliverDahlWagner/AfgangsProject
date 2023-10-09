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

        // We can swap the available dropzones on its head.



        chooseCardForAttack();
    }

    private void chooseCardForAttack()
    {

        // Figure out which AICardZones have cards in them and attack for each card out on the board. 
        var AIZones = GetComponent<DrawCards>().enemyDropZones;
        for (int i = 0; i < AIZones.Count; i++)
        {
            if (AIZones[i].transform.childCount > 0)
            {
                Debug.Log("HOW MANY TIMES DO WE ATTACK?");
                Debug.Log(AIZones[i]);

                Transform childTransform = AIZones[i].transform.GetChild(0);
                ThisCard thisCardComponent = childTransform.GetComponent<ThisCard>();
                Attack(childTransform.GetComponent<ThisCard>().cardPower);
            }
        }

    }

    // All that this attack does is prioritise to attack cards first. However the cards attacked are random.
    private void Attack(int damageAmount)
    {
        
        // Figuring out which playerzones has cards in them, if any.
        var playerZones = GetComponent<DrawCards>().playerDropZones;
        List<int> attackableCardIndexes = new List<int>();
        for (int i = 0; i < playerZones.Count; i++)
        {
            Debug.Log("NEVSAdasdsadsadas");
            Debug.Log(playerZones[i]);
            if (playerZones[i].transform.childCount > 0)
            {
                Debug.Log("NEVER IF?");
                Debug.Log(playerZones[i].transform.GetChild(0));
                attackableCardIndexes.Add(i);
            }
        }

        Debug.Log("ODD?");
        Debug.Log(playerZones);
        Debug.Log(playerZones.Count);
        Debug.Log(attackableCardIndexes.Count);
        Debug.Log(attackableCardIndexes);
        if(attackableCardIndexes.Count > 0)
        {
            // ATTACK A RANDOM AVAILABLE CARD
            var random = new System.Random();

            int targetIndex = attackableCardIndexes[random.Next(0, attackableCardIndexes.Count)];
            playerZones[targetIndex].transform.GetChild(0);


            Transform childTransform = playerZones[targetIndex].transform.GetChild(0);
            ThisCard thisCardComponent = childTransform.GetComponent<ThisCard>();

        
            // Call the TakeDamage method on the ThisCard component.
           thisCardComponent.TakeDamage(damageAmount);

            }
        else{
            // ATTACK THE PLAYER CUZ NO PLAYERCARDS ON BOARD'
            GetComponent<BattleSystem>().playerAva.GetComponent<Avatar>().TakeDamage(damageAmount);

        }






        //   int index = random.Next(playerZones.Count);



    }

}
