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

    private float h_current = 0f;
    private float v_current = 0f;
    private float initialJumpVelocity = 0f;
    private float gravity;
    public float CurrentLane { get; private set; }
    public float PrevLane { get; private set; }

    public bool isPause { get; private set; } = true;
    #endregion

    #region Health and score
    public float health { get; private set; }
    public int score { get; private set; } = 0;
    #endregion


    [SerializeField] float _timeToSwipe;
    [SerializeField] IGMUI ingameUI;
    [SerializeField] float _offset;
    public float StartSwipePoint { get; private set; }
    public float SwipeTimer;

    private bool redDare_touched = false;

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
        gravity = data.gravity;

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
            PlayerUpdate();
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

    public void Jump()
    {
        Vector3 move = new Vector3();
        
        v_current -= data.gravity * Time.deltaTime;
        move.y = v_current;
        move.x = h_current;
    }

    void PlayerUpdate()
    {
        AddGravity();
        Vector3 move = new Vector3();
        move.y = 0f;
        move.x = h_current;
        MoveToTarget(move);
    }

    public void EndSwipe()
    {
        SwipeTimer = 0f;
    }

    void MoveToTarget(Vector3 target)
    {

        var test = target - transform.position;
        test.y = v_current;
        //Debug.Log(test);1
        control.Move(test);
    }

    public void Swiping()
    {
        SwipeTimer += data.SwipeSpeed * Time.deltaTime;
        float timerRatio = SwipeTimer / data.SwipeDuration;
        if (timerRatio > 1f)
            timerRatio = 1f;
        h_current = Mathf.Lerp(PrevLane, CurrentLane, timerRatio);
        //Debug.Log(h_current);
    }

    public bool CheckIsSwiping()
    {
        float test = Mathf.Abs(CurrentLane - transform.position.x);
        Debug.Log(test);
        if (test > _offset)
            return true;
        return false;
    }

    public void AddGravity()
    {
        v_current = v_current > data.gravity ? data.gravity : v_current + gravity;
    }

    public void AddJumpForce()
    {
        v_current = initialJumpVelocity;
    }
    #endregion

    public void SetJumpVar()
    {
        float timeToApex = data.maxJumpTime / 2;
        gravity = (-2 * data.maxJumpHeight) / timeToApex*timeToApex;
        initialJumpVelocity = (2 * data.maxJumpHeight) / timeToApex;
    }

    

    #region Subscriber functions
    public void PlayerRoll(InputAction.CallbackContext obj)
    {

    }

    public void PlayerJump(InputAction.CallbackContext obj)
    {
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
        return h_current == CurrentLane;
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

    private IEnumerator waitBeforeCheckGrounded(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if (control.isGrounded)
        {
            pStateMachine.ChangeState(stateNoSwipe);
        }
    }

    public void waitBeforeCheckGround(float timeToWait)
    {
        StartCoroutine(waitBeforeCheckGrounded(timeToWait));
    }

}
