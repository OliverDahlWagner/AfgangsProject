using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTargeting : MonoBehaviour, IEndDragHandler
{
    
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
        var isOnBoard = gameObject.GetComponent<ThisCard>().isOnBoard;
        if (gameObject.CompareTag("PlayerCard") && collisionTaget.gameObject.CompareTag("EnemyCard") & isOnBoard)  // this can hit card still in hand. need to make a variable so (bool isPlayed or something)
        {                                                                                                                      // so only is played card interact    just another if below should work                       
            var damageAmount = gameObject.GetComponent<ThisCard>().cardPower;
                
            collisionTaget.gameObject.GetComponent<ThisCard>().TakeDamage(damageAmount);
            
            gameObject.GetComponent<ThisCard>().hasAttacked = true;
            
            Debug.Log(gameObject.tag + " hit " + collisionTaget.gameObject.tag);
        }
        
        if (gameObject.CompareTag("PlayerCard") &&
            collisionTaget.gameObject.CompareTag("EnemyAvatar")
            && isOnBoard) // this work (will go in minus, but lost state is not made yet)
        {
            var damageAmount = gameObject.GetComponent<ThisCard>().cardPower;
                
            collisionTaget.gameObject.GetComponent<Avatar>().TakeDamage(damageAmount);
            Debug.Log(gameObject.tag + " hit " + collisionTaget.gameObject.tag);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) // set the targets
    {
        collisionTaget = collision.gameObject;
    }
}
