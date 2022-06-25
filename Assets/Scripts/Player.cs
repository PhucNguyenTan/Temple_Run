using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State machine
    public Player_state_machine pStateMachine { get; private set; }
    public Player_state_swipeLeft stateSwipeL { get; private set; }
    public Player_state_swipeRight stateSwipeR { get; private set; }
    public Player_state_noSwipe stateNoSwipe { get; private set; }
    public Player_state_dealth stateDeath { get; private set; }
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

    public bool isPause { get; private set; } = true;
    #endregion

    #region Health and score
    public float health { get; private set; }
    public int score { get; private set; } = 0;
    #endregion

    [SerializeField]
    private IGMUI ingameUI;

    private bool redDare_touched = false;

    private void Awake()
    {
        pStateMachine = new Player_state_machine();
        stateSwipeL = new Player_state_swipeLeft(this, pStateMachine, data, "left");
        stateSwipeR = new Player_state_swipeRight(this, pStateMachine, data, "right");
        stateNoSwipe = new Player_state_noSwipe(this, pStateMachine, data, "neutral");
        stateDeath = new Player_state_dealth(this, pStateMachine, data, "death"); 

        control = GetComponent<CharacterController>();
        boxCollider = GetComponent<BoxCollider>();
        controlInput = GetComponent<InputHandler>();
        gravity = data.gravity;

        GameManager.OnStateChange += GameManagerOnStateChanged;


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
    }

    void Update()
    {
        if (!isPause)
        {
            pStateMachine.currentState.LogicUpdate();
            MoveForward();
            SetJumpVar();
        }
    }


    #region Set functions
      
    public void MoveForward()
    {
        /*
        h_current = Mathf.Lerp(h_current, currentLane, data.SwipeSpeed * Time.deltaTime);
        Vector3 move = new Vector3();
        
        move.y += gravity * Time.deltaTime;
        v_current = move.y;
        move.x = h_current - transform.position.x;
        move.z = data.forwardSpeed * Time.deltaTime;
        //control.Move(move);
        Debug.Log("h_current :" + h_current + " Lane: "+currentLane);
        */

        h_current = Mathf.MoveTowards(h_current, currentLane, data.SwipeSpeed*Time.deltaTime);
        //Debug.Log(h_current);
        Vector3 move = new Vector3();

        move.y += gravity * Time.deltaTime;
        v_current = move.y;
        move.x = h_current - transform.position.x;
        move.z = 0.0f;//data.forwardSpeed * Time.deltaTime;
        control.Move(move);


        //h_current = Mathf.MoveTowards(h_current, currentLane, data.SwipeSpeed * Time.deltaTime);


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
        currentLane = data.laneMid;
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
}
