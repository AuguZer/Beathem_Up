using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [SerializeField] GameObject holdPoint;
    [SerializeField] GameObject graphics;
    [SerializeField] GameObject player;

    [SerializeField] float throwSpeed = 1f;
    float t;
    float throwDuration = 2f;

    Rigidbody2D rb2dItem;
    SpriteRenderer sprite;

    public bool canBeHold;
    // Start is called before the first frame update
    void Start()
    {
        sprite = graphics.GetComponent<SpriteRenderer>();
        sprite.sortingOrder = 0;

        rb2dItem = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        IsHolded();

        if (Input.GetKeyDown(KeyCode.A))
        {
            t += Time.deltaTime;

            Debug.Log("A");
            rb2dItem.AddForce(transform.right * throwSpeed, ForceMode2D.Impulse);
            rb2dItem.gravityScale = .5f;

            if (t > throwDuration)
            {
                rb2dItem.velocity = Vector2.zero;
                rb2dItem.gravityScale = 0f;
            }
        }
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
        if (collision.gameObject.tag == "Player")
        {
            canBeHold = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canBeHold = false;
        }
    }
}
