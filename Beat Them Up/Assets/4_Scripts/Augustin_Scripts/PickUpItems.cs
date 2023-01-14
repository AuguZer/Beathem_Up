using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [SerializeField] GameObject holdPoint;
    [SerializeField] GameObject graphics;

    SpriteRenderer sprite;


    public bool canBeHold;
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


    private void Throw()
    {
      
    }

    private void Follow()
    {
        sprite.sortingOrder = 1;
        transform.SetParent(holdPoint.transform);
        transform.position = holdPoint.transform.position;
    }

    private void IsHolded()
    {
        if (canBeHold)
        {
            if (Input.GetButtonDown("Hold"))
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
