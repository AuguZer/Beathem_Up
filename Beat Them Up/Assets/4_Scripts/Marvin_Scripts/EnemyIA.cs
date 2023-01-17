using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    [Header("OverlapCircle Parameters")]
   
    public float detectionRadius = .7f;
    public LayerMask detectorLayerMask;




    [Header("Info Enemy")]
    [SerializeField] GameObject graphics;
    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject player;

    [SerializeField] float enemySpeed = 3f;
    [SerializeField] float EnemyHealth = 100f;
    [SerializeField] float EnemyCurrentHealth = 100f;
    [SerializeField] float EnemyDamage = 5f;
    [SerializeField] float AttackZone = .5f;
    [SerializeField] float AttackTimer;
    [SerializeField] float AttackDelay = 3f;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] EnemyState currentState;
    float Enemycurrentspeed;
    Vector2 enemyDir;

    




    [Header("Bool Enemy")]
    bool target = false;
    bool IsWalking;
    bool IsAttacking;
    bool IsDead;
    bool playerDetected = false;
    bool Attacked = false;
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
        Move();
        Target();


    }

    private void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                AttackTimer = 0f;
                break;
            case EnemyState.Walk:
                Enemycurrentspeed = enemySpeed;

                animator.SetBool("IsWalking", true);
                break;
            case EnemyState.Attack:
                //animator.SetBool("IsAttacking", true);
                hitbox.SetActive(true);
                animator.SetTrigger("IsAttacking");

                Collider2D player = Physics2D.OverlapCircle(hitbox.transform.position, detectionRadius, detectorLayerMask);

                
                if (player != null)
                {
                    player.GetComponent<PlayerHealth>().TakeDamage(EnemyDamage);
                
                    // CREATE PARTICLE
                    //GameObject go = Instantiate(hitbox, hitbox.transform.position, hitbox.transform.rotation);

                }
                StartCoroutine(ReloadAttack());

                break;
            case EnemyState.Dead:
                IsDead = true;
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
                if (playerDetected && Vector2.Distance(transform.position, player.transform.position) > AttackZone)
                {
                    TransitionToState(EnemyState.Walk);
                }

                // TO ATTACK

                if (playerDetected && Vector2.Distance(transform.position, player.transform.position) <= AttackZone)
                {
                    TransitionToState(EnemyState.Attack);
                }

                if (IsDead)
                {
                    TransitionToState(EnemyState.Dead);
                }

                break;
            case EnemyState.Walk:
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Enemycurrentspeed * Time.deltaTime);

                // TO IDLE
                if (!playerDetected)
                {
                    TransitionToState(EnemyState.Idle);
                }

                // TO ATTACK
                if (Vector2.Distance(transform.position, player.transform.position) <= AttackZone)
                {
                    TransitionToState(EnemyState.Attack);
                }

                if (IsDead)
                {
                    TransitionToState(EnemyState.Dead);
                }

                break;
            case EnemyState.Attack:
                if (!playerDetected)
                {
                    TransitionToState(EnemyState.Idle);
                }


                if (IsDead)
                {
                    TransitionToState(EnemyState.Dead);
                }
                break;
            case EnemyState.Dead:


                if (IsDead)
                {
                    TransitionToState(EnemyState.Dead);
                }

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
                hitbox.SetActive(false);
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


    public void Target()
    {


        if (playerDetected)
        {
            target = true;

        }



        if (AttackTimer >= AttackDelay)
        {
            TransitionToState(EnemyState.Attack);

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

    public void TakeDamage(float amount)
    {
        EnemyCurrentHealth -= amount;

        if (EnemyCurrentHealth <= 0)
        {
            IsDead = true;
        }

        if (IsDead)
        {
            animator.SetTrigger("IsDead");

            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

           

            
        }


    }
    


    IEnumerator ReloadAttack()
    {


        yield return new WaitForSeconds(2f);

        TransitionToState(EnemyState.Idle);



    }

}
