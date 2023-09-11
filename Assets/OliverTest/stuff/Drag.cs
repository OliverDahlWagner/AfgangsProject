using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{

    private Transform _transform;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        v3.z = -9;
        _transform.position = v3;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _spriteRenderer.color = Color.red;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _spriteRenderer.color = Color.blue;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _spriteRenderer.color = Color.green;
    }


































    /*[Range(1,10)]
    public float speed = 5;     // currently irrelevant

    public GameObject circle;
    public SpriteRenderer renderer;


    private bool isMoving = false;
    private Vector3 targetPosition;
    
    private Vector3 offset;
    private bool selected = false;

   
    
    private void Update()
    {

        /*if (selected)
        {
            renderer.color = Color.red;
        }
        else
        {
            renderer.color = Color.green;
        }#1#


        /*if (Input.GetMouseButtonDown(1))
        {
            /*if (selected)
            {
                SetTargetPosition();
            }#2#

            /*SetTargetPosition();#2#
            
        }#1#

        /*if (selected)
        {
            if (isMoving)
            {
                Move();
            }
        }#1#
        
        /*if (isMoving)
        {
            Move();
        }#1#

        
        /*if (selected && !isMoving)
        {
            SetTargetPosition();
        }#1#
        
        
        /*
        while(!selected) // essentially a "while true", but with a bool to break out naturally
        {
            if(Input.GetMouseButtonDown(0))
            {
                selected = true; // breaks the loop
            }
            return; // wait until next frame, then continue execution from here (loop continues)
        }
        #1#

    }

    /*void OnMouseDown()
    {
        while (!selected)
        {
            selected = true;
        }

        while (selected && !isMoving)
        {
            
        }

        


    }#1#

    void SetTargetPosition()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = transform.position.z;

        isMoving = true;
        
        Debug.Log(targetPosition);
        
    }

    void Move()
    {
        // there was some Quaternion, what would change the rotation of the object not need because this is a circle
        /*transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);#1#

        var startPosition = transform.position;
        startPosition.x = targetPosition.x;
        startPosition.y = targetPosition.y;
        startPosition.z = targetPosition.z;

        transform.position = startPosition;
        
            isMoving = false;
            selected = false;
            
        
        
    }*/


    ///////////////////////////////////////

    // this is for drag movement

    /*private bool dragging;
    private Vector3 offset;

    private bool set = false;
    private Vector3 start;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            }

        }

        /*while (dragging)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                var position = new Vector3();
                position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = -3;
                transform.position = position;
                dragging = false;
            }
        }#1#
    }

    private void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dragging = true;
    }
    
    private void OnMouseUp()
    {

        // stopping dragging
        /*dragging = false;#1#
    }*/



}
