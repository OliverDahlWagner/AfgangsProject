using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTargeting : MonoBehaviour, IEndDragHandler
{

    private bool isPlayerTurn = true; // might need to get this boolean from a turn combat handler

    private GameObject collisionTaget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndDrag(PointerEventData eventData) // handle the targets
    {
        if (isPlayerTurn)
        {
            if (gameObject.CompareTag("PlayerCard") &&
                collisionTaget.gameObject.CompareTag("EnemyCard"))
            {
                var damageAmount = gameObject.GetComponent<ThisCard>().cardPower;
                
                collisionTaget.gameObject.GetComponent<ThisCard>().TakeDamage(damageAmount);
                Debug.Log(gameObject.tag + " hit " + collisionTaget.gameObject.tag);
            }
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision) // set the targets
    {
        collisionTaget = collision.gameObject;
    }
}
