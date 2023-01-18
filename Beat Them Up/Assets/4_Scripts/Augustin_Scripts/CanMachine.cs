using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMachine : MonoBehaviour
{
    [SerializeField] float itemCurrentHealth;
    [SerializeField] float itemMaxHealth = 100f;
    [SerializeField] float damageTaken = 50;

    [SerializeField] Animator destAnimator;

    [SerializeField] GameObject [] spawnPoint;
    [SerializeField] GameObject [] _cans;

    bool distrib = true;

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
            Debug.Log("cc");
            destAnimator.SetTrigger("HIT");
            itemCurrentHealth -= damageTaken;

            if (distrib)
            {
            int i = Random.Range(0, _cans.Length);
            int s = Random.Range(0, spawnPoint.Length);
            GameObject go = Instantiate(_cans[i], spawnPoint[s].transform.position, transform.rotation);
            }
        }

        if (itemCurrentHealth <= 0)
        {
            distrib = false;
            destAnimator.SetBool("BROKEN", true);
        }
    }
}
