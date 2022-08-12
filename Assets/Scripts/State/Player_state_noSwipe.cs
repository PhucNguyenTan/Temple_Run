using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_noSwipe : Player_base_state
{
    public Player_state_noSwipe(Player player, Player_state_machine state_Machine, Player_data data, string animName) : base(player, state_Machine, data, animName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(player.controlInput.pressLeft && player.controlInput.pressRight)
        {
            player.controlInput.UsedLeftInput();
            player.controlInput.UsedRightInput();
            return;
        }
        if (player.controlInput.pressLeft)
        {
            player.controlInput.UsedLeftInput();
            if(player.currentLane != data.laneLeft)
                player.pStateMachine.ChangeState(player.stateSwipeL);
        }
        if (player.controlInput.pressRight)
        {
            player.controlInput.UsedRightInput();
            if (player.currentLane != data.laneRight)
                player.pStateMachine.ChangeState(player.stateSwipeR);
        }
        if (player.controlInput.pressUp)
        {
            player.controlInput.UsedUpInput();
            player.pStateMachine.ChangeState(player.stateJump);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
