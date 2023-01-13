using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [SerializeField] GameObject point;
    [SerializeField] GameObject endPoint;
    [SerializeField] GameObject graphics;

    Vector2 startPos;
    Vector2 endPos;


    SpriteRenderer sprite;


    public bool canBeHold;
    // Start is called before the first frame update
    void Start()
    {
        sprite = graphics.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = 0;

        startPos = point.transform.position;
        endPos = endPoint.transform.position;



    }

    // Update is called once per frame
    void Update()
    {
        IsHolded();
        Throw();
    }


    private void Throw()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = Vector2.Lerp(startPos, endPos, 20f);
        }
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
