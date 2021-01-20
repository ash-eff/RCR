using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	private Rigidbody2D rigidbody2D;
	private Vector3 velocity = Vector3.zero;
	private float movementSmoothing = .05f;	// How much to smooth out the movement
	
	private bool isGrounded = true;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	public void Move(Vector2 move)
	{
		if (isGrounded)
		{
			Vector3 targetVelocity = move * 10f;
			rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
		}
	}
}
