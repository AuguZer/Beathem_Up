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
            Debug.Log("no");
            psHit.SetActive(false);
        }

        if (collision.gameObject.tag == "Destructibles")
        {
            Debug.Log("ok");
            psHit.SetActive(true);
        } 
        if (collision.gameObject.tag == "Items")
        {
            Debug.Log("ok");
            psHit.SetActive(true);
        } 

        if (collision.gameObject.tag == "Machine")
        {
            Debug.Log("ok");
            psHit.SetActive(true);
        }

        //if (collision.gameObject.tag == "Enemy")
        //{
        //    Debug.Log("no");
        //    psHit.SetActive(false);
        //}

        if (collision.gameObject.tag == "TakeDamage")
        {
            Debug.Log("damage");
            enemyPrefab.GetComponent<EnemyIA>().TakeDamage(hitDamage);
            psHit.SetActive(true);
        }
    }
}
