using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public GameObject canvas;
    public GameObject zoomCardArea;

    private GameObject zoomCard;
    
    public GameObject battleSystem;
    

    public void Awake()
    {
        canvas = GameObject.Find("Main Canvas");
        battleSystem = GameObject.Find("Battle System");
        zoomCardArea = GameObject.Find("ZoomArea");
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (battleSystem.GetComponent<BattleSystem>().state != BattleState.PLAYERTURN || battleSystem.GetComponent<BattleSystem>().isPaused)
        {
            return;
        }
        
        if (!gameObject.CompareTag("PlayerCard")){return;} // this will need changing if the tags change (or end up doing something differnt)
        
        /*zoomCard = Instantiate(gameObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (float)3.25), Quaternion.identity); // a static position above the card*/
        zoomCard = Instantiate(gameObject, new Vector3(zoomCardArea.transform.position.x, zoomCardArea.transform.position.y + (float)3.25), Quaternion.identity); // a static position above the card
        zoomCard.transform.SetParent(canvas.transform, true); // the new cards that are created will not be in the canvas so we are setting them there

        zoomCard.layer = LayerMask.NameToLayer("Zoom"); // this is one solution to this (avoid collisionfrom hover card with dropzone) you could instantiate the zoomcard without a collider or something else

        zoomCard.transform.localScale = new Vector3((float)1.5, (float)1.5, (float)1.5);

        /*Debug.Log("zoom card position" + zoomCard.transform.position);*/
    }
    public void OnPointerExit(PointerEventData eventData) // we want to delete the object if we aint hovering the hovered object
    {
        /*Debug.Log("Exiting");*/
        Destroy(zoomCard);
    }

    public void OnDrag(PointerEventData eventData) // we want to delete the object if we move the hovered object
    {
        Destroy(zoomCard);
    }
}
