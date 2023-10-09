using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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

    public void OnEndDrag(PointerEventData eventData) // handle the targets
    {
        if (gameObject.CompareTag("PlayerCard") && collisionTaget.gameObject.CompareTag("EnemyCard") && collisionTaget.gameObject.GetComponent<ThisCard>().isOnBoard)
        {                                                                                                                                        
            var damageAmount = gameObject.GetComponent<ThisCard>().cardPower;
                
            collisionTaget.gameObject.GetComponent<ThisCard>().TakeDamage(damageAmount);
            gameObject.GetComponent<ThisCard>().hasAttacked = true;
            Debug.Log(gameObject.tag + " hit " + collisionTaget.gameObject.tag);
        }
        
        if (gameObject.CompareTag("PlayerCard") &&
            collisionTaget.gameObject.CompareTag("EnemyAvatar")
            && gameObject.GetComponent<ThisCard>().isOnBoard) 
        {
            var damageAmount = gameObject.GetComponent<ThisCard>().cardPower;
                
            collisionTaget.gameObject.GetComponent<Avatar>().TakeDamage(damageAmount);
            Debug.Log(gameObject.tag + " hit " + collisionTaget.gameObject.tag);
        
            battleSystem.GetComponent<BattleSystem>().PlayerWon(); // will only do its thing if enemy dies
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) // set the targets
    {
        collisionTaget = collision.gameObject;
    }
}
