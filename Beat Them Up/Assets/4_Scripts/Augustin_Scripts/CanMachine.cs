using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMachine : MonoBehaviour
{
    [SerializeField] float machineCurrentHealth;
    [SerializeField] float machineMaxHealth = 100f;
    [SerializeField] float damageTaken = 50;
    [SerializeField] float spawnRadius = .5f;

    [SerializeField] Animator destAnimator;

    //[SerializeField] GameObject [] spawnPoint;
    [SerializeField] GameObject [] _cans;
    [SerializeField] GameObject spawnCircle;

    Vector2 circlepos;

    bool distrib = true;

    // Start is called before the first frame update
    void Start()
    {
        machineCurrentHealth = machineMaxHealth;
        

    }

    // Update is called once per frame
    void Update()
    {



    }

    private void DistribCan()
    {
      
        
        circlepos = new Vector2(transform.position.x, -.5f);
        spawnCircle.transform.position = circlepos - Random.insideUnitCircle * spawnRadius;
        int i = Random.Range(0, _cans.Length);
        //int s = Random.Range(0, spawnPoint.Length);
        //GameObject go = Instantiate(_cans[i], spawnPoint[s].transform.position, transform.rotation);
        GameObject go = Instantiate(_cans[i], spawnCircle.transform.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerHitBox")
        {
            Debug.Log("cc");
            destAnimator.SetTrigger("HIT");
            machineCurrentHealth -= damageTaken;

            if (distrib)
            {
                DistribCan();
            }
        }

        if (machineCurrentHealth <= 0)
        {
            distrib = false;
            destAnimator.SetBool("BROKEN", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Items")
        {
            destAnimator.SetTrigger("HIT");
            machineCurrentHealth -= damageTaken;

            if (distrib)
            {
                DistribCan();
            }

            if (machineCurrentHealth <= 0)
            {
                distrib = false;
                destAnimator.SetBool("BROKEN", true);
            }
        }
    }
}
