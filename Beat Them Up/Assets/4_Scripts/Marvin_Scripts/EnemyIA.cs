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
    Transform target;


    float Enemycurrentspeed;
    [SerializeField] Rigidbody2D rb2d;



    bool IsWalking;
    bool IsAttacking;
    bool IsDead;
    bool playerDetected = false;

    Vector2 dirInput;
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
        currentState = EnemyState.Idle;
        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        OnStateUpdate();
        GetInput();
    }

    private void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                Enemycurrentspeed = enemySpeed;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime);
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
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 3 * Time.deltaTime);
    }

    public void PlayerUndetected()
    {
        playerDetected = false;
    }

    private void GetInput()
    {
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //rb2d.velocity = dirInput.normalized * walkSpeed;

        if (dirInput != Vector2.zero)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        if (dirInput.x != 0 && !IsDead)
        {
            right = dirInput.x > 0;
            graphics.transform.rotation = right ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }

    }
}
