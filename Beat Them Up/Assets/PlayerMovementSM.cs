using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementSM : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
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
    Vector2 dirInput;
    bool sprintInput;
    Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        currentState = PlayerState.IDLE_Player;
        OnStateEnter();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        OnStateUpdate();
    }

    private void Move()
    {
        dirInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb2d.velocity = dirInput.normalized * walkSpeed;

        if (dirInput != Vector2.zero)
        {
            playerAnimator.SetBool("IsWalking", true);
        }
        else
        {
            playerAnimator.SetBool("IsWalking", false);
        }

        if (Input.GetButton("Sprint")) 
        {
            playerAnimator.SetBool("IsSprinting", true);
        }
        else
        {
            playerAnimator.SetBool("IsSprinting", false);
        }
        /////// FIXER LE SPRINT 

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
                break;
            case PlayerState.DEATH_Player:
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
