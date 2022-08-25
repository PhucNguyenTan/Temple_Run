using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_jump : Player_base_state
{
    bool jumped = false;
    public Player_state_jump(Player player, Player_state_machine stateMachine, Player_data data, string animName) : base(player, stateMachine, data, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ApplyGravity();
        InputHandler.Instance.Input.Player.Up.performed -= player.PlayerJump;
    }

    public override void Exit()
    {
        base.Exit();
        player.UnapplyGravity();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.IsGrounded())
        {
            if(player.IsSwiping())
                stateMachine.ChangeState(player.stateSwiping);
            else
                stateMachine.ChangeState(player.stateNoSwipe);

        }
        else
        {
            if (player.IsSwiping())
                stateMachine.ChangeState(player.stateSwiping); // Air Swipe
        }
        player.AddGravity();
        player.MoveToTarget();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
