using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructLamp : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] bool washit = false;
    [SerializeField] float Damages = 1f;
    [SerializeField] float currentWearDamages = 0f;
    [SerializeField] float currentAttackDamages;
    [SerializeField] GameObject lampAnim;
    [SerializeField] GameObject lampStanding;
    [SerializeField] GameObject lampBroken;


    // Start is called before the first frame update
    void Start()
    {
        lampBroken.GetComponent<SpriteRenderer>().enabled = false;
        Collider2D[] lamp2BrokenColliders = lampBroken.GetComponents<Collider2D>();
        for (int i = 0; i < lamp2BrokenColliders.Length; i++)
        {
            lamp2BrokenColliders[i].enabled = false;
        }

        lampAnim.GetComponent<SpriteRenderer>().enabled = false;
        Collider2D[] lamp2AnimColliders = lampAnim.GetComponents<Collider2D>();
        for (int i = 0; i < lamp2AnimColliders.Length; i++)
        {
            lamp2AnimColliders[i].enabled = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D lampWear)
    {
        if (lampWear.gameObject.tag == "HitPoint")
        {
            washit = true;
            currentWearDamages = Damages;
            health -= currentWearDamages;
        }

    }

    private void OnCollisionExit2D(Collision2D lampWear)
    {
        if (lampWear.gameObject.tag == "HitPoint")
        {
            washit = false;
            currentWearDamages = 0 ;
        }
    }

    private void OnTriggerEnter2D(Collider2D lampAttacked)
    {
        if (lampAttacked.gameObject.tag == "AttackPoint")
        {
            washit = true;
            currentAttackDamages = lampAttacked.gameObject.GetComponent<CircleMove>().damage;
            health -= currentAttackDamages;
        }
    }

    private void OnTriggerExit2D(Collider2D currentAttackDamagesReset)
    {
        if (currentAttackDamagesReset.gameObject.tag == "AttackPoint")
        {
            washit = false;
            currentAttackDamages = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
