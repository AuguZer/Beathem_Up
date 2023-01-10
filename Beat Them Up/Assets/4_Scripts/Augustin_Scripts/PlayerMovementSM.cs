using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSM : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] GameObject graphics;
    public enum PlayerState
    {
        IDLE_Player,
        WALK_Player,
        SPRINT_Player,
        ATTACK_Player,
        DEATH_Player,
        JUMP_Player,

    }

    //PLAYER MOVEMENT & ANIMATION
    [SerializeField] PlayerState currentState;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    Vector2 dirInput;
    bool sprintInput;
    Rigidbody2D rb2d;
    bool right = true;

    //HEALTH & DEATH
    [SerializeField] public float playerMaxHealth = 100f;
    [SerializeField] public float playerCurrentHealth;
    bool isDead;


    //JUMP
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpDuration = 3f;
    Transform _graphics;
    float jumpTimer;
    bool isJumping;

    //POINTS
    [SerializeField] float playerCurrentPoints;

    //ATTACK
    int attackNumber = 0;
    bool isAttacking;
    float t;
    float attackCoolDown = 2f;

    private void Awake()
    {
        _graphics = transform.Find("GRAPHICS");
    }
    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        playerCurrentPoints = 0f;
        rb2d = GetComponent<Rigidbody2D>();
        currentState = PlayerState.IDLE_Player;
        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        OnStateUpdate();
        Jump();
        Attack();

    }
    private void Jump()
    {

        if (isJumping && jumpTimer < jumpDuration)
        {
            jumpTimer += Time.deltaTime;
            float y = jumpCurve.Evaluate(jumpTimer / jumpDuration);

            _graphics.localPosition = new Vector3(_graphics.transform.localPosition.x, y * jumpHeight, _graphics.transform.localPosition.z);
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
        }

        if (jumpTimer > jumpDuration)
        {
            isJumping = false;
            jumpTimer = 0f;
            rb2d.constraints = RigidbodyConstraints2D.None;
        }
    }


    public void TakeDamage(float amout)
    {
        playerCurrentHealth -= amout;

        if (playerCurrentHealth <= 0)
        {
            isDead = true;
        }

        if (isDead)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

            playerAnimator.SetTrigger("IsDead");
        }
    }

    public void TakeHealth(float amount)
    {
        playerCurrentHealth += amount;

        if (playerCurrentHealth > playerMaxHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }
    }

    public void TakePoints(float amount)
    {
        playerCurrentPoints += amount;
    }

    private void Attack()
    {
        if (isAttacking)
        {
            playerAnimator.SetInteger("AttackNumber", attackNumber);

            if (attackNumber >= 5)
            {
                attackNumber = 1;
            }
        }
    }

    private void GetInput()
    {
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //rb2d.velocity = dirInput.normalized * walkSpeed;

        if (dirInput != Vector2.zero)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
        }

        if (dirInput.x != 0 && !isDead)
        {
            right = dirInput.x > 0;
            graphics.transform.rotation = right ? Quaternion.identity : Quaternion.Euler(0, 180f, 0);
        }

        sprintInput = Input.GetButton("Sprint");
        playerAnimator.SetBool("IsSprinting", sprintInput);

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            playerAnimator.SetTrigger("IsJumping");
        }

        if (Input.GetButtonDown("Attack"))
        {
            attackNumber += 1;
            isAttacking = true;
            playerAnimator.SetTrigger("Attack");
        }
    }


    void OnStateEnter()
    {
        switch (currentState)
        {
            case PlayerState.IDLE_Player:
                break;

            case PlayerState.WALK_Player:
                rb2d.velocity = dirInput.normalized * walkSpeed;
                break;

            case PlayerState.SPRINT_Player:
                rb2d.velocity = dirInput.normalized * sprintSpeed;
                break;
            case PlayerState.ATTACK_Player:
                break;
            case PlayerState.DEATH_Player:
                isDead = true;
                break;
            case PlayerState.JUMP_Player:
                isJumping = true;
                break;

            default:
                break;
        }
    }
    void OnStateUpdate()
    {
        switch (currentState)
        {
            case PlayerState.IDLE_Player:
                //TO WALK
                if (dirInput != Vector2.zero && !sprintInput)
                {
                    TransitionToState(PlayerState.WALK_Player);
                }

                //TO SPRINT
                if (dirInput != Vector2.zero && sprintInput)
                {
                    TransitionToState(PlayerState.SPRINT_Player);
                }

                //TO ATTACK
                if (isAttacking)
                {
                    TransitionToState(PlayerState.ATTACK_Player);
                }

                //TO JUMP
                if (isJumping)
                {

                    TransitionToState(PlayerState.JUMP_Player);
                }
                //TO DEATH
                if (isDead)
                {
                    TransitionToState(PlayerState.DEATH_Player);
                }
                break;


            case PlayerState.WALK_Player:
                rb2d.velocity = dirInput.normalized * walkSpeed;
                //TO IDLE
                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE_Player);
                }

                //TO SPRINT
                if (sprintInput)
                {
                    TransitionToState(PlayerState.SPRINT_Player);
                }

                //TO ATTACK
                if (isAttacking)
                {
                    TransitionToState(PlayerState.ATTACK_Player);
                }

                //TO JUMP
                if (isJumping)
                {
                    TransitionToState(PlayerState.JUMP_Player);
                }

                //TO DEATH
                if (isDead)
                {

                    TransitionToState(PlayerState.DEATH_Player);
                }
                break;

            case PlayerState.SPRINT_Player:
                rb2d.velocity = dirInput.normalized * sprintSpeed;
                //TO IDLE
                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE_Player);
                }

                //TO WALK
                if (!sprintInput)
                {
                    TransitionToState(PlayerState.WALK_Player);
                }

                //TO JUMP
                if (isJumping)
                {

                    TransitionToState(PlayerState.JUMP_Player);
                }

                //TO DEATH
                if (isDead)
                {
                    TransitionToState(PlayerState.DEATH_Player);
                }
                break;

            case PlayerState.ATTACK_Player:
                rb2d.velocity = dirInput.normalized * walkSpeed*.5f;
                //TO IDLE
                if (dirInput == Vector2.zero)
                {
                    TransitionToState(PlayerState.IDLE_Player);
                }
                break;

            case PlayerState.DEATH_Player:
                isJumping = false;
                break;

            case PlayerState.JUMP_Player:
                if (sprintInput)
                {
                    rb2d.velocity = dirInput.normalized * sprintSpeed;
                }
                else
                {
                    rb2d.velocity = dirInput.normalized * walkSpeed;
                }
                //TO IDLE
                if (!isJumping)
                {
                    TransitionToState(PlayerState.IDLE_Player);
                }

                //TO WALK
                if (!isJumping)
                {
                    TransitionToState(PlayerState.WALK_Player);
                }

                //TO SPRINT
                if (!isJumping)
                {
                    TransitionToState(PlayerState.SPRINT_Player);
                }
                break;
            default:
                break;
        }

    }
    void OnStateExit()
    {
        switch (currentState)
        {
            case PlayerState.IDLE_Player:
                break;
            case PlayerState.WALK_Player:
                break;
            case PlayerState.SPRINT_Player:
                break;
            case PlayerState.ATTACK_Player:
                isAttacking = false;
                break;
            case PlayerState.DEATH_Player:
                break;
            case PlayerState.JUMP_Player:
                isJumping = false;
                break;
            default:
                break;
        }
    }
    void TransitionToState(PlayerState nextState)
    {
        OnStateExit();
        currentState = nextState;
        OnStateEnter();
    }
}
