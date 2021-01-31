using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Weapon weapon;
    
    public Weapon CollectWeapon()
    {
        Destroy(gameObject);
        return weapon;
    }
}
