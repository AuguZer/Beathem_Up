using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    [Header("OverlapCircle Parameters")]

    public float detectionRadius = 1f;
    public LayerMask detectorLayerMask;


    [Header("GIVE POINTS")]
    [SerializeField] GameObject[] _pointsItems;
    [SerializeField] GameObject _pointCircle;
    [SerializeField] float radiusSpawn = .5f;
    Vector2 posCircle;

    [Header("Info Enemy")]
    [SerializeField] GameObject graphics;
    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject player;

    [SerializeField] float enemySpeed = 3f;
    [SerializeField] float EnemyHealth = 100f;
    [SerializeField] float EnemyCurrentHealth = 100f;
    [SerializeField] float EnemyDamage = 5f;
    [SerializeField] float AttackZone = 1f;

    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] EnemyState currentState;
    float Enemycurrentspeed;
    Vector2 enemyDir;

    Collider2D playerCollider;

    float attackNumber = 0;
    bool AttackSet;

    [Header("Bool Enemy")]
    bool target = false;

    bool IsDead;
    bool playerDetected = false;
    // bool IsWalking;


    bool right = true;
    bool isHurt;






    public enum EnemyState

    {
        Idle,
        Walk,
        Attack,
        Dead,
        Hurt
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
    }

    private void OnStateEnter()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                //isHurt = false;

                break;
            case EnemyState.Walk:
                Enemycurrentspeed = enemySpeed;

                animator.SetBool("IsWalking", true);

                break;
            case EnemyState.Attack:

                StartCoroutine(ReloadAttack());

                break;
            case EnemyState.Hurt:
                isHurt = true;
                animator.SetTrigger("HURT");
                StartCoroutine(Hurt());


                break;

            case EnemyState.Dead:
                StartCoroutine(Death());
                IsDead = true;
                GivePoints();
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


                // TO ATTACK

                if (playerDetected && Vector2.Distance(transform.position, player.transform.position) <= AttackZone)
                {
                    TransitionToState(EnemyState.Attack);


                    animator.SetFloat("AttackNumber", attackNumber);

                    attackNumber = attackNumber >= 1 ? 0 : attackNumber + 1;

                    AttackSet = true;

                }



                //TO DEATH
                if (IsDead)
                {
                    TransitionToState(EnemyState.Dead);
                }

                //TO HURT
                if (isHurt)
                {
                    TransitionToState(EnemyState.Hurt);
                }

                break;

            case EnemyState.Walk:
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemySpeed * Time.deltaTime);

                // TO IDLE
                if (!playerDetected && !playerCollider)
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

                //TO HURT
                if (isHurt)
                {
                    TransitionToState(EnemyState.Hurt);
                }

                break;
            case EnemyState.Attack:

                if (IsDead)
                {
                    TransitionToState(EnemyState.Dead);
                }

                //TO HURT
                if (isHurt)
                {
                    TransitionToState(EnemyState.Hurt);
                }

                break;


            case EnemyState.Hurt:
                animator.SetLayerWeight(1, 1f);

                break;
            case EnemyState.Dead:
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

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
                animator.SetBool("Idle", false);
                break;
            case EnemyState.Walk:
                animator.SetBool("IsWalking", false);
                break;
            case EnemyState.Attack:
                StopAllCoroutines();
                hitbox.SetActive(false);
                animator.SetBool("IsAttacking", false);

                break;

            case EnemyState.Hurt:
                animator.SetLayerWeight(0, 1f);
                isHurt = false;
                //animator.SetBool("Hurt", false);
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

    private void GivePoints()
    {
        posCircle = new Vector2(transform.position.x, transform.position.y);
        _pointCircle.transform.position = posCircle - Random.insideUnitCircle * radiusSpawn;
        int i;

        i = Random.Range(0, _pointsItems.Length);
        GameObject go = Instantiate(_pointsItems[i], _pointCircle.transform.position, transform.rotation);
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

        isHurt = true;


        if (EnemyCurrentHealth <= 0)
        {
            IsDead = true;
        }

        if (IsDead)
        {
            animator.SetTrigger("IsDead");
        }

    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }
    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(.3f);

        // TO IDLE
        if (!playerDetected && !playerCollider)
        {
            TransitionToState(EnemyState.Idle);
        }

        //TO WALK
        if (playerDetected)
        {
            TransitionToState(EnemyState.Walk);
        }


        // TO ATTACK

        if (playerDetected && Vector2.Distance(transform.position, player.transform.position) <= AttackZone)
        {
            TransitionToState(EnemyState.Attack);


            animator.SetFloat("AttackNumber", attackNumber);

            attackNumber = attackNumber >= 1 ? 0 : attackNumber + 1;

            AttackSet = true;

        }



        //TO DEATH
        if (IsDead)
        {
            TransitionToState(EnemyState.Dead);
        }


    }
    IEnumerator ReloadAttack()
    {


        yield return new WaitForSeconds(Random.Range(1f, 3f));

        //animator.SetBool("IsAttacking", true);
        hitbox.SetActive(true);
        animator.SetTrigger("IsAttacking");

        playerCollider = Physics2D.OverlapCircle(hitbox.transform.position, detectionRadius, detectorLayerMask);



        if (playerCollider != null)
        {
            playerCollider.GetComponent<PlayerMovementSM>().TakeDamage(EnemyDamage);
            // CREATE PARTICLE
            //GameObject go = Instantiate(hitbox, hitbox.transform.position, hitbox.transform.rotation);
        }

        if (playerCollider == null)
        {
            yield return new WaitForSeconds(1f);

            TransitionToState(EnemyState.Idle);

        }




    }

}
