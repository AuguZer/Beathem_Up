using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleItems : MonoBehaviour
{
    [SerializeField] float itemCurrentHealth;
    [SerializeField] float itemMaxHealth = 100f;
    [SerializeField] float damageTaken = 50;

    [SerializeField] Animator destAnimator;

    // Start is called before the first frame update
    void Start()
    {
        itemCurrentHealth = itemMaxHealth;
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
            itemCurrentHealth -= damageTaken;
        }

        if (itemCurrentHealth <= 0)
        {
            destAnimator.SetBool("DEAD", true);
        }
    }
}
