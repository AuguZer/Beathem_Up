using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [SerializeField] GameObject holdPoint;
    [SerializeField] GameObject graphics;
    [SerializeField] GameObject player;

    SpriteRenderer sprite;

    BoxCollider2D bx2d;

    public bool canBeHold;
    // Start is called before the first frame update
    void Start()
    {
        sprite = graphics.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = 0;

        bx2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsHolded();
    }



    private void Follow()
    {
        sprite.sortingOrder = 1;
        transform.SetParent(holdPoint.transform);
        transform.position = holdPoint.transform.position;
        bx2d.enabled = false;

        if (bx2d.enabled == false)
        {
            StartCoroutine(BoxCR());
        }
        
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canBeHold = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canBeHold = false;
        }
    }

    IEnumerator BoxCR()
    {
        yield return new WaitForSeconds(1f);
        bx2d.enabled = true;
    }
}
