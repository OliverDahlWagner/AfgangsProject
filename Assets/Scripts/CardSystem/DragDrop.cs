using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject battleSystem;

    public GameObject canvas;
    private GameObject startParent;

    private bool isOverDropZone = false;

    private GameObject dropZone;

    private Vector3 startPosition;

    private GameObject specificCard;


    public void Awake() // calls when script/object is instantiated
    {
        battleSystem = GameObject.Find("Battle System");
        canvas = GameObject.Find("Main Canvas"); // will find the Object with the scene of the same name
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // now right clicks wont do shit
        {
            return;
        }

        if (CardNotMove())
        {
            return;
        }

        startParent = transform.parent.gameObject;
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) // now right clicks wont do shit
        {
            return;
        }

        if (CardNotMove())
        {
            return;
        }

        /*battleSystem.GetComponent<DrawCards>().playerHand.Remove(transform.gameObject);
        battleSystem.GetComponent<DrawCards>().playerHandsize = battleSystem.GetComponent<DrawCards>().playerHand.Count;*/

        Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(v3.x, v3.y, 0); // so the z value wont stays 0

        transform.SetParent(canvas.transform,
            true);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            return;
        }

        if (CardNotMove())
        {
            return;
        }

        if (transform.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            DropChampionCard();
        }

        if (transform.GetComponent<Card>().cardType == CardTypes.SUPPORT &&
            transform.GetComponent<SupportCard>().supCardType == SupCardTypes.INSTANT)
        {
            DropInstantSupCard();
        }

        if (transform.GetComponent<Card>().cardType == CardTypes.SUPPORT &&
            transform.GetComponent<SupportCard>().supCardType == SupCardTypes.SPECIFIC)
        {
            DropSpecificSupCard();
        }

        if (transform.GetComponent<Card>().cardType == CardTypes.SUPPORT &&
            transform.GetComponent<SupportCard>().supCardType == SupCardTypes.LASTING)
        {
            DropLastingSupCard();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameObject.CompareTag("PlayerCard") || !collision.gameObject.CompareTag("PlayerZone")) return;

        isOverDropZone = true;

        if (1 == collision.gameObject.transform
                .childCount) // so there wont be a indicator that it can be dropped if it contains card. Also a good check to have. Not really needed because of the check in endDrag
        {
            specificCard = collision.gameObject.transform.GetChild(0).GameObject();
            Debug.Log(specificCard);
            isOverDropZone = false;
            return;
        }

        dropZone = collision.gameObject;

        if (dropZone.CompareTag("PlayerZone"))
        {
            dropZone.GetComponent<Image>().color = new Color32(124, 193, 191, 255);
        }


        Debug.Log(dropZone.name);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerZone"))
        {
            isOverDropZone = false;
            if (dropZone != null) dropZone.GetComponent<Image>().color = new Color32(1, 1, 1, 0);
            dropZone = null;
            specificCard = null;
            Debug.Log(specificCard);
        }
    }

    private void DropChampionCard()
    {
        if (isOverDropZone
            && dropZone.transform.childCount < 1 && dropZone.CompareTag("PlayerZone") &&
            transform.GetComponent<Card>().cardType == CardTypes.CHAMPION)
        {
            if (transform.gameObject.GetComponent<ChampionCard>().isOnBoard)
            {
                transform.SetParent(dropZone.transform, false);
                dropZone.GetComponent<Image>().color = new Color32(1, 1, 1, 0);
                return;
            }

            if (battleSystem.GetComponent<BattleSystem>().playerAva.currentMana >=
                transform.GetComponent<Card>().cardCost)
            {
                transform.SetParent(dropZone.transform, false);
                transform.GetComponent<ChampionCard>().isOnBoard = true;
                transform.GetComponent<ChampionCard>().hasBeenPlaced = true;

                battleSystem.GetComponent<BattleSystem>().ManaCostHandler(transform.GetComponent<Card>().cardCost);
                battleSystem.GetComponent<BattleSystem>().playerPlayedCards.Add(transform.GameObject());

                dropZone.GetComponent<Image>().color = new Color32(1, 1, 1, 0);
            }
            else
            {
                ReturnToHand();
            }
        }
        else
        {
            ReturnToHand();
        }
    }

    private void DropInstantSupCard()
    {
        if (isOverDropZone
            && dropZone.transform.childCount < 1 && dropZone.CompareTag("PlayerZone") &&
            transform.GetComponent<Card>().cardType == CardTypes.SUPPORT)
        {
            if (battleSystem.GetComponent<BattleSystem>().playerAva.currentMana >=
                transform.GetComponent<Card>().cardCost)
            {
                transform.SetParent(dropZone.transform, false);
                battleSystem.GetComponent<BattleSystem>().ManaCostHandler(transform.GetComponent<Card>().cardCost);
                dropZone.GetComponent<Image>().color = new Color32(67, 89, 87, 255);

                transform.GetComponent<SupportCard>().SupportFunction(
                    null,
                    battleSystem.GetComponent<BattleSystem>()
                        .playerPlayedCards); // this will fire of the support effect

                Destroy(this.GameObject());
            }
            else
            {
                ReturnToHand();
            }
        }
        else
        {
            ReturnToHand();
        }
    }

    private void DropSpecificSupCard()
    {
        if (specificCard != null)
        {
            if (battleSystem.GetComponent<BattleSystem>().playerAva.currentMana >=
                transform.GetComponent<Card>().cardCost)
            {
                battleSystem.GetComponent<BattleSystem>().ManaCostHandler(transform.GetComponent<Card>().cardCost);
                transform.GetComponent<SupportCard>().SupportFunction(specificCard, null);
                Destroy(gameObject);
            }
            else
            {
                ReturnToHand();
            }
        }
        else
        {
            ReturnToHand();
        }
    }

    private void DropLastingSupCard()
    {
        if (isOverDropZone
            && dropZone.transform.childCount < 1 && dropZone.CompareTag("PlayerZone") &&
            transform.GetComponent<Card>().cardType == CardTypes.SUPPORT)
        {
            if (battleSystem.GetComponent<BattleSystem>().playerAva.currentMana >=
                transform.GetComponent<Card>().cardCost)
            {
                transform.SetParent(dropZone.transform, false);
                battleSystem.GetComponent<BattleSystem>().ManaCostHandler(transform.GetComponent<Card>().cardCost);
                battleSystem.GetComponent<BattleSystem>().playerPlayedCards.Add(transform.GameObject());
                dropZone.GetComponent<Image>().color = new Color32(67, 89, 87, 255);

                transform.GetComponent<SupportCard>().SupportFunction(null,
                    battleSystem.GetComponent<BattleSystem>()
                        .playerPlayedCards); // this will fire of the support effect

                dropZone.GetComponent<Image>().color = new Color32(1, 1, 1, 0);
            }
            else
            {
                ReturnToHand();
            }
        }
        else
        {
            ReturnToHand();
        }
    }

    private void ReturnToHand()
    {
        transform.position = startPosition;
        transform.SetParent(startParent.transform,
            false);
    }

    private bool CardNotMove() // if any conditions is true, card wont move
    {
        return HasAttackedThisRound() || HasBeenPlacedThisRound() || IsNotPlayerTurn() ||
               battleSystem.GetComponent<BattleSystem>().isPaused;
    }

    private bool HasAttackedThisRound() // condition for not moving
    {
        return transform.GetComponent<Card>().cardType == CardTypes.CHAMPION &&
               gameObject.GetComponent<ChampionCard>().hasAttacked;
    }

    private bool HasBeenPlacedThisRound() // condition for not moving
    {
        return transform.GetComponent<Card>().cardType == CardTypes.CHAMPION &&
               gameObject.GetComponent<ChampionCard>().hasBeenPlaced;
    }

    private bool IsNotPlayerTurn() // condition for not moving
    {
        return battleSystem.GetComponent<BattleSystem>().state != BattleState.PLAYERTURN;
    }
}