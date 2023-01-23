using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCan : MonoBehaviour
{
    
    [SerializeField] float healthPoint = 5f;
    
    [SerializeField] Animator canAnimator;
    AudioSource audiosource;

    [SerializeField] float canDuration = 5f;
    [SerializeField] float canDestroy = 10f;
    float t;
    
    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > canDuration)
        {
            canAnimator.SetBool("WillDestroy", true);
        }

        if (t > canDestroy)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovementSM>().TakeHealth(healthPoint);
            AudioSource.PlayClipAtPoint(audiosource.clip, transform.position);
            Destroy(gameObject);
        }
    }
}
