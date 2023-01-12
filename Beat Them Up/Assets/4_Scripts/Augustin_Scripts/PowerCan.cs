using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCan : MonoBehaviour
{
    [SerializeField] float powerPoints = 20f;

    [SerializeField] Animator powerCanAnimator;

    float t;
    [SerializeField] float powerCanDuration = 5f;
    [SerializeField] float powerCanDestroy = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        if (t > powerCanDuration)
        {
            powerCanAnimator.SetBool("WillDestroy", true);
        }

        if (t > powerCanDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovementSM>().TakePower(powerPoints);
            Destroy(gameObject);
        }
    }
}
