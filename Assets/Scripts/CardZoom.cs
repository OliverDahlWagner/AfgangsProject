using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public GameObject canvas;

    private GameObject zoomCard;
    

    public void Awake()
    {
        canvas = GameObject.Find("Main Canvas");
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        zoomCard = Instantiate(gameObject, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (float)3.25), Quaternion.identity); // a static position above the card
        zoomCard.transform.SetParent(canvas.transform, true); // the new cards that are created will not be in the canvas so we are setting them there

        zoomCard.layer = LayerMask.NameToLayer("Zoom"); // this is one solution to this (avoid collisionfrom hover card with dropzone) you could instantiate the zoomcard without a collider or something else
        
        var rectTransform = zoomCard.GetComponent<RectTransform>(); // get recttransform component from zoomcard
        rectTransform.sizeDelta = new Vector2((float)2.2, (float)3.4); // a 50% size increase
        
        Debug.Log("zoom card position" + zoomCard.transform.position);
    }
    public void OnPointerExit(PointerEventData eventData) // we want to delete the object if we aint hovering the hovered object
    {
        Debug.Log("Exiting");
        Destroy(zoomCard);
    }

    public void OnDrag(PointerEventData eventData) // we want to delete the object if we move the hovered object
    {
        Destroy(zoomCard);
    }
}
