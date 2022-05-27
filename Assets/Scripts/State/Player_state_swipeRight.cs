using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_swipeRight : Player_base_state
{
    public Player_state_swipeRight(Player player, Player_state_machine state_Machine, Player_data data, string animName) : base(player, state_Machine, data, animName)
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

        

        if (!startSwipe)
        {
            startSwipe = true;
            if (player.currentLane == data.laneLeft)
            {
                player.SetLandMid();
            }
            else if (player.currentLane == data.laneMid)
            {
                player.SetLandRight();
            }
        }

        if (player.DoneSwiping())
        {
            startSwipe = false;
            player.pStateMachine.ChangeState(player.noSwipeState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
