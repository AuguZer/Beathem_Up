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

    bool right = true;
    float playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentState = PlayerState.IDLE_Player;
        playerHealth = GetComponent<PlayerHealth>().playerCurrentHealth;
        OnStateEnter();

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        OnStateUpdate();
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

        if (dirInput.x != 0)
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
                //playerAnimator.SetBool("IsDead", true);
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
                if (playerHealth <= 0)
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
                if (playerHealth <= 0)
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
                if (playerHealth <= 0)
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
