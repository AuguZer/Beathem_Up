using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [SerializeField] GameObject point;
    [SerializeField] GameObject graphics;

    SpriteRenderer sprite;


    bool canBeHold;
    // Start is called before the first frame update
    void Start()
    {
        sprite = graphics.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = 0; 
        
    }

    // Update is called once per frame
    void Update()
    {
        IsHolded();
    }

    private void Follow()
    {
        sprite.sortingOrder = 1;
        transform.SetParent(point.transform);
        transform.position = point.transform.position;
    }

    private void IsHolded()
    {
        if (canBeHold)
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                Follow();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canBeHold = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeHold = false;
    }
}
