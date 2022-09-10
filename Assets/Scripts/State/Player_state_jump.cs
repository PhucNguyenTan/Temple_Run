using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_jump : Player_base_state
{
    public Player_state_jump(Player player, Player_state_machine stateMachine, Player_data data, string animName) : base(player, stateMachine, data, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.ApplyGravity();
        player.LockJump();
        player.UnlockSwipe();
    }

    public override void Exit()
    {
        base.Exit();
        player.UnapplyGravity();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        bool isGrounded = player.IsGrounded();
        bool isSwiping = player.IsSwiping();
        bool isFalloff = player.IsFallOff();
        if (isFalloff)
        {
            player.TakeDamage(player.health);
        }
        if (isGrounded)
        {
            SoundManager.Instance.PlayEffectRandomOnce(data.LandAudio);
            if(isSwiping)
                stateMachine.ChangeState(player.stateSwiping);
            else
                stateMachine.ChangeState(player.stateNoSwipe);

        }
        else
        {
            if (isSwiping)
                stateMachine.ChangeState(player.stateAirSwiping); // Air Swipe
        }
        player.AddGravity();
        player.MoveToTarget();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
