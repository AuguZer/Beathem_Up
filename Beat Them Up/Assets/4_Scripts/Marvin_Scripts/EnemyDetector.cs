using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] GameObject player;
    
    [SerializeField] float detectionRadius = 2f;
    [SerializeField] LayerMask detectionLayer;

    EnemyIA enemyIA;

    private void Start()
    {
        enemyIA = GetComponent<EnemyIA>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag =="Player")

        {


            enemyIA.PlayerDetected();
            

            
        }
    }
    
}
