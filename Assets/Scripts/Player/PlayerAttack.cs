using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float rateOfFire;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private bool isShootingPresseed = false;
    [SerializeField] private GameObject reticle;

    private Vector2 aimDir;
    private float lastShot = 0;
    private PlayerInputs playerInputs;
    
    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    private void Awake()
    {
	    aimDir =  transform.right;
	    
	    playerInputs = new PlayerInputs();

        playerInputs.Player.Move.performed += cxt => SetAim(cxt.ReadValue<Vector2>());
    }

    private void Update()
    {
        if(isShootingPresseed)
            Shoot();
        
        //Debug.DrawRay(transform.position, aimDir * 10f, Color.red);
        SetReticle(aimDir);
    }
    
    private void Shoot()
    {
    	if (Time.time > rateOfFire + lastShot)
    	{
	        GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<Projectile>().dir = aimDir;

    		lastShot = Time.time;
    	}
    }

    private void SetReticle(Vector2 aim)
    {
        var pos = (Vector2)transform.position + aim * 4f;
        reticle.transform.position = pos;
    }

    private void SetAim(Vector2 aim)
    {
	    aimDir = aim;
    }
}
