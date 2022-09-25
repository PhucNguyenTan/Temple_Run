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
        player.UnlockSwipe();
        player.ApplyGravity();
    }

    public override void Exit()
    {
        base.Exit();
        player.UnApplyGravity();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        bool isSwiping = player.CheckSwiping();
        bool isFalloff = player.CheckFallOff();
        if(player.CoyoteTimeCounter > 0f)
        {
             player.UnlockJump();
        }
        else
        {
            player.LockJump();
            player.ApplyGravity();
        }
        if (isFalloff)
        {
            SoundManager.Instance.PlayEffectRandomOnce(data.FallOffAudio);
            player.TakeDamage(player.Health);
        }
        if (player.IsGrounded)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
