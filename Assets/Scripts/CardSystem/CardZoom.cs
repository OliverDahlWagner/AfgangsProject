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


        battleSystem.GetComponent<BattleSystem>().zoomCard.GetComponent<ZoomHelperScript>().SetZoomCardActive(gameObject);
        
    }
    public void OnPointerExit(PointerEventData eventData) // we want to delete the object if we aint hovering the hovered object
    {
        /*/*Debug.Log("Exiting");#1#
        Destroy(zoomCard);*/
        battleSystem.GetComponent<BattleSystem>().zoomCard.GetComponent<ZoomHelperScript>().ClearTheZoomCardArea();
    }

    public void OnDrag(PointerEventData eventData) // we want to delete the object if we move the hovered object
    {
        /*Destroy(zoomCard);*/
        battleSystem.GetComponent<BattleSystem>().zoomCard.GetComponent<ZoomHelperScript>().ClearTheZoomCardArea();
    }
}
