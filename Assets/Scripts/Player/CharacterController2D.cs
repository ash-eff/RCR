using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float jumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float crawlSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	// How much to smooth out the movement
	private bool airControl = false;						// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround;						// A mask determining what is ground to the character
	[SerializeField] private Transform ceilingCheck;						// A position marking where to check for ceilings.
	[SerializeField] private Transform groundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Collider2D crouchDisableCollider;				// A collider that will be disabled when crouching
	[SerializeField] private GameObject playerSprite;						// The Transform that will be flipped for right/left facing
	
	const float checkRadius = .1f; 							// Radius of the overlap circle to determine if grounded
	[SerializeField] private bool isGrounded;            		// Whether or not the player is grounded.
	private Rigidbody2D rigidbody2D;
	private bool facingRight = true;  							// For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent OnJumpEvent;


	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool wasCrawling = false;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		
		if (OnJumpEvent == null)
			OnJumpEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		// WHY DOES THIS CALL LAND AND JUMP WHEN YOU JUMP????
		// WORKS FOR NOW
		bool wasGrounded = isGrounded;
		isGrounded = false;
		
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, checkRadius, whatIsGround);

		if (colliders.Length > 0)
		{
			isGrounded = true;
			if (!wasGrounded) 
				OnLandEvent.Invoke();
		}
		else
		{
			isGrounded = false;
			if(wasGrounded)
				OnJumpEvent.Invoke();
		}
			
	}
	
	public void Move(float move, bool dash, bool crawl, bool jump)
	{
		// If crawling, check to see if the character can stand up
		if (!crawl)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(ceilingCheck.position, checkRadius, whatIsGround))
			{
				crawl = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (isGrounded || airControl)
		{
			// If crawling
			if (crawl)
			{
				if (!wasCrawling)
				{
					wasCrawling = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crawlSpeed multiplier
				move *= crawlSpeed;
				
				// Disable one of the colliders when crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = false;
			} 
			else
			{
				// Enable the collider when not crouching
				if (crouchDisableCollider != null)
					crouchDisableCollider.enabled = true;
				
				if (wasCrawling)
				{
					wasCrawling = false;
					OnCrouchEvent.Invoke(false);
				}
			} 

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && facingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		
		// If the player should jump...
		if (isGrounded && jump)
		{
			// Add a vertical force to the player.
			isGrounded = false;
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}

	}
	
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = playerSprite.transform.localScale;
		theScale.x *= -1;
		playerSprite.transform.localScale = theScale;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(groundCheck.transform.position, checkRadius);
		Gizmos.DrawWireSphere(ceilingCheck.transform.position, checkRadius);
	}
}
