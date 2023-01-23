using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoints : MonoBehaviour
{
    [SerializeField] float points = 200f;

    [SerializeField] Animator collectAnimator;

    [SerializeField] float collectDuration = 5f;
    [SerializeField] float collectDestroy = 10f;
    float t;

    AudioSource audioSource;
  
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > collectDuration)
        {
            collectAnimator.SetBool("WillDestroy", true);
        }

        if (t > collectDestroy)
        {
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            collision.gameObject.GetComponent<PlayerMovementSM>().TakePoints(points);
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            Destroy(gameObject);
        }
    }
}
