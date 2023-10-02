using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickThing : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if( Input.GetMouseButtonDown(0) )
        {
            
            Debug.Log(  "Click was hit" );
            
            /*var ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            RaycastHit hit;
		
            if( Physics.Raycast( rayray, out hit, 100 ) )
            {
                Debug.Log( hit.transform.gameObject.name + " was hit" );
            }*/
        }
    }
}
