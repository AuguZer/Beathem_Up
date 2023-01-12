using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] float enemySpeed = 3f;
    [SerializeField] float EnemyHealth = 100f;
    [SerializeField] float EnemyDamage = 5f;
    [SerializeField] Animator animator;


    float Enemycurrentspeed;
    [SerializeField] Rigidbody2D rb2d;
    

    
    bool IsWalking;
    bool IsAttacking;


    public enum EnemyState
    {
        Idle_Enemy_Regular,
        Walk_Enemy_Regular,
        Attack_Enemy_Regular,
        Death_Enemy_Regular,
    }
    [SerializeField] EnemyState currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Idle_Enemy_Regular;
        OnStateEnter();
    }


   private void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.Idle_Enemy_Regular:

                Enemycurrentspeed = enemySpeed;

                rb2d.velocity = Vector2.zero;

                break;
            case EnemyState.Walk_Enemy_Regular:
                animator.SetBool("IsWalking", true);
                Enemycurrentspeed = enemySpeed;
                break;
            case EnemyState.Attack_Enemy_Regular:
                break;
            case EnemyState.Death_Enemy_Regular:
                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle_Enemy_Regular:
                if (IsWalking)
                {
                    TransitionToState(EnemyState.Walk_Enemy_Regular);
                }
                break;
            case EnemyState.Walk_Enemy_Regular:
                if (!IsWalking)
                {
                    TransitionToState(EnemyState.Idle_Enemy_Regular);
                }   

                break;
            case EnemyState.Attack_Enemy_Regular:
                animator.SetBool("IsAttacking", true);
                break;
            case EnemyState.Death_Enemy_Regular:
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (currentState)
        {
            case EnemyState.Idle_Enemy_Regular:
                break;
            case EnemyState.Walk_Enemy_Regular:
                animator.SetBool("IsWalking", false);

                break;
            case EnemyState.Attack_Enemy_Regular:
                animator.SetBool("IsAttacking", false);

                break;
            case EnemyState.Death_Enemy_Regular:
                break;
            default:
                break;
        }
    }

    private void TransitionToState(EnemyState nextstate)
    {
        OnStateExit();
        currentState = nextstate;
        OnStateEnter();
    }
    
    
    void PlayerDetected()

    {
        Debug.Log("PlayerDetected");
    }


    void Update()
    {
        
        OnStateUpdate();


    }




}
