using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_TriggerForDamage : MonoBehaviour
{
    [SerializeField] float damage = 50f;
     [SerializeField] float objHealth;
     [SerializeField] float objMaxHealth = 100f;
    // Start is called before the first frame update
    void Start()
    {
          objHealth = objMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
          if (objHealth <= 0)
        {
               Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyIA>().TakeDamage(damage);
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovementSM>().TakeDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovementSM>().TakeDamage(damage);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
   // {
     //   if (collision.gameObject.tag == "PlayerHitBox")
       // {
         //   objHealth -= 50f;
       // }
   // }
}
