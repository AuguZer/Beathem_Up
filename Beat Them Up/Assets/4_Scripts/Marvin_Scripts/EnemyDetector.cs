using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] GameObject player;



    EnemyIA enemyIA;

    private void Start()
    {
        enemyIA = GetComponent<EnemyIA>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")

        {

            enemyIA.PlayerDetected();



        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyIA.PlayerUndetected();
        }

        

    }
}
