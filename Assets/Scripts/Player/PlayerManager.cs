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
	[SerializeField] private PlayerWeapon weapon;
	[SerializeField] private GameObject startMarker;
	[SerializeField] private GameObject endMarker;

	private PlayerInputs playerInputs;
	private PlayerCursor cursor;
	public Vector2 directionAxis;
	public Vector2 landPosition;
	private float runSpeed = 40f;
	private float jumpSpeed = 15f;
	public bool isFalling;
	private bool isDashing;
	private bool isJumping;
	private bool canJump = true;
	public float idleTimer = 0;

	[SerializeField] private GameObject playerSprite;
	[SerializeField] private GameObject playerFallTrigger;
	private static readonly int Pit = Animator.StringToHash("Pit");


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
		playerInputs.Player.Jump.performed += cxt => Jump();
	}

	private void Update()
	{
		if (!isFalling)
		{
			// flip sprite based on aim direction
			controller.Flip(cursor.GetCursorDirection.x);
		
			// set sprite direction animation
			anim.SetFloat("xDir", cursor.GetCursorDirection.x);
			anim.SetFloat("yDir", cursor.GetCursorDirection.y);
		
			// set idle or not based on movement
			if (Mathf.Abs(directionAxis.x) > 0 || Mathf.Abs(directionAxis.y) > 0)
			{
				anim.SetBool("Moving", true);
				anim.SetBool("Idle", false);
				weapon.HideWeapon(false);
				idleTimer = 0;
			}
			else
			{
				anim.SetBool("Moving", false);
			
				if (weapon.isFiring)
				{
					anim.SetBool("Idle", false);
					weapon.HideWeapon(false);
					idleTimer = 0;
				}
				else
				{
					// count down for idle
					idleTimer += Time.deltaTime;
					if (idleTimer > 5f)
					{
						anim.SetBool("Idle", true);
						weapon.HideWeapon(true);
					}
				}
			}
		}

	}

	void FixedUpdate ()
	{
		// Move our character
		controller.Move(directionAxis * (runSpeed * Time.fixedDeltaTime),landPosition, isDashing, isFalling, isJumping);
	}

	public void PlayerIsFalling(Vector3 atPosition)
	{
		anim.SetBool(Pit, true);
		var foo = anim.GetBool(Pit);
		Debug.Log(foo + " Falling.");
		playerFallTrigger.SetActive(false);
		StartCoroutine(PlayerFall());

		IEnumerator PlayerFall()
		{
			weapon.HideWeapon(true);
			isFalling = true;
			//playerSprite.SetActive(false);

			yield return new WaitForSeconds(1f);
			
			transform.position = atPosition;
			//playerSprite.SetActive(true);
			isFalling = false;
			anim.SetBool(Pit, false);
			playerFallTrigger.SetActive(true);
			weapon.HideWeapon(false);
			foo = anim.GetBool(Pit);
			Debug.Log(foo + " Falling.");
		}
	}

	private void SetMovement(Vector2 movement) => directionAxis = movement;
	
	private void ResetMovement() => directionAxis = Vector3.zero;

	private void Jump()
	{
		if (canJump)
		{
			canJump = false;
			landPosition = (Vector2)transform.position + (directionAxis * 4);
			isJumping = true;
			anim.SetBool("Jumping", true);
			playerFallTrigger.SetActive(false);
		}
	}

	public void DoneJumping()
	{
		isJumping = false;
		anim.SetBool("Jumping", false);
		canJump = true;
		playerFallTrigger.SetActive(true);
	}
}
