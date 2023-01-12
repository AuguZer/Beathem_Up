using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] float enemySpeed = 3f;
    [SerializeField] float EnemyHealth = 100f;
    [SerializeField] float EnemyDamage = 5f;
    [SerializeField] Animator animator;
    [SerializeField] EnemyState currentState;


    float Enemycurrentspeed;
    [SerializeField] Rigidbody2D rb2d;



    bool IsWalking;
    bool IsAttacking;
    bool IsDead;
    bool PlayerDetected = false;

    public enum EnemyState

    {
        Idle,
        Walk,
        Attack,
        Dead,
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyState.Idle;
        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
    }

    private void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                animator.SetBool("IsWalking", true);
                break;
            case EnemyState.Attack:
                animator.SetBool("IsAttacking", true);
                break;
            case EnemyState.Dead:

                break;
            default:
                break;
        }
    }

    private void OnStateUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }
    }

    private void OnStateExit()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                animator.SetBool("IsWalking", false);
                break;
            case EnemyState.Attack:
                animator.SetBool("IsAttacking", false);
                break;
            case EnemyState.Dead:
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

    public void PlayerDetected()
    {
        Debug.Log("Detecté");
    }

}
