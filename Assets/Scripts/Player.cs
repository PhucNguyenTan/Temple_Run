using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region State machine
    public Player_state_machine pStateMachine { get; private set; }
    public Player_state_swipeLeft stateSwipeL { get; private set; }
    public Player_state_swipeRight stateSwipeR { get; private set; }
    public Player_state_noSwipe stateNoSwipe { get; private set; }
    public Player_state_dealth stateDeath { get; private set; }
    public Player_state_jump stateJump { get; private set; }
    public Player_state_swipe stateSwiping { get; private set; }
    [SerializeField]
    private Player_data data;
    #endregion

    #region Components
    public InputHandler controlInput { get; private set; }
    private BoxCollider boxCollider;
    public CharacterController control { get; private set; }
    #endregion

    #region undefined variables

    float _h_current = 0f;
    float _v_current = 0f;
    float _initialJumpVelocity;
    float _gravity;
    Vector3 _bottomContact;

    public float CurrentLane { get; private set; }
    public float PrevLane { get; private set; }
    public bool isPause { get; private set; } = true;
    #endregion
    #region Health and score
    public float health { get; private set; }
    public int score { get; private set; } = 0;
    public bool CanCheckGrounded { get; private set; }
    public bool IsApplyGravity { get; private set; }
    #endregion


    [SerializeField] float _timeToSwipe;
    [SerializeField] IGMUI ingameUI;
    public float StartSwipePoint { get; private set; }
    public float SwipeTimer;


    private bool redDare_touched = false;
    float _y_StableGroudn;

    float _v_force;
    private void Awake()
    {
        pStateMachine = new Player_state_machine();
        stateSwipeL = new Player_state_swipeLeft(this, pStateMachine, data, "left");
        stateSwipeR = new Player_state_swipeRight(this, pStateMachine, data, "right");
        stateNoSwipe = new Player_state_noSwipe(this, pStateMachine, data, "neutral");
        stateDeath = new Player_state_dealth(this, pStateMachine, data, "death");
        stateJump = new Player_state_jump(this, pStateMachine, data, "jump");
        stateSwiping = new Player_state_swipe(this, pStateMachine, data, "swipe");

        control = GetComponent<CharacterController>();
        boxCollider = GetComponent<BoxCollider>();
        controlInput = GetComponent<InputHandler>();
        CanCheckGrounded = true;
        SetJumpVar();

    }


    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameManager.GameState state)
    {
        switch (state)  
        {
            case GameManager.GameState.CountDown:
                InitializePlayer();
                isPause = true;
                break;
            case GameManager.GameState.Run:
                isPause = false;
                pStateMachine.Initialize(stateNoSwipe);
                break;
            case GameManager.GameState.Pause:
                isPause = true;
                break;
            case GameManager.GameState.End:
                isPause = true;
                break;
            default:
                throw new Exception("Something wrong, Patrick?");

        }
        //throw new NotImplementedException();
    }

    void Start()
    {
        pStateMachine.Initialize(stateNoSwipe);
        InputHandler.Instance.Input.Player.Left.performed += PlayerMoveLeft;
        InputHandler.Instance.Input.Player.Right.performed += PlayerMoveRight;
        InputHandler.Instance.Input.Player.Up.performed += PlayerJump;
        InputHandler.Instance.Input.Player.Down.performed += PlayerRoll;
        GameManager.OnStateChange += GameManagerOnStateChanged;
    }

    void Update()
    {
        if (!isPause)
        {
            pStateMachine.currentState.LogicUpdate();
        }
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        InputHandler.Instance.Input.Player.Left.performed -= PlayerMoveLeft;
        InputHandler.Instance.Input.Player.Right.performed -= PlayerMoveRight;
        InputHandler.Instance.Input.Player.Up.performed -= PlayerJump;
        InputHandler.Instance.Input.Player.Down.performed -= PlayerRoll;
        GameManager.OnStateChange -= GameManagerOnStateChanged;
    }

    #region Set functions

    //public void SetGrounded()
    //{
    //    _v_current = data.GroudY;
    //}
    public void MoveToTarget()
    {
        Vector3 move = new Vector3();
        move.y = _v_current;
        move.x = _h_current;
        transform.position = move;
    }

    public bool IsGrounded()
    {
        if (!CanCheckGrounded) return false;
        RaycastHit hit;
        Vector3 test = -transform.up * data.GroundDectectHeight;
        bool castTouch = Physics.Raycast(transform.position, -transform.up, out hit, 10 ,data.Standable);

        if (!castTouch)
            return false;

        _y_StableGroudn = hit.point.y + data.GroundDectectHeight;
        if (hit.distance <= data.GroundDectectHeight)
        {
            return true;
        }
        return false;
    }

    public void Swiping()
    {
        SwipeTimer += data.SwipeSpeed * Time.deltaTime;
        float timerRatio = SwipeTimer / data.SwipeDuration;
        if (timerRatio > 1f)
            timerRatio = 1f;
        _h_current = Mathf.Lerp(PrevLane, CurrentLane, timerRatio);
    }
    public bool IsSwiping()
    {
        if (transform.position.x != CurrentLane)
            return true;
        return false;
    }


    public void EndSwipe()
    {
        SwipeTimer = 0f;
    }

    public void ApplyGravity() { IsApplyGravity = true; }
    public void UnapplyGravity() { IsApplyGravity = false; }


    public void AddGravity()
    {
        _v_force += _gravity  * Time.deltaTime;
        _v_force = Mathf.Max(data.Gravity, _v_force);
        _v_current = transform.position.y + _v_force;
        if (_v_current < _y_StableGroudn)
        {
            _v_current = _y_StableGroudn;
            UnapplyGravity();
        }
    }

    public void AddJumpForce()
    {
        _v_force = _initialJumpVelocity;
    }
    #endregion

    public void SetJumpVar()
    {
        float timeToApex = data.maxJumpTime / 2;
        _gravity = (-2 * data.maxJumpHeight) / (timeToApex*timeToApex);
        _initialJumpVelocity = (2 * data.maxJumpHeight) / timeToApex;
    }

    

    #region Subscriber functions
    public void PlayerRoll(InputAction.CallbackContext obj)
    {

    }

    public void PlayerJump(InputAction.CallbackContext obj)
    {
        waitBeforeCheckGround(0.1f);
        SetJumpVar();
        AddJumpForce();
    }

    public void PlayerMoveLeft(InputAction.CallbackContext obj)
    {
        PrevLane = CurrentLane;
        if (CurrentLane == data.laneMid)
            CurrentLane = data.laneLeft;
        else if (CurrentLane == data.laneRight)
            CurrentLane = data.laneMid;
    }

    public void PlayerMoveRight(InputAction.CallbackContext obj)
    {
        PrevLane = CurrentLane;
        if (CurrentLane == data.laneMid)
            CurrentLane = data.laneRight;
        else if (CurrentLane == data.laneLeft)
            CurrentLane = data.laneMid;
    }


    #endregion

    #region Check functions
    public bool CheckIfDoneSwiping()
    {
        return _h_current == CurrentLane;
    }
    #endregion

    public void TakeDamage()
    {
        health -= 10f;
        ingameUI.SetHealthValue(health);
        if(health <= 0)
        {
            pStateMachine.ChangeState(stateDeath);
        }
    }

    public void InitializePlayer()
    {
        health = data.health;
        ingameUI.SetHealthValue(health);
        //pStateMachine.ChangeState(stateNoSwipe); Should probably check why this doesn't work
        transform.position = new Vector3(0f, 0.432f, 0f);
        CurrentLane = data.laneMid;
    }

    #region Other Object collision
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("redbox"))
        {
            Destroy(other.gameObject);
            TakeDamage();
        }

        else if (other.CompareTag("redbox_dare"))
        {
            
        }


    }
    #endregion

    private IEnumerator Wait(float timeToWait)
    {
        CanCheckGrounded = false;
        yield return new WaitForSeconds(timeToWait);
        CanCheckGrounded = true;
    }

    public void waitBeforeCheckGround(float timeToWait)
    {
        StartCoroutine(Wait(timeToWait));
    }

}
