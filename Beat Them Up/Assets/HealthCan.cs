using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCan : MonoBehaviour
{
    [SerializeField] Animator canAnimator;
    [SerializeField] GameObject canGraphics;
    [SerializeField] float healthPoint = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovementSM>().TakeHealth(healthPoint);
            Destroy(gameObject);
        }
    }
}
