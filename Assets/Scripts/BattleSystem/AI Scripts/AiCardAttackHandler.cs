using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AiCardAttackHandler : MonoBehaviour
{
    public IEnumerator ChooseCardForAttack() // well it di
    {
        // nothing to attack with
        if (GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count == 0)
        {
            yield break;
        }

        // this is the wrong approach. down under here. add it to the for loop.
        // make it so that inb the for loop it doesnt card from where i is and assign proper attack and target.
        // Remenber to set HasAttacked to keep track, that will help
        // how IDK yet but this is better then before

        // ask about a rule change


        // Figure out which AICardZones have cards in them and attack for each card out on the board. 
        for (int i = 0; i < GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count; i++)
        {
            if (GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>()
                    .hasBeenPlaced == false)
            {
                var cardList = GetAttackAndTargetCard();

                yield return new WaitForSeconds(1);

                yield return
                    StartCoroutine(Attack(cardList[0], cardList[1])); // ------- then make take it list item like now
            }
        }

        yield return new WaitForSeconds(1.5f); // just for a better feel
    }

    private GameObject threatCardObject = null;

    private List<GameObject> GetAttackAndTargetCard()
    {
        var aiCards = GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards();
        var playerCards = GetPlayerChampionCards(GetComponent<BattleSystem>().playerPlayedCards);
        Debug.Log(playerCards.Count + "---------------------");


        var cardAndTargetList = new List<GameObject>();

        // no cards, attack the player avatar   ---# this works
        if (playerCards.Count == 0)
        {
            // picks first that have not attacked or been placed that round
            var theCard = aiCards.FirstOrDefault(card =>
                card.GetComponent<ChampionCard>().hasAttacked == false &&
                card.GetComponent<ChampionCard>().hasBeenPlaced == false);
            var playerAva = GetComponent<BattleSystem>().playerAvatar;

            cardAndTargetList.Add(theCard);
            cardAndTargetList.Add(playerAva);

            return cardAndTargetList;
        }
        
        // the ai will either win    or is ahead of the player
        if (AttackPlayerForWin(aiCards) || BalanceInFavorOfAI())
        {
            // picks first that have not attacked or been placed that round
            var theCard = aiCards.FirstOrDefault(card =>
                card.GetComponent<ChampionCard>().hasAttacked == false &&
                card.GetComponent<ChampionCard>().hasBeenPlaced == false);
            var playerAva = GetComponent<BattleSystem>().playerAvatar;

            cardAndTargetList.Add(theCard);
            cardAndTargetList.Add(playerAva);

            return cardAndTargetList;
        }

        // no threat card, assign threat card
        if (threatCardObject == null)
        {
            threatCardObject = AssignThreatCard(playerCards);
        }

        // Attack the threat coming from the player
        var theChosenCard = aiCards.FirstOrDefault(card => card.GetComponent<ChampionCard>().hasAttacked == false);

        cardAndTargetList.Add(theChosenCard);
        cardAndTargetList.Add(threatCardObject);

        return cardAndTargetList;
    }

    private bool AttackPlayerForWin(List<GameObject> aiCards) // this condition works
    {
        var totalDamage = 0;

        for (int i = 0; i < aiCards.Count; i++)
        {
            totalDamage += aiCards[i].GetComponent<ChampionCard>().cardPower;
        }

        if (totalDamage >= GetComponent<BattleSystem>().playerAva.currentHP)
        {
            return true;
        }

        return false;
    }

    private bool BalanceInFavorOfAI()
    {
        var aiTotalHealth = GetComponent<AiBasicFunctions>().GetAttackReadyAiCardsTotalHealth();
        var aiTotalPower = GetComponent<AiBasicFunctions>().GetAttackReadyAiCardsTotalPower();
        var playerTotalHealth = GetTotalPlayerHealth();
        var playerTotalPower = GetTotalPlayerPower();

        int powerBalanceInt = 0;

        if (GetComponent<BattleSystem>().playerAva.currentHP < GetComponent<BattleSystem>().enemyAva.currentHP)
        {
            powerBalanceInt++;
        }
        
        if (GetComponent<BattleSystem>().playerAva.currentMana < GetComponent<BattleSystem>().enemyAva.currentMana)
        {
            powerBalanceInt++;
        }

        if (aiTotalHealth > playerTotalPower)
        {
            powerBalanceInt++;
        }

        if (aiTotalPower > playerTotalHealth)
        {
            powerBalanceInt++;
        }


        return powerBalanceInt > 1; // if 2 conditions is meet it will attack the player avatar (things can be added, modified and removed)
    }

    private List<GameObject> GetPlayerChampionCards(List<GameObject> playerPlayedCards)
    {
        var cardList = new List<GameObject>();

        for (int i = 0; i < playerPlayedCards.Count; i++)
        {
            if (playerPlayedCards[i].GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                cardList.Add(playerPlayedCards[i]);
            }
        }

        return cardList;
    }

    private int GetTotalPlayerPower()
    {
        var totalPower = 0;
        var champList = GetPlayerChampionCards(GetComponent<BattleSystem>().playerPlayedCards);
        for (int i = 0; i < champList.Count; i++)
        {
            totalPower += champList[i].GetComponent<ChampionCard>().cardPower;
        }

        return totalPower;
    }
    
    private int GetTotalPlayerHealth()
    {
        var totalHealth = 0;
        var champList = GetPlayerChampionCards(GetComponent<BattleSystem>().playerPlayedCards);
        for (int i = 0; i < champList.Count; i++)
        {
            totalHealth += champList[i].GetComponent<ChampionCard>().cardHealth;
        }

        return totalHealth;
    }

    private GameObject AssignThreatCard(List<GameObject> playerPlayedCards)
    {
        GameObject threatCard = null;

        for (int i = 0; i < playerPlayedCards.Count; i++)
        {
            if (threatCard == null)
            {
                threatCard = playerPlayedCards[i];
            }

            if (threatCard.GetComponent<ChampionCard>().cardPower <
                playerPlayedCards[i].GetComponent<ChampionCard>().cardPower)
            {
                threatCard = playerPlayedCards[i];
            }
        }

        return threatCard;
    }


    /*private GameObject threatCard = new GameObject(); */
    private IEnumerator
        Attack(GameObject attackerCard, GameObject targetGameObject) // --------------- make it a so it has a target
    {
        if (GetComponent<BattleSystem>().playerAvatar == targetGameObject) // attack the player
        {
            yield return StartCoroutine(AttackAvatarFunction(attackerCard));
            yield break;
        }

        // attack player cards
        StartCoroutine(AttackPlayerCardFunction(attackerCard, targetGameObject));
    }

    private IEnumerator AttackAvatarFunction(GameObject aiCard)
    {
        var startPosition = aiCard.transform.position;
        yield return StartCoroutine(MoveCardToAvatar(aiCard));
        yield return StartCoroutine(AttackAvatar(aiCard));
        yield return StartCoroutine(MoveCardBack(aiCard, startPosition));
    }

    private IEnumerator MoveCardToAvatar(GameObject aiCard)
    {
        Vector3 startPoint = aiCard.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            // Calculate the current position based on the starting and ending points, based on the time passed
            aiCard.transform.position = Vector3.Lerp(startPoint,
                GetComponent<BattleSystem>().playerAva.transform.position, (elapsedTime / 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator AttackPlayerCardFunction(GameObject attackCard, GameObject targetCard)
    {
        var random = new System.Random();

        var startPosition = attackCard.transform.position;
        yield return StartCoroutine(MoveCardToPlayerCard(attackCard, targetCard));
        yield return StartCoroutine(AttackPlayerCard(attackCard, targetCard));
        yield return StartCoroutine(MoveCardBack(attackCard, startPosition));
    }

    private IEnumerator AttackAvatar(GameObject aiCard)
    {
        aiCard.GetComponent<ChampionCard>().hasAttacked = true;
        GetComponent<BattleSystem>().playerAva.GetComponent<Avatar>()
            .TakeDamage(aiCard.GetComponent<ChampionCard>().cardPower);
        GetComponent<BattleSystem>().PlayerLost();
        yield return null;
    }

    private IEnumerator MoveCardBack(GameObject aiCard, Vector3 startPosition)
    {
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            // Calculate the current position based on the starting and ending points, based on the time passed
            aiCard.transform.position = Vector3.Lerp(aiCard.transform.position, startPosition, (elapsedTime / 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveCardToPlayerCard(GameObject attackerCard, GameObject targetCard)
    {
        Vector3 startPoint = attackerCard.transform.position;
        Vector3 endPoint = targetCard.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < 1)
        {
            // Calculate the current position based on the starting and ending points, based on the time passed
            attackerCard.transform.position = Vector3.Lerp(startPoint, endPoint, (elapsedTime / 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator AttackPlayerCard(GameObject attackCard, GameObject targetCard)
    {
        targetCard.GetComponent<ChampionCard>().TakeDamage(attackCard.GetComponent<ChampionCard>().cardPower);
        yield return null;
    }
    
}