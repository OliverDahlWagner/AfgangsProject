using System.Collections;
using System.Collections.Generic;
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

        // Figure out which AICardZones have cards in them and attack for each card out on the board. 
        for (int i = 0; i < GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count; i++)
        {
            if (GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards()[i].GetComponent<ChampionCard>().hasBeenPlaced == false)
            {
                yield return new WaitForSeconds(1);
                yield return StartCoroutine(Attack(GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards()[i]));
            }
        }

        yield return new WaitForSeconds(1); // just for a better feel
    }
    
    /*private GameObject threatCard = new GameObject(); */
    // maybe set this as the target card. (so when we attacking starts this be set as the biggest threat)
    // if destroyed set to null, then new highest threat. now just need a way to determine threat  
    // All that this attack does is prioritise to attack cards first. However the cards attacked are random.
    private IEnumerator Attack(GameObject attackerCard)
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
            /*GetComponent<BattleSystem>().playerAva.GetComponent<Avatar>().TakeDamage(damageAmount);
            GetComponent<BattleSystem>().PlayerLost();*/
            yield return StartCoroutine(AttackAvatarFunction(attackerCard));
            yield break;
        }

        StartCoroutine(AttackPlayerCardFunction(attackerCard, attackableCards));
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
    
    private IEnumerator AttackPlayerCardFunction(GameObject attackCard, List<GameObject> attackableCards)
    {
        var random = new System.Random();
        var targetCard = attackableCards[random.Next(0, attackableCards.Count)]; // random

        var startPosition = attackCard.transform.position;
        yield return StartCoroutine(MoveCardToPlayerCard(attackCard, targetCard));
        yield return StartCoroutine(AttackPlayerCard(attackCard, targetCard));
        yield return StartCoroutine(MoveCardBack(attackCard, startPosition));
    }
    private IEnumerator AttackAvatar(GameObject aiCard)
    {
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
