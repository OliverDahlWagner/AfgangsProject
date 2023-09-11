using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Click : MonoBehaviour,  IPointerClickHandler 
{
    
    public SpriteRenderer renderer;
    private Vector3 targetPosition;


    public void OnPointerClick(PointerEventData eventData)
    {
        /*renderer.color = Color.red;*/

        /*
        var positionx = eventData.position.x;
        var positiony = eventData.position.y;
        
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z;
        var startPosition = transform.position;
        startPosition.x = targetPosition.x;
        startPosition.y = targetPosition.y;
        startPosition.z = targetPosition.z;

        transform.position = startPosition;
        */

        

    }


}
