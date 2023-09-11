using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    public Vector2 id;
    


    public void Init(bool isOffset) {
        if (isOffset)
        {
            _renderer.color = Color.grey;
        }
        else
        {
            _renderer.color = Color.black;
        }
    }

    public void changeColor()
    {
        _renderer.color = Color.red;
    }

    void OnMouseEnter() {
        _highlight.SetActive(true);
    }
 
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
}