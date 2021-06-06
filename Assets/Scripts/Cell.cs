using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public bool Alive
    {
        get { return _alive; }
        set
        {
            _alive = value;
            UpdateSprite();
        }
    }
    bool _alive = false;

    public bool WillBeAlive = false;

    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void UpdateSprite()
    {
        if (Alive)
        {
            sprite.color = Color.white;
        }
        else
        {
            sprite.color = Color.black;
        }
    }



    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            Alive = !Alive;
        }
    }
}
