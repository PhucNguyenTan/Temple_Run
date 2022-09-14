using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
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
    public Player_state_airSwipe stateAirSwiping { get; private set; }
    [SerializeField] Player_data data;
    #endregion

    #region Components
    BoxCollider boxCollider;
    public CharacterController control { get; private set; }
    #endregion

    #region undefined variables

    float _h_current = 0f;
    float _v_current = 0f;
    float _initialJumpVelocity;
    float _gravity;
    float _vForceApply;
    float _y_StableGround;
    float _v_force = 0f;
    float _v_prevForce = 0f;
    Vector3 _bottomContact;

    public float CurrentLane { get; private set; }
    public float PrevLane { get; private set; }
    public bool isPause { get; private set; } = true; 
    #endregion
    #region Health and score
    public float Health { get; private set; }
    public int score { get; private set; } = 0;
    public bool CanCheckGrounded { get; private set; }
    public bool IsApplyGravity { get; private set; }
    #endregion

    bool _canJump;
    bool _canSwipe;

    [SerializeField] float _timeToSwipe;
    [SerializeField] IGMUI ingameUI;
    public float StartSwipePoint { get; private set; }
    public float SwipeTimer;

    #region Events
    public UnityAction OnObstacleCollided;
    public UnityAction OnBigObstacleCollided;
    public UnityAction OnPickupCoin;
    public UnityAction OnPickupHealth;
    #endregion

    private bool redDare_touched = false;

    #region Buffering variable
    public float CoyoteTimeCounter { get; private set; }
    #endregion

    private void Awake()
    {
        pStateMachine = new Player_state_machine();
        stateNoSwipe = new Player_state_noSwipe(this, pStateMachine, data, "neutral");
        stateDeath = new Player_state_dealth(this, pStateMachine, data, "death");
        stateJump = new Player_state_jump(this, pStateMachine, data, "jump");
        stateSwiping = new Player_state_swipe(this, pStateMachine, data, "swipe");
        stateAirSwiping = new Player_state_airSwipe(this, pStateMachine, data, "airSwipe");

        control = GetComponent<CharacterController>();
        boxCollider = GetComponent<BoxCollider>();
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
        SubLeftRight();
        SubDown();
        SubUp();
        GameManager.OnStateChange   += GameManagerOnStateChanged;
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
        UnSubLeftRight();
        UnSubDown();
        UnSubUp();
        GameManager.OnStateChange   -= GameManagerOnStateChanged;
    }

    #region Sub wrapper
    public void UnSubLeftRight()
    {
        InputHandler.Instance.Left.RemoveListener(PlayerMoveLeft);
        InputHandler.Instance.Right.RemoveListener(PlayerMoveRight);
    }

    public void UnSubUp()
    {
        InputHandler.Instance.Up.RemoveListener(PlayerJump);
    }

    public void UnSubDown()
    {
        InputHandler.Instance.Down.RemoveListener(PlayerRoll);
    }

    public void SubLeftRight()
    {
        InputHandler.Instance.Left.AddListener(PlayerMoveLeft);
        InputHandler.Instance.Right.AddListener(PlayerMoveRight);
    }

    public void SubUp()
    {
        //InputHandler.Instance._input.Player.Up.performed += PlayerJumptest;
        InputHandler.Instance.Up.AddListener(PlayerJump);

    }

    public void SubDown()
    {
        InputHandler.Instance.Down.AddListener(PlayerRoll);
    }
    #endregion

    #region Set functions
    public void LockJump() { _canJump = false; }
    public void LockSwipe() { _canSwipe = false; }
    public void UnlockJump() { _canJump = true; }
    public void UnlockSwipe() { _canSwipe = true; }
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
        if (!CanCheckGrounded)
        {
            return false;
        }
        RaycastHit hit;
        Vector3 test = -transform.up * data.GroundDectectHeight;
        bool isGroundBelow = Physics.Raycast(transform.position, -transform.up, out hit, 10 ,data.Standable);

        if (!isGroundBelow)
        {
            _y_StableGround = -10f;
            CoyoteTimeCounter -= Time.deltaTime;
            return false;
        }

        _y_StableGround = hit.point.y + data.GroundDectectHeight;
        if (hit.distance <= data.GroundDectectHeight)
        {
            CoyoteTimeCounter = data.CoyoteTime;
            return true;
        }
        CoyoteTimeCounter -= Time.deltaTime;
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
    public void UnApplyGravity() { IsApplyGravity = false; }


    public void AddGravity()
    {
        if (!IsApplyGravity) return;
        _v_prevForce = _v_force;
        _v_force += _gravity * Time.deltaTime;
        _vForceApply = Mathf.Max((_v_prevForce + _v_force) * .5f, data.Gravity);
        _v_current = transform.position.y + (_vForceApply * Time.deltaTime);

        if (_v_current < _y_StableGround)
        {
            _v_current = _y_StableGround;
            UnApplyGravity();
        }
    }

    public void AddJumpForce()
    {
        _v_force = _initialJumpVelocity;
         _vForceApply = _initialJumpVelocity;
    }
    #endregion

    public void SetJumpVar()
    {
        float timeToApex = data.maxJumpTime * .5f;
        _gravity = (-2 * data.maxJumpHeight) / (timeToApex * timeToApex);
        _initialJumpVelocity = 2 * data.maxJumpHeight / timeToApex;
    }


    #region Subscriber =======
    public void PlayerRoll()
    {

    }

    public void PlayerJump()
    {
        if (!_canJump) return;
        waitBeforeCheckGround(0.1f);
        CoyoteTimeCounter = 0f;
        SoundManager.Instance.PlayEffectRandomOnce(data.JumpAudio);
        SetJumpVar();
        AddJumpForce();
    }


    public void PlayerMoveLeft()
    {
        if (!_canSwipe) return;
        if (CurrentLane == data.laneLeft) return;

        SoundManager.Instance.PlayEffectRandomOnce(data.SwipeAudio);
        PrevLane = CurrentLane;
        if (CurrentLane == data.laneMid)
        {
            CurrentLane = data.laneLeft;
            return;
        }
        CurrentLane = data.laneMid;
    }

    public void PlayerMoveRight()
    {
        if (!_canSwipe) return;
        if (CurrentLane == data.laneRight) return;

        SoundManager.Instance.PlayEffectRandomOnce(data.SwipeAudio);
        PrevLane = CurrentLane;
        if (CurrentLane == data.laneMid)
        {
            CurrentLane = data.laneRight;
            return;
        }

        CurrentLane = data.laneMid;
    }


    #endregion

    #region Check functions
    public bool CheckIfDoneSwiping()
    {
        return _h_current == CurrentLane;
    }
    #endregion

    public void TakeDamage(float damage)
    {
        Health -= damage;
        ingameUI.SetHealthValue(Health);
        if(Health <= 0)
        {
            pStateMachine.ChangeState(stateDeath);
        }
    }

    public void AddHealth(float health)
    {
        Health = Mathf.Min(Health + health, data.MaxHealth);
        ingameUI.SetHealthValue(Health);
    }

    public void InitializePlayer()
    {
        Health = data.MaxHealth;
        ingameUI.SetHealthValue(Health);
        //pStateMachine.ChangeState(stateNoSwipe); Should probably check why this doesn't work
        transform.position = new Vector3(0f, 0.432f, 0f);
        CurrentLane = data.laneMid;
    }

    #region Other Object collision
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("redbox"))
        {
            OnObstacleCollided?.Invoke();
            Destroy(other.gameObject);
            SoundManager.Instance.PlayEffectRandomOnce(data.CollidingAudio);
            TakeDamage(10f);
        }

        else if (other.CompareTag("bigRedbox"))
        {
            Destroy(other.gameObject);
            OnBigObstacleCollided?.Invoke();
            SoundManager.Instance.PlayEffectRandomOnce(data.CollidingAudio);
            TakeDamage(20f);
        }
        else if (other.CompareTag("Health"))
        {
            Destroy(other.gameObject);
            AddHealth(10f);
            OnPickupHealth?.Invoke();

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


    public bool IsFallOff()
    {
        if(transform.position.y < 0)
        {
            return true;
        }
        return false;
    }

}
