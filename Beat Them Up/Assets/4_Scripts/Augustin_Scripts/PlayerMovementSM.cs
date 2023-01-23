using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        HURT_Player

    }

    [Header("MOVEMENT")]
    [SerializeField] PlayerState currentState;
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] GameObject psSprintR;
    [SerializeField] GameObject psSprintL;
    Vector2 dirInput;
    bool sprintInput;
    Rigidbody2D rb2d;
    bool right = true;

    [Header("LIFE")]
    [SerializeField] public float playerMaxHealth = 100f;
    [SerializeField] public float playerCurrentHealth;
    [SerializeField] public int playerMaxLife = 3;
    [SerializeField] public int playerCurrentLife;
    bool isHurt;
    bool isDead;
    bool invincible;

    [Header("POWER")]
    [SerializeField] public float playerMaxPower = 100f;
    [SerializeField] public float playerCurrentPower = 100f;


    [Header("JUMP")]
    [SerializeField] AnimationCurve jumpCurve;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpDuration = 3f;
    [SerializeField] GameObject psJump;
    [SerializeField] GameObject psLand;
    [SerializeField] Animator shadowAnimator;
    AudioSource audioPlayer;
    Transform _graphics;
    float jumpTimer;
    bool isJumping;

    [Header("POINTS")]
    [SerializeField] float playerCurrentPoints;

    [Header("ATTACK")]
    [SerializeField] float attackSpeed = 2.5f;
    [SerializeField] GameObject hitBox;
    int attackNumber = 0;
    bool isAttacking;
    bool isResetting;

    [Header("HOLD")]
    bool _canBeHold;
    bool isHolding;
    [SerializeField] GameObject _pickUpPrefab;
    [SerializeField] GameObject _pickUpGraphics;
    [SerializeField] int holdCount;
    [SerializeField] float throwSpeed = 5f;
    Rigidbody2D rb2dPickUp;
    SpriteRenderer _sprite;
    AudioSource audioThrow;

    [Header("UI")]
    [SerializeField] GameObject healthSlider;
    [SerializeField] GameObject pwSlider;
    [SerializeField] GameObject pointsText;
    [SerializeField] GameObject lifeText;
    Slider lifeSlider;
    Slider powerSlider;
    TextMeshProUGUI ptsText;
    TextMeshProUGUI lifeCount;



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
        _sprite = _pickUpGraphics.GetComponent<SpriteRenderer>();
        OnStateEnter();

        audioPlayer = GetComponent<AudioSource>();

        hitBox.SetActive(false);
        psJump.SetActive(false);
        psLand.SetActive(false);
        psSprintR.SetActive(false);
        psSprintL.SetActive(false);

        //HOLD
        rb2dPickUp = _pickUpPrefab.GetComponent<Rigidbody2D>();
        holdCount = 0;
        audioThrow = _pickUpPrefab.GetComponent<AudioSource>();


        //UI
        //---- LIFE BAR ----
        lifeSlider = healthSlider.GetComponent<Slider>();
        lifeSlider.maxValue = playerMaxHealth;
        lifeSlider.value = playerCurrentHealth;
        lifeCount = lifeText.GetComponent<TextMeshProUGUI>();
        playerCurrentLife = playerMaxLife;
        lifeCount.text = playerCurrentLife.ToString();
        //---- POWER BAR ----
        powerSlider = pwSlider.GetComponent<Slider>();
        powerSlider.maxValue = playerMaxPower;
        powerSlider.value = playerCurrentPower;

        //---- SCORE ----
        ptsText = pointsText.GetComponent<TextMeshProUGUI>();
        ptsText.text = "Score : 0000" + playerCurrentPoints.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        OnStateUpdate();
        Jump();
        Hold();

    }
    private void Jump()
    {

        if (isJumping && jumpTimer < jumpDuration)
        {
            jumpTimer += Time.deltaTime;
            float y = jumpCurve.Evaluate(jumpTimer / jumpDuration);

            _graphics.localPosition = new Vector3(_graphics.transform.localPosition.x, y * jumpHeight, _graphics.transform.localPosition.z);
            rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            psJump.SetActive(true);
            psLand.SetActive(false);

        }

        if (jumpTimer > jumpDuration)
        {
            isJumping = false;
            jumpTimer = 0f;
            rb2d.constraints = RigidbodyConstraints2D.None;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
            psJump.SetActive(false);
            psLand.SetActive(true);
        }

    }
    public void TakeDamage(float amout)
    {
        if (invincible)
        {
            return;
        }

        isHurt = true;


        playerCurrentHealth -= amout;


        if (lifeSlider != null)
        {
            lifeSlider.value = playerCurrentHealth;

            if (playerCurrentHealth <= 0)
            {
                isDead = true;
                playerAnimator.SetBool("Dead", true);
                playerCurrentLife -= 1;
                invincible = true;
            }

            if (isDead)
            {
                lifeCount.text = playerCurrentLife.ToString();
                rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

                playerAnimator.SetTrigger("IsDead");
            }
        }
    }
    public void TakeHealth(float amount)
    {

        playerCurrentHealth += amount;

        if (lifeSlider != null)
        {
            lifeSlider.value = playerCurrentHealth;

            if (playerCurrentHealth > playerMaxHealth)
            {
                playerCurrentHealth = playerMaxHealth;
            }
        }
    }
    public void TakePoints(float amount)
    {
        playerCurrentPoints += amount;
        ptsText.text = "Score : " + playerCurrentPoints.ToString();
    }
    public void TakePower(float amount)
    {
        playerCurrentPower += amount;

        if (powerSlider != null)
        {
            powerSlider.value = playerCurrentPower;

            if (playerCurrentPower > playerMaxPower)
            {
                playerCurrentPower = playerMaxPower;
            }
        }


    }
    private void Attack()
    {
        if (isHolding)
        {
            return;
        }

        isAttacking = true;
        attackNumber += 1;
        playerAnimator.SetInteger("AttackNumber", attackNumber);

        if (attackNumber == 4)
        {
            attackNumber = 0;
        }

        playerAnimator.SetTrigger("Attack");
    }
    public void Hold()
    {
        if (_canBeHold & Input.GetButtonDown("Hold") & holdCount == 0)
        {
            isHolding = true;
            rb2dPickUp.isKinematic = true;
            rb2dPickUp.constraints = RigidbodyConstraints2D.None;
            rb2dPickUp.constraints = RigidbodyConstraints2D.FreezeRotation;
            holdCount++;
            playerAnimator.SetLayerWeight(1, 1f);
            //_sprite.sortingOrder = 1;
        }
        if (!_canBeHold & Input.GetButtonDown("Hold") & holdCount == 1)
        {
            isHolding = false;
            rb2dPickUp.isKinematic = false;
            playerAnimator.SetTrigger("Throw");
            //_sprite.sortingOrder = 0;
            AudioSource.PlayClipAtPoint(audioThrow.clip, transform.position);
            StartCoroutine(ThrowTime());

            if (!right)
            {
                rb2dPickUp.AddForce(-transform.right * throwSpeed, ForceMode2D.Impulse);
            }
            if (right)
            {
                rb2dPickUp.AddForce(transform.right * throwSpeed, ForceMode2D.Impulse);
            }

            StartCoroutine(ThrowReset());

            holdCount = 0;
        }

        if (holdCount == 0)
        {
            _pickUpPrefab.transform.SetParent(null);
        }

        rb2dPickUp.gravityScale = rb2dPickUp.isKinematic ? rb2dPickUp.gravityScale = 0f : rb2dPickUp.gravityScale = 2f;

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
            Attack();
        }

        //JUMP
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            AudioSource.PlayClipAtPoint(audioPlayer.clip, transform.position);
            isJumping = true;
            playerAnimator.SetTrigger("IsJumping");
            shadowAnimator.SetTrigger("JUMP");

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Items")
        {
            _canBeHold = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Items")
        {
            _canBeHold = false;
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
                if (right)
                {
                    psSprintR.SetActive(true);
                }
                if (!right)
                {
                    psSprintL.SetActive(true);
                }
                break;
            case PlayerState.ATTACK_Player:
                hitBox.SetActive(true);
                if (!isResetting)
                {
                    isResetting = true;
                    StartCoroutine(AttackReset());
                }
                StartCoroutine(AttackCD());

                break;
            case PlayerState.DEATH_Player:
                isDead = true;
                invincible = true;
                break;
            case PlayerState.JUMP_Player:
                isJumping = true;
                isAttacking = false;
                break;
            case PlayerState.HURT_Player:
                invincible = true;
                isHurt = true;
                if (!isDead)
                {
                    StartCoroutine(Invincible());
                }
                playerAnimator.SetTrigger("IsHit");
                StartCoroutine(Hurt());
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

                //TO HURT
                if (isHurt)
                {
                    TransitionToState(PlayerState.HURT_Player);
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

                //TO HURT
                if (isHurt)
                {
                    TransitionToState(PlayerState.HURT_Player);
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

                //TO HURT
                if (isHurt)
                {
                    TransitionToState(PlayerState.HURT_Player);
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

                //TO HURT
                if (!isJumping && isHurt)
                {
                    TransitionToState(PlayerState.HURT_Player);
                }
                break;

            case PlayerState.HURT_Player:
                rb2d.velocity = dirInput.normalized * attackSpeed;
                holdCount = 0;
                _pickUpPrefab.transform.SetParent(null);
                playerAnimator.SetLayerWeight(1, 0f);
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
                psSprintR.SetActive(false);
                psSprintL.SetActive(false);
                break;
            case PlayerState.ATTACK_Player:
                isAttacking = false;
                hitBox.SetActive(false);

                break;
            case PlayerState.DEATH_Player:
                break;
            case PlayerState.JUMP_Player:
                isJumping = false;
                break;
            case PlayerState.HURT_Player:
                isHurt = false;
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
    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(.3f);

        //TO IDLE
        if (dirInput == Vector2.zero)
        {
            TransitionToState(PlayerState.IDLE_Player);
        }
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
    }
    IEnumerator Invincible()
    {
        yield return new WaitForSeconds(2f);
        invincible = false;
    }
    IEnumerator ThrowReset()
    {
        yield return new WaitForSeconds(.3f);
        playerAnimator.SetLayerWeight(0, 1f);
        playerAnimator.SetLayerWeight(1, 0f);
    }
    IEnumerator ThrowTime()
    {
        yield return new WaitForSeconds(.5f);

        rb2dPickUp.isKinematic = true;
        rb2dPickUp.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
