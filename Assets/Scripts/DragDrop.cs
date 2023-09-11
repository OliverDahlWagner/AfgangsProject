using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    public GameObject canvas;

    private bool isOverDropZone = false;

    private GameObject dropZone;

    private Vector3 startPosition;


    public void Awake() // calls when script/object is instantiated
    {
        canvas = GameObject.Find("Main Canvas"); // will find the Object with the scene of the same name
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = v3;


        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

      
        
        /*transform.SetParent(canvas.transform, true);*/
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (isOverDropZone)
        {
            transform.SetParent(dropZone.transform, false);
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject; // here we set the gameobject to the collision object (the dropzone)
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }
}
