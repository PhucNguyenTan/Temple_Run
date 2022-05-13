using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State machine
    public Player_state_machine pStateMachine { get; private set; }
    public Player_state_swipeLeft swipeLState { get; private set; }
    public Player_state_swipeRight swipeRState { get; private set; }
    public Player_state_noSwipe noSwipeState { get; private set; }
    [SerializeField]
    private Player_data data;
    #endregion

    #region Components
    public InputHandler controlInput { get; private set; }
    private BoxCollider boxCollider;
    public CharacterController control { get; private set; }
    #endregion

    #region undefined variables
    private float _current;
    private Vector3 _currentPosition;

    private float h_current = 0f;
    private float v_current = 0f;
    private float f_current = 0f;
    private float initialJumpVelocity = 0f;
    private float gravity;
    #endregion

    #region Health and score
    private float health = 50f;
    private int score = 0;
    #endregion

    private void Awake()
    {
        pStateMachine = new Player_state_machine();
        swipeLState = new Player_state_swipeLeft(this, pStateMachine, data, "left");
        swipeRState = new Player_state_swipeRight(this, pStateMachine, data, "right");
        noSwipeState = new Player_state_noSwipe(this, pStateMachine, data, "neutral");

        control = GetComponent<CharacterController>();
        boxCollider = GetComponent<BoxCollider>();
        controlInput = GetComponent<InputHandler>();
        gravity = data.gravity;

        SetJumpVar();

    }

    void Start()
    {
        pStateMachine.Initialize(noSwipeState);
    }

    void Update()
    {
        pStateMachine.currentState.LogicUpdate();
    }

    /*public void SetDirection(int dir)
    {
        startSwipe = transform.position;
        endSwipe = transform.position + Vector3.left * dir * speed;
        isSwiping = true;
    }*/

    public void Swipe(float direction)
    {
        //transform.position = Vector3.Lerp(transform.position, transform.position + data._goalPosition*direction, _current);
        _current = Mathf.MoveTowards(_current, data.SwipeRange, data.SwipeSpeed * Time.fixedDeltaTime);
        Vector3 move = new Vector3(0f, v_current, 1f*Time.fixedDeltaTime) + data._goalPosition*direction*_current;
        move.y += data.gravity * Time.deltaTime;
        v_current = move.y;
        control.Move(move);

    }    

    public void MoveForward()
    {
        Vector3 move = new Vector3(0f, v_current, data.forwardSpeed);
        move.y += gravity * Time.deltaTime;
        v_current = move.y;
        control.Move(move* Time.fixedDeltaTime);
        Debug.Log(move.y);
    }

    public void AddJumpForce()
    {
        v_current = initialJumpVelocity;
    }

    public void SetJumpVar()
    {
        float timeToApex = data.maxJumpTime / 2;
        gravity = (-2 * data.maxJumpTime) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * data.maxJumpTime) / timeToApex;
    }

    #region Check functions
    public bool DoneSwiping(float direction)
    {
        if (_current == data.SwipeRange)
        {
            _current = 0f;
            return true;
        }
        return false;
    }
    public bool CheckLeftMost()
    {
        if (transform.position.x < data.laneLeft)
            return true;
        return false;
    }

    public bool CheckRightMost()
    {
        if (transform.position.x > data.laneRight)
            return true;
        return false;
    }
    #endregion
}
