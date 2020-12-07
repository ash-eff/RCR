using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

	[SerializeField] private CharacterController2D controller;
	[SerializeField] private Animator animator;
	
	private PlayerInputs playerInputs;
	private Vector2 directionAxis;
	private float runSpeed = 40f;
	private float horizontalMove = 0f;
	
	[SerializeField] private bool jump = false;
	[SerializeField] private bool dash = false;
	[SerializeField] private bool crawl = false;
	[SerializeField] private bool standInPlace = false;
	[SerializeField] private GameObject playerSprite;						// The Transform that will be flipped for right/left facing
	
	private bool facingRight = true;
	
	//bool dashAxis = false;

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
		playerInputs = new PlayerInputs();
        
		playerInputs.Player.Move.performed += cxt => SetMovement(cxt.ReadValue<Vector2>());
		playerInputs.Player.Move.canceled += cxt => ResetMovement();
        
		playerInputs.Player.Jump.performed += cxt => jump = true;

		playerInputs.Player.Stand.performed += cxt => StandInPlace(true);
		playerInputs.Player.Stand.canceled += cxt => StandInPlace(false);
	}
	
	void Update () 
	{
		if(!standInPlace)
			horizontalMove = directionAxis.x * runSpeed;
		else
			horizontalMove = 0;
		
		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
		
		// for emergency until ground detection is refined
		if (Input.GetKeyDown(KeyCode.Y))
		{
			controller.EmergencyJump();
		}
		
		// If the input is moving the player right and the player is facing left...
		if (directionAxis.x > 0 && !facingRight)
		{
			// ... flip the player.
			FlipSprite();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (directionAxis.x < 0 && facingRight)
		{
			// ... flip the player.
			FlipSprite();
		}

		/*if (Input.GetAxisRaw("Dash") == 1 || Input.GetAxisRaw("Dash") == -1) //RT in Unity 2017 = -1, RT in Unity 2019 = 1
		{
			if (dashAxis == false)
			{
				dashAxis = true;
				dash = true;
			}
		}
		else
		{
			dashAxis = false;
		}
		*/
	}
	
	void FixedUpdate ()
	{
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, dash, crawl, jump);
		if(jump)
			animator.SetBool("IsJumping", true);
		jump = false;
		dash = false;
	}
	
	private void FlipSprite()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = playerSprite.transform.localScale;
		theScale.x *= -1;
		playerSprite.transform.localScale = theScale;
	}
	private void SetMovement(Vector2 movement) => directionAxis = movement;
	
	private void ResetMovement() => directionAxis = Vector3.zero;
	


	private void StandInPlace(bool b)
	{
		standInPlace = b;
	}
	public void OnFall()
	{
		//animator.SetBool("IsJumping", true);
	}

	public void OnLandEvent()
	{
		//Debug.Log("Land");
		animator.SetBool("IsJumping", false);

	}
	
	public void OnJumpEvent()
	{
		//Debug.Log("Jump");
		animator.SetBool("IsJumping", true);

	}

	public void OnCrouchEvent(bool isCrouching)
	{
		//animator.SetBool("IsCrouching", isCrouching);
	}
}
