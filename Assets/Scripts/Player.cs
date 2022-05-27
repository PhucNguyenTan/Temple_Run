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

    private float h_current = 0f;
    private float v_current = 0f;
    private float initialJumpVelocity = 0f;
    private float gravity;
    public float currentLane { get; private set; }
    public float prevLane { get; private set; }
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
        MoveForward();
        SetJumpVar();
    }


    #region Set functions
      
    public void MoveForward()
    {
        

        h_current = Mathf.Lerp(h_current, currentLane, data.SwipeSpeed * Time.deltaTime);
        Vector3 move = new Vector3();

        
        move.y += gravity * Time.deltaTime;
        v_current = move.y;
        move.x = h_current - transform.position.x;
        move.z = data.forwardSpeed * Time.deltaTime;
        control.Move(move);
        Debug.Log("h_current :" + h_current + " Lane: "+currentLane);
    }

    public void AddJumpForce()
    {
        v_current = initialJumpVelocity;
    }

    public void SetJumpVar()
    {
        float timeToApex = data.maxJumpTime / 2;
        gravity = (-2 * data.maxJumpHeight) / timeToApex*timeToApex;
        initialJumpVelocity = (2 * data.maxJumpHeight) / timeToApex;
    }

    public void SetLandLeft() {
        currentLane = data.laneLeft; 
    }
    public void SetLandRight()
    {
        currentLane = data.laneRight; 
    }
    public void SetLandMid()
    {
        currentLane = data.laneMid; 
    }
    #endregion

    #region Check functions
    public bool DoneSwiping()
    {
        return h_current == currentLane;
    }
    #endregion

}
