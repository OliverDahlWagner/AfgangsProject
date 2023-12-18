using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiCardPlacementHandler : MonoBehaviour
{
    public IEnumerator HandleCardPlacement()
    {
        var theChosenCards = ChosenCard();
        if (theChosenCards == null)
        {
            Debug.Log("no cards on hand or no mana for card");
            yield break;
        }

        foreach (var theChosenCard in theChosenCards)
        {
            if (theChosenCard.GetComponent<Card>().cardType == CardTypes.CHAMPION)
            {
                yield return new WaitForSeconds(1);
                yield return StartCoroutine(HandleChampionCard(theChosenCard));
            }

            if (theChosenCard.GetComponent<Card>().cardType == CardTypes.SUPPORT)
            {
                yield return new WaitForSeconds(1);
                yield return StartCoroutine(HandleSupportCard(theChosenCard));
            }
        }
    }

    ///////////////////////////////////////
    ///////////////////////////////////////    Choose Card/s
    ///////////////////////////////////////

    private List<GameObject> ChosenCard()
    {
        var cardsOnHand = GetComponent<DrawCards>().enemyHand; // this is a list of gameobjects

        if (cardsOnHand.Count == 0 ||
            GetComponent<AiBasicFunctions>().AvailableDropZonesIndexes().Count ==
            0) // no card to place or nowhere to place
        {
            return null; // not to play
        }

        var playingCards = SelectBestCardToPlay(GetComponent<DrawCards>().enemyHand); // no more random

        if (playingCards.Count == 0 || playingCards == null) // sometimes a null error arrives here. but it doesnt look to mess with anything
        {
            return null; // not to play
        }

        var chosenCards = new List<GameObject>();

        foreach (var playingCard in playingCards)
        {
            var theChosenCardCopy = playingCard;
            var thePosition = new Vector3(playingCard.transform.position.x, playingCard.transform.position.y, 0);
            var theChosenCard = Instantiate(theChosenCardCopy, thePosition, Quaternion.identity);

            GetComponent<DrawCards>().enemyHand.Remove(playingCard);
            Destroy(playingCard);

            theChosenCard.GetComponent<Card>().SetBackSideFalse();
            chosenCards.Add(theChosenCard);
        }


        return chosenCards;
    }


    private List<GameObject> SelectBestCardToPlay(List<GameObject> aiHand)
    {
        var currentMana = GetComponent<AiSystem>().battleSystem.GetComponent<BattleSystem>().enemyAva.currentMana;
        var playableCards = new List<GameObject>();
        GameObject bestCard = null;

        for (var index = 0; index < aiHand.Count; index++)
        {
            var card = aiHand[index];
            if (card.GetComponent<Card>().cardCost <= GetComponent<AiSystem>().battleSystem.GetComponent<BattleSystem>()
                    .enemyAva.currentMana)
            {
                playableCards.Add(card);
            }
        }

        var bestCardsList = GetComponent<SelectCards>().NewWayOfPicking(playableCards,currentMana);
        return bestCardsList;
    }

    ///////////////////////////////////////
    ///////////////////////////////////////    Champion Card
    ///////////////////////////////////////
    private IEnumerator HandleChampionCard(GameObject playingCard)
    {
        Debug.Log("played champion card " + playingCard.name);

        var dropZone = GetComponent<AiBasicFunctions>().GetRandomDropzone();

        dropZone.transform.position = new Vector3(dropZone.transform.position.x, dropZone.transform.position.y, 0);

        yield return StartCoroutine(StartPlacementOfCard(playingCard, dropZone));

        playingCard.transform.GetComponent<ChampionCard>().isOnBoard = true;
        playingCard.transform.GetComponent<ChampionCard>().hasBeenPlaced = true;

        GetComponent<BattleSystem>().enemyPlayedCards.Add(playingCard);

        GetComponent<AiBasicFunctions>().HandleManaCost(playingCard);

        yield return null;
    }

    private IEnumerator StartPlacementOfCard(GameObject playingCard, GameObject dropzone)
    {
        yield return StartCoroutine(ShowPlacingCard(playingCard));
        yield return StartCoroutine(MoveCardToDropZone(playingCard, dropzone, 1));
        yield return StartCoroutine(SetDropzoneAsParent(playingCard, dropzone));
    }

    private IEnumerator ShowPlacingCard(GameObject playingCard)
    {
        playingCard.transform.position = new Vector3(-190, 410, 0);
        playingCard.transform.SetParent(GetComponent<AiSystem>().enemyDropZone.transform, false);
        yield return null;
    }

    private IEnumerator MoveCardToDropZone(GameObject playingCard, GameObject endPoint, float duration)
    {
        Vector3 startPoint = playingCard.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // Calculate the current position based on the starting and ending points, based on the time passed
            playingCard.transform.position =
                Vector3.Lerp(startPoint, endPoint.transform.position, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SetDropzoneAsParent(GameObject playingCard, GameObject dropzone)
    {
        playingCard.transform.SetParent(dropzone.transform, false);
        playingCard.transform.position =
            new Vector3(playingCard.transform.position.x, playingCard.transform.position.y, 0);
        yield return null;
    }

    ///////////////////////////////////////
    ///////////////////////////////////////    Support Card
    ///////////////////////////////////////


    private IEnumerator HandleSupportCard(GameObject playingCard)
    {
        Debug.Log("played Support card " + playingCard.name);

        GetComponent<AiBasicFunctions>().HandleManaCost(playingCard);

        if (playingCard.GetComponent<SupportCard>().supCardType == SupCardTypes.INSTANT)
        {
            var dropzone = GetComponent<AiBasicFunctions>().GetRandomDropzone();
            dropzone.transform.position = new Vector3(dropzone.transform.position.x, dropzone.transform.position.y, 0);
            yield return
                StartCoroutine(StartPlacementOfCard(playingCard, dropzone)); // can use the same as champcard here
            yield return StartCoroutine(HandleInstantSupportCard(playingCard));
            yield break;
        }

        if (playingCard.GetComponent<SupportCard>().supCardType == SupCardTypes.LASTING)
        {
            var dropzone = GetComponent<AiBasicFunctions>().GetRandomDropzone();
            dropzone.transform.position = new Vector3(dropzone.transform.position.x, dropzone.transform.position.y, 0);
            StartCoroutine(StartPlacementOfCard(playingCard, dropzone)); // can use the same as champcard here
            StartCoroutine(HandleLastingSupportCard(playingCard));
            yield break;
        }

        if (playingCard.GetComponent<SupportCard>().supCardType == SupCardTypes.SPECIFIC)
        {
            var champCard = GetComponent<AiBasicFunctions>().GetRandomAIPlayedChampionCard();
            StartCoroutine(HandleSpecificSupportCard(playingCard, champCard));
            yield break;
        }
    }

    private IEnumerator HandleInstantSupportCard(GameObject supportCard)
    {
        if (GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count != 0)
        {
            supportCard.GetComponent<SupportCard>()
                .SupportFunction(null,
                    GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards());
        }

        Destroy(supportCard);
        yield return null;
    }

    private IEnumerator HandleLastingSupportCard(GameObject supportCard)
    {
        if (GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards().Count != 0)
        {
            supportCard.GetComponent<SupportCard>()
                .SupportFunction(null,
                    GetComponent<AiBasicFunctions>().GetAIPlayedChampionCards());
        }

        yield return null;
    }

    private IEnumerator HandleSpecificSupportCard(GameObject supportCard, GameObject champCard)
    {
        yield return StartCoroutine(SpecificSupportMovement(supportCard, champCard));
        yield return StartCoroutine(SpecificSupportAction(supportCard, champCard));
        yield return null;
    }

    private IEnumerator SpecificSupportMovement(GameObject supportCard, GameObject champCard)
    {
        yield return StartCoroutine(ShowPlacingCard(supportCard));
        yield return StartCoroutine(SpecificToChamp(supportCard, champCard, 1));
        yield return null;
    }

    private IEnumerator SpecificSupportAction(GameObject supportCard, GameObject champCard)
    {
        if (champCard == null)
        {
            Destroy(supportCard);
            yield break;
        }

        supportCard.GetComponent<SupportCard>()
            .SupportFunction(champCard, null);

        Destroy(supportCard);
        yield return null;
    }

    private IEnumerator SpecificToChamp(GameObject supportCard, GameObject champCard, float time)
    {
        Vector3 startPoint = supportCard.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            // Calculate the current position based on the starting and ending points, based on the time passed
            supportCard.transform.position =
                Vector3.Lerp(startPoint, champCard.transform.position, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}