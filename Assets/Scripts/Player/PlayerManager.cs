using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using Ash.StateMachine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public StateMachine<PlayerManager> stateMachine;
	[SerializeField] private float runSpeed = 40f;
	private PlayerAnimationController animController;
	private PlayerController playerController;
	private PlayerInput playerInput;
	private PlayerWeapon currentWeapon;

	[NonSerialized] public readonly PlayerBaseState playerBaseState = new PlayerBaseState();
	[NonSerialized] public readonly PlayerIdleState playerIdleState = new PlayerIdleState();
	

	private void Awake()
	{
		animController = GetComponent<PlayerAnimationController>();
		playerController = GetComponent<PlayerController>();
		playerInput = GetComponent<PlayerInput>();
		currentWeapon = GetComponent<PlayerWeapon>();
		stateMachine = new StateMachine<PlayerManager>(this);
		stateMachine.ChangeState(playerBaseState);
	}

	private void Update()
	{
		playerInput.AdjustCursorPosition();
		animController.SpriteFlip(playerInput.GetCursorDirection.x);
		animController.SetSpriteFacingDirection(playerInput.GetCursorDirection);
		animController.IsPlayerMoving(playerInput.GetDirectionAxis);
		stateMachine.Update();
	}
	
	void FixedUpdate ()
	{
		playerController.Move(playerInput.GetDirectionAxis * (runSpeed * Time.fixedDeltaTime));
		stateMachine.FixedUpdate();
	}

	//public void PlayerIsFalling(Vector3 atPosition)
	//{
	//	anim.SetBool(Pit, true);
	//	var foo = anim.GetBool(Pit);
	//	Debug.Log(foo + " Falling.");
	//	playerFallTrigger.SetActive(false);
	//	StartCoroutine(PlayerFall());
//
	//	IEnumerator PlayerFall()
	//	{
	//		weapon.HideWeapon(true);
	//		isFalling = true;
	//		//playerSprite.SetActive(false);
//
	//		yield return new WaitForSeconds(1f);
	//		
	//		transform.position = atPosition;
	//		//playerSprite.SetActive(true);
	//		isFalling = false;
	//		anim.SetBool(Pit, false);
	//		playerFallTrigger.SetActive(true);
	//		weapon.HideWeapon(false);
	//		foo = anim.GetBool(Pit);
	//		Debug.Log(foo + " Falling.");
	//	}
	//}
}
