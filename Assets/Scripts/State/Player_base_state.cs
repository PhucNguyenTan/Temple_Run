using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_base_state
{
    protected Player player;
    protected Player_state_machine stateMachine;
    protected float startTime;
    protected string animName;
    protected Player_data data;
    protected bool startSwipe = false;

    public Player_base_state(Player player, Player_state_machine state_Machine, Player_data data, string animName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.data = data;
        this.animName = animName;
    }

    public virtual void Enter()
    {
        Debug.Log(animName);
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {
        Debug.Log(animName);
        if (player.control.isGrounded && player.controlInput.pressUp)
        {
            player.controlInput.UsedUpInput();
            player.AddJumpForce();
        }
    }

    public virtual void PhysicsUpdate()
    {

    }
}
