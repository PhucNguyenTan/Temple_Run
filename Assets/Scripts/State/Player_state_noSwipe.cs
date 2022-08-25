using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_noSwipe : Player_base_state
{
    public Player_state_noSwipe(Player player, Player_state_machine stateMachine, Player_data data, string animName) : base(player, stateMachine, data, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InputHandler.Instance.Input.Player.Up.performed += player.PlayerJump;
        InputHandler.Instance.Input.Player.Left.performed += player.PlayerMoveLeft;
        InputHandler.Instance.Input.Player.Right.performed += player.PlayerMoveRight;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.IsSwiping())
        {
            stateMachine.ChangeState(player.stateSwiping);
        }
        if (!player.IsGrounded())
        {
            stateMachine.ChangeState(player.stateJump);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
