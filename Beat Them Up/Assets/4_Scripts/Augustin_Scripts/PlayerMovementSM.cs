using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //POWER
    [SerializeField] public float playerMaxPower = 100f;
    [SerializeField] public float playerCurrentPower = 100f;


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
    [SerializeField] float attackSpeed = 2.5f;
    int attackNumber = 0;
    bool isAttacking;
    bool isResetting;

    //HOLD
 
    bool _canBeHold;
    [SerializeField] GameObject _pickUpPrefab;
    [SerializeField] int holdCount;
    bool throwed;
    //UI
    [SerializeField] GameObject healthSlider;
    [SerializeField] GameObject pwSlider;
    Slider lifeSlider;
    Slider powerSlider;

    private void Awake()
    {
        _graphics = transform.Find("GRAPHICS");
    }
    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        playerCurrentPower = 0f;
        playerCurrentPoints = 0f;
        rb2d = GetComponent<Rigidbody2D>();
        currentState = PlayerState.IDLE_Player;
        OnStateEnter();

        holdCount = 0;

        //UI
        //---- LIFE BAR ----
        lifeSlider = healthSlider.GetComponent<Slider>();
        lifeSlider.maxValue = playerMaxHealth;
        lifeSlider.value = playerCurrentHealth;

        //---- POWER BAR ----
        powerSlider = pwSlider.GetComponent<Slider>();
        powerSlider.maxValue = playerMaxPower;
        powerSlider.value = playerCurrentPower;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        OnStateUpdate();
        Jump();
        Hold();
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

        lifeSlider.value = playerCurrentHealth;

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

        lifeSlider.value = playerCurrentHealth;

        if (playerCurrentHealth > playerMaxHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }
    }

    public void TakePoints(float amount)
    {
        playerCurrentPoints += amount;
    }

    public void TakePower(float amount)
    {
        playerCurrentPower += amount;

        powerSlider.value = playerCurrentPower;

        if (playerCurrentPower > playerMaxPower)
        {
            playerCurrentPower = playerMaxPower;
        }

    }

    private void Attack()
    {
        if (isAttacking)
        {
            playerAnimator.SetInteger("AttackNumber", attackNumber);

            if (attackNumber == 4)
            {
                attackNumber = 0;
            }
        }

    }

    public void Hold()
    {
        if (_canBeHold & Input.GetButtonDown("Hold") & holdCount == 0)
        {
            holdCount++;
            playerAnimator.SetLayerWeight(1, 1f);

        }
        if (!_canBeHold & Input.GetButtonDown("Hold") & holdCount == 1)
        {
            throwed = true; 
            playerAnimator.SetTrigger("Throw");
            StartCoroutine(ThrowReset());

            holdCount = 0;
        }
    }

    IEnumerator ThrowReset()
    {
        yield return new WaitForSeconds(.5f);
        playerAnimator.SetLayerWeight(0, 1f);
        playerAnimator.SetLayerWeight(1, 0f);
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

        //SPRINT
        sprintInput = Input.GetButton("Sprint");
        playerAnimator.SetBool("IsSprinting", sprintInput);
        if (sprintInput && Input.GetButtonDown("Attack"))
        {
            isAttacking = false;
        }
        //ATTACK
        if (Input.GetButtonDown("Attack") && !sprintInput)
        {
            isAttacking = true;
            attackNumber += 1;
            playerAnimator.SetTrigger("Attack");
        }

        //JUMP
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            playerAnimator.SetTrigger("IsJumping");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Items")
        {
            _canBeHold = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _canBeHold = false;
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
                if (!isResetting)
                {
                    isResetting = true;
                    StartCoroutine(AttackReset());
                }
                StartCoroutine(AttackCD());

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
                rb2d.velocity = dirInput.normalized * attackSpeed;

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
                isAttacking = false;
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

    IEnumerator AttackReset()
    {
        float t = 0;
        float attackDuration = 1.5f;

        while (t < attackDuration)
        {
            t += Time.deltaTime;

            if (Input.GetButtonDown("Attack"))
            {
                t = 0;
            }

            yield return null;
        }

        attackNumber = 0;
        isResetting = false;

    }

    IEnumerator AttackCD()
    {
        yield return new WaitForSeconds(.3f);

        //TO IDLE
        if (dirInput == Vector2.zero)
        {
            TransitionToState(PlayerState.IDLE_Player);
        }

        //TO WALK
        if (dirInput != Vector2.zero)
        {
            TransitionToState(PlayerState.WALK_Player);
        }

        //TO SPRINT
        if (sprintInput)
        {
            TransitionToState(PlayerState.SPRINT_Player);
        }
    }
}
