using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_airSwipe : Player_base_state
{
    public Player_state_airSwipe(Player player, Player_state_machine stateMachine, Player_data data, string animName) : base(player, stateMachine, data, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        InputHandler.Instance.Input.Player.Left.performed -= player.PlayerMoveLeft;
        InputHandler.Instance.Input.Player.Right.performed -= player.PlayerMoveRight;
        player.ApplyGravity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        bool isSwiping = player.IsSwiping();
        bool isGrounded = player.IsGrounded();

        if (isGrounded)
        {
            SoundManager.Instance.PlayEffectRandomOnce(data.LandAudio);
            if (isSwiping)
            {
                player.UnapplyGravity();
                stateMachine.ChangeState(player.stateSwiping);
            }
            else
            {
                player.EndSwipe();
                player.UnapplyGravity();
                stateMachine.ChangeState(player.stateNoSwipe);
            }
        }
        else
        {
            if (isSwiping)
            {
                player.AddGravity();
                player.MoveToTarget();
                player.Swiping();
            }
            else
            {
                player.EndSwipe();
                stateMachine.ChangeState(player.stateJump);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
