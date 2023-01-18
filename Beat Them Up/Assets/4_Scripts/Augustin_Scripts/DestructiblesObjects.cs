using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblesObjects : MonoBehaviour
{
    [SerializeField] float objectCurrentHealth;
    [SerializeField] float objectMaxHealth = 100f;
    [SerializeField] float damageTaken = 50;

    [SerializeField] Animator destAnimator;

    BoxCollider2D bx2d;
    // Start is called before the first frame update
    void Start()
    {
        bx2d = GetComponent<BoxCollider2D>();
        objectCurrentHealth = objectMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerHitBox")
        {
            destAnimator.SetTrigger("HIT");
            objectCurrentHealth -= damageTaken;
        }

        if (objectCurrentHealth <= 0)
        {
            destAnimator.SetTrigger("BROKEN");
            bx2d.enabled = false;
        }
    }
}
