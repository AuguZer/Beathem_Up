using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampDestruct : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] bool washit = false;
    [SerializeField] float damages = 2f;
    [SerializeField] GameObject lampAnim;
    [SerializeField] GameObject lampBroken;

    public enum LampStates
    {
        IDLE,
        FALL,
        BROKEN
    }

    [SerializeField] LampStates currentState;

    float animTime = .8f;

    void OnStateEnter()
    {
        switch (currentState)
        {
            case LampStates.IDLE:
                break;
            case LampStates.FALL:
                lampAnim.SetActive(true);
                break;
            case LampStates.BROKEN:
                lampBroken.SetActive(true);
                break;
            default:
                break;
        }
    }

    void OnStateUpdate()
    {
        switch (currentState)
        {
            case LampStates.IDLE:

                // TO FALL
                if (health <= 0)
                {
                    TransitionToState(LampStates.FALL);
                }

                break;
            case LampStates.FALL:
                animTime -= Time.deltaTime;
                // TO BROKEN
                //if (lampAnim.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("BROKEN"))
                if (animTime <= 0)
                {
                    TransitionToState(LampStates.BROKEN);
                }

                break;
            case LampStates.BROKEN:
                break;
            default:
                break;
        }
    }

    void OnStateExit()
    {
        switch (currentState)
        {
            case LampStates.IDLE:
                GetComponent<SpriteRenderer>().enabled = false;
                Collider2D[] cols = GetComponents<Collider2D>();


                for (int i = 0; i < cols.Length; i++)
                {
                    cols[i].enabled = false;
                }


                //foreach (Collider2D collider in cols)
                //{
                //    collider.enabled = false;
                //}



                break;
            case LampStates.FALL:
                lampAnim.SetActive(false);
                break;
            case LampStates.BROKEN:
                break;
            default:
                break;
        }
    }


    void TransitionToState(LampStates nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D blabla)
    {
        CircleMove cm = blabla.gameObject.GetComponent<CircleMove>();

        //if (blabla.gameObject.tag == "HitPoint")
        if (cm != null)
        {
            //float damagePlayer = blabla.gameObject.GetComponent<CircleMove>().damage;
            float damagePlayer = cm.damage;

            washit = true;
            if (washit == true)
            {
                //health -= damagePlayer;
                DoDamage(damagePlayer);
            }
        }



    }

    void DoDamage(float blablaDamage)
    {
        health -= blablaDamage;
        Debug.Log("Health : " + health, gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HitPoint")
        {
            washit = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
    }
}
