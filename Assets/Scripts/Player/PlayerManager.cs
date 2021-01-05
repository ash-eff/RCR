using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[SerializeField] private CharacterController2D controller;
	[SerializeField] private Animator anim;
	private PlayerInputs playerInputs;
	private PlayerCursor cursor;
	private Vector2 directionAxis;
	private float runSpeed = 40f;
	private float horizontalMove = 0f;

	[SerializeField] private GameObject playerSprite;

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
		cursor = GetComponent<PlayerCursor>();
		playerInputs = new PlayerInputs();
		playerInputs.Player.Move.performed += cxt => SetMovement(cxt.ReadValue<Vector2>());
		playerInputs.Player.Move.canceled += cxt => ResetMovement();
	}

	private void Update()
	{
		// use this for weapon
		//var mousePos = MyUtils.GetMouseWorldPosition();
		//var dir = mousePos - transform.position;
		//var rotation = MyUtils.GetAngleFromVector(dir);
		//weapon.transform.rotation = Quaternion.Euler(0,0,rot);
		controller.Flip(cursor.AimDirection.x);
		anim.SetFloat("xDir", Mathf.Abs(directionAxis.x));
	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(directionAxis * (runSpeed * Time.fixedDeltaTime), false, false, false);
	}
	
	private void SetMovement(Vector2 movement) => directionAxis = movement;
	
	private void ResetMovement() => directionAxis = Vector3.zero;
}
