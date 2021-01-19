using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private SpriteRenderer spr;
	public float jumpSpeed;
	private bool moveJump;
	private Rigidbody2D rigidbody2D;
	private Vector3 velocity = Vector3.zero;
	public bool isGrounded = true;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	public void Move(Vector2 move, Vector2 landPosition, bool dash, bool fall, bool jump)
	{
		//only control the player if grounded or airControl is turned on
		if (isGrounded && !fall && !jump)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = move * 10f;
			// And then smoothing it out and applying it to the character
			rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
		}
		
		// If the player should jump
		if (jump)
		{
			isGrounded = false;
			var sqrRemainingDistance = ((Vector2)transform.position - landPosition).sqrMagnitude;

			if (sqrRemainingDistance > float.Epsilon)
			{
				var newPosition =
					Vector3.MoveTowards(rigidbody2D.position, landPosition, jumpSpeed * Time.fixedDeltaTime);
				rigidbody2D.MovePosition(newPosition);
			}
			else
			{
				OnLand();
			}
		}

		// If the player should fall...
		if (fall)
		{
			isGrounded = false;
			// fall stuff here
			rigidbody2D.velocity = Vector3.zero;
		}

		if (!fall || !jump)
		{
			isGrounded = true;
		}
	}

	private void OnLand()
	{
		OnLandEvent.Invoke();
	}

	public void Flip(float xDir)
	{
		if (xDir > 0)
			spr.transform.localScale = new Vector3(1,1,1);

		if (xDir < 0)
			spr.transform.localScale = new Vector3(-1,1,1);
	}
}
