using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_state_swipeLeft : Player_base_state
{
    Vector3 startSwipe;
    Vector3 endSwipe;
    float time;


    public Player_state_swipeLeft(Player player, Player_state_machine state_Machine, Player_data data, string animName) : base(player, state_Machine, data, animName)
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
        player.Swipe(-1.0f);
        if (player.DoneSwiping(-1.0f))
            player.pStateMachine.ChangeState(player.noSwipeState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
