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
    private CharacterController control;
    #endregion

    private float _current;
    private Vector3 _currentPosition;

    private void Awake()
    {
        pStateMachine = new Player_state_machine();
        swipeLState = new Player_state_swipeLeft(this, pStateMachine, data, "left");
        swipeRState = new Player_state_swipeRight(this, pStateMachine, data, "right");
        noSwipeState = new Player_state_noSwipe(this, pStateMachine, data, "neutral");

        control = GetComponent<CharacterController>();
        boxCollider = GetComponent<BoxCollider>();
        controlInput = GetComponent<InputHandler>();

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
        _current = Mathf.MoveTowards(_current, data.SwipeRange, data.SwipeSpeed * Time.fixedDeltaTime);
        transform.position = Vector3.Lerp(transform.position, transform.position + data._goalPosition*direction, _current);

    }    

    public bool DoneSwiping(float direction)
    {
        if (_current == data.SwipeRange)
        {
            _current = 0f;
            return true;
        }
        return false;
    }

    public void MoveForward()
    {
        control.Move(Vector3.forward*Time.fixedDeltaTime*data.forwardSpeed);
    }

    public void Jump()
    {

    }
}
