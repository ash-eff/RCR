using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State<PlayerManager>
{
    public override void EnterState(PlayerManager player)
    { ;
    }

    public override void ExitState(PlayerManager player)
    {
    }

    public override void UpdateState(PlayerManager player)
    {
        Debug.Log("Idle State");
    }

    public override void FixedUpdateState(PlayerManager player)
    {
    }
}
