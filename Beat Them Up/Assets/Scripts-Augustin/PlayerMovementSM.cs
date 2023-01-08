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
        ATTACK1_Player,
        DEATH_Player
    }

    [SerializeField] PlayerState currentState;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    Vector2 dirInput;
    bool sprintInput;
    Rigidbody2D rb2d;

    [SerializeField] public float playerMaxHealth = 100f;
    [SerializeField] public float playerCurrentHealth;

    bool right = true;
    bool isDead;


    //JUMP
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpDuration = 3f;
    Transform _graphics;
    float jumpTimer;
    CapsuleCollider2D cc2d;


    private void Jump()
    {
       if(Input.GetButton("Jump"))
        {
            if (jumpTimer < jumpDuration)
            {
                jumpTimer += Time.deltaTime;

                float y = jumpCurve.Evaluate(jumpTimer / jumpDuration);

                _graphics.position = new Vector3(transform.position.x, y * jumpHeight, transform.position.z);

            }
            else
            {
                jumpTimer = 0f;
            }
        }
        

    }

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        currentState = PlayerState.IDLE_Player;
        OnStateEnter();
        cc2d = GetComponent<CapsuleCollider2D>();

        _graphics = transform.Find("GRAPHICS");


    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        OnStateUpdate();
        Jump();


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

    }


    void OnStateEnter()
    {
        switch (currentState)
        {
            case PlayerState.IDLE_Player:
                break;

            case PlayerState.WALK_Player:
                break;

            case PlayerState.SPRINT_Player:
                break;
            case PlayerState.ATTACK1_Player:
                playerAnimator.SetTrigger("Attack");
                break;
            case PlayerState.DEATH_Player:
                isDead = true;
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
                if (Input.GetButtonDown("Attack"))
                {
                    TransitionToState(PlayerState.ATTACK1_Player);
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
                if (Input.GetButtonDown("Attack"))
                {
                    TransitionToState(PlayerState.ATTACK1_Player);
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

                //TO DEATH
                if (isDead)
                {
                    TransitionToState(PlayerState.DEATH_Player);
                }
                break;
            case PlayerState.ATTACK1_Player:

                TransitionToState(PlayerState.IDLE_Player);

                break;
            case PlayerState.DEATH_Player:

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
            case PlayerState.ATTACK1_Player:
                break;
            case PlayerState.DEATH_Player:
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
