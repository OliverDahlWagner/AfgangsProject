using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject canvas;
    private GameObject startParent;

    private bool isOverDropZone = false;

    private GameObject dropZone;

    private Vector3 startPosition;


    public void Awake() // calls when script/object is instantiated
    {
        canvas = GameObject.Find("Main Canvas"); // will find the Object with the scene of the same name
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent.gameObject; // when we start dragging save the parent object
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = v3;


        transform.position = new Vector3(transform.position.x, transform.position.y, 0);


        transform.SetParent(canvas.transform,
            true); // here we pull the card out of hand zone, in unity you can see the card in main canvas
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (isOverDropZone && dropZone.transform.childCount < 1) // I write 2 here ... <=2
        {
            transform.SetParent(dropZone.transform, false);
            Debug.Log(dropZone.transform.childCount); // but it prints 3 here, when maxed ?
            dropZone.GetComponent<Image>().color = new Color32(67, 89, 87, 255);
        }
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform,
                false); // because the card is no longer in the hand zone, it will need to placed back
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)  // placing in general is a little buggy (maybe something to do with colliders)
    {
        isOverDropZone = true;

        if ((gameObject.CompareTag("EnemyCard") &&
             collision.gameObject.CompareTag("EnemyZone")) || // both ways work, write the comparison
            (gameObject.tag == "PlayerCard" && collision.gameObject.tag == "PlayerZone"))
        {
            if (1 == collision.gameObject.transform
                    .childCount) // so there wont be a indicator that it can be dropped if it contains card. Also a good check to have. Not really needed because of the check in endDrag
            {
                return;
            }

            dropZone = collision.gameObject; // here we set the gameobject to the collision object (the dropzone)

            if (dropZone.CompareTag("PlayerZone")) // no need for enemy recolor
            {
                dropZone.GetComponent<Image>().color = new Color32(124, 193, 191, 255);
            } 
            
            Debug.Log(dropZone.name + " : Dropzone is set");
        }
        else
        {
            isOverDropZone = false;
        }


        /*if (gameObject.CompareTag(collision.gameObject.tag))
        {
            dropZone = collision.gameObject; // here we set the gameobject to the collision object (the dropzone)
        }*/
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        if (dropZone != null) dropZone.GetComponent<Image>().color = new Color32(67, 89, 87, 255);
        dropZone = null;
    }
}