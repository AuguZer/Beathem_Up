using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPS : MonoBehaviour
{
    [SerializeField] GameObject psHit;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float hitDamage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        psHit.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Untagged")
        {
            psHit.SetActive(false);
        }
        else
        {
            psHit.SetActive(true);
        }

        if (collision.gameObject.tag == "TakeDamage")
        {
            enemyPrefab.GetComponent<EnemyIA>().TakeDamage(hitDamage);
        }
    }
}
