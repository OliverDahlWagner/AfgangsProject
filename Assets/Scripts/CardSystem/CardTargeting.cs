using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardTargeting : MonoBehaviour, IEndDragHandler
{
    public GameObject battleSystem;
    private GameObject collisionTaget;

    // Start is called before the first frame update
    void Start()
    {
        battleSystem = GameObject.Find("Battle System");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnEndDrag(PointerEventData eventData) // handle the targets    // get en error try some PLAYERTURN State
    {
        if (gameObject.GetComponent<SupportCard>() || collisionTaget == null)
        {
            return;
        }

        if (gameObject.CompareTag("PlayerCard") && collisionTaget.gameObject.CompareTag("EnemyCard") &&
            collisionTaget.gameObject.GetComponent<ChampionCard>().isOnBoard)
        {
            var damageAmount = gameObject.GetComponent<ChampionCard>().cardPower;

            collisionTaget.gameObject.GetComponent<ChampionCard>().TakeDamage(damageAmount);
            gameObject.GetComponent<ChampionCard>().hasAttacked = true;
            Debug.Log(gameObject.tag + " hit " + collisionTaget.gameObject.tag);
            collisionTaget = null;
        }

        if (gameObject.CompareTag("PlayerCard") &&
            collisionTaget.gameObject.CompareTag("EnemyAvatar")
            && gameObject.GetComponent<ChampionCard>().isOnBoard)
        {
            var damageAmount = gameObject.GetComponent<ChampionCard>().cardPower;

            collisionTaget.gameObject.GetComponent<Avatar>().TakeDamage(damageAmount);
            gameObject.GetComponent<ChampionCard>().hasAttacked = true;
            Debug.Log(gameObject.tag + " hit " + collisionTaget.gameObject.tag);
            collisionTaget = null;
            battleSystem.GetComponent<BattleSystem>().PlayerWon(); // will only do its thing if enemy dies
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // set the targets
    {
        if (collision.gameObject.CompareTag("EnemyZone"))
        {
            collision.gameObject.GetComponent<Image>().color = new Color32(114, 49, 49, 255);
            if (collision.gameObject.transform.childCount == 1)
            {
                collisionTaget = collision.gameObject.transform.GetChild(0).gameObject;
                Debug.Log(collisionTaget.name);
            }
            else
            {
                collisionTaget = null;
                Debug.Log(null);
            }
        }

        if (collision.gameObject.CompareTag("EnemyAvatar"))
        {
            collisionTaget = collision.gameObject;
            Debug.Log(collisionTaget.name);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyZone"))
        {
            collision.gameObject.GetComponent<Image>().color =
                new Color32(202, 126, 112, 0); // this will need to better, it loses the edge

            collisionTaget = null;
        }
        
        if (collision.gameObject.CompareTag("EnemyAvatar"))
        {
            collisionTaget = null;
        }
    }
}