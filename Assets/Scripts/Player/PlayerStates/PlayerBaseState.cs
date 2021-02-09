using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : State<PlayerManager>
{
    public override void EnterState(PlayerManager player)
    {
        Debug.Log("Player Base");
        player.playerWeaponManager.RevealWeapon();
    }

    public override void ExitState(PlayerManager player)
    {
    }

    public override void UpdateState(PlayerManager player)
    {
    }

    public override void FixedUpdateState(PlayerManager player)
    {
    }
}
