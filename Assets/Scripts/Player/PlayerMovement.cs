using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private CharacterController2D controller;
	[SerializeField] private Animator animator;
	
	public float runSpeed = 40f;

	private float horizontalMove = 0f;
	[SerializeField] private bool jump = false;
	[SerializeField] private bool dash = false;
	[SerializeField] private bool crawl = false;

	//bool dashAxis = false;
	
	// Update is called once per frame
	void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
		}

		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			crawl = true;
		}
		else if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			crawl = false;
		}

		if (Input.GetKeyDown(KeyCode.C))
		{
			dash = true;
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

	public void OnFall()
	{
		//animator.SetBool("IsJumping", true);
	}

	public void OnLandEvent()
	{
		Debug.Log("Land");
		animator.SetBool("IsJumping", false);

	}
	
	public void OnJumpEvent()
	{
		Debug.Log("Jump");
		animator.SetBool("IsJumping", true);

	}

	public void OnCrouchEvent(bool isCrouching)
	{
		//animator.SetBool("IsCrouching", isCrouching);
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
}
