using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] GameObject graphics;
    [SerializeField] GameObject player;

    [SerializeField] float enemySpeed = 3f;
    [SerializeField] float EnemyHealth = 100f;
    [SerializeField] float EnemyDamage = 5f;
    [SerializeField] Animator animator;
    [SerializeField] EnemyState currentState;
    bool target = false;


    float Enemycurrentspeed;
    [SerializeField] Rigidbody2D rb2d;



    bool IsWalking;
    bool IsAttacking;
    bool IsDead;
    bool playerDetected = false;
    bool Attacked = false;

    Vector2 enemyDir;
    bool sprintInput;
    bool right = true;

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

        Target();
        currentState = EnemyState.Idle;
        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
        Move();
        Attack();

    }

    private void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                Enemycurrentspeed = enemySpeed;

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
                if (playerDetected)
                {
                    TransitionToState(EnemyState.Walk);
                }
                break;
            case EnemyState.Walk:
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Enemycurrentspeed * Time.deltaTime);

                if (!playerDetected)
                {
                    TransitionToState(EnemyState.Idle);
                }
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
        playerDetected = true;

    }

    public void PlayerUndetected()
    {
        playerDetected = false;
        target = false;
    }


    void Target()
    {



        if (playerDetected)
        {
            target = true;
        }
    }

    void Move()
    {
        if (playerDetected)
        {
            target = true;
        }
        enemyDir = player.transform.position - transform.position;


        if (enemyDir.x < 0)
        {
            right = false;
        }

        if (enemyDir.x > 0)
        {
            right = true;
        }

        if (right)
        {
            graphics.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            graphics.transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }

    void Attack()
    {
        if (playerDetected && Vector2.Distance(transform.position, player.transform.position) >= 0.5f)
        {
            IsAttacking = true;
        }
    }

}
