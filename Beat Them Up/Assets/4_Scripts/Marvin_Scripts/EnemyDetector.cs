using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] GameObject player;
    CircleCollider2D cc2D;
    [SerializeField] float detectionRadius = 2f;
    [SerializeField] LayerMask detectionLayer;



    private void Start()
    {
        cc2D = GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);

        if (collision.gameObject.tag =="Player")

        {
              Physics2D.OverlapCircle(transform.position, detectionRadius,detectionLayer);

           

            Debug.Log("Detecté");

            
        }
    }
    
}
