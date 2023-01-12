using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCan : MonoBehaviour
{
    
    [SerializeField] float healthPoint = 5f;
    
    [SerializeField] Animator canAnimator;

    [SerializeField] float canDuration = 3f;
    float t;
    bool willDestroy;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > canDuration)
        {
            willDestroy = true;
        }

        if (willDestroy)
        {

        }
        
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
