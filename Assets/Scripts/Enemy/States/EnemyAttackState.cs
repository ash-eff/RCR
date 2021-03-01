using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ash.MyUtils;
using Ash.StateMachine;
using UnityEngine;

public class EnemyAttackState : State<Enemy>
{
    public override void EnterState(Enemy enemy)
    {
    }

    public override void ExitState(Enemy enemy)
    {
    }

    public override void UpdateState(Enemy enemy)
    {
        // chase player until dead

    }

    public override void FixedUpdateState(Enemy enemy)
    {
    }
}
