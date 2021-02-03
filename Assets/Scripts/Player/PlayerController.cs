using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	private Rigidbody rigidbody2D;
	private Vector3 velocity = Vector3.zero;
	private float movementSmoothing = .05f;	// How much to smooth out the movement
	
	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody>();
	}
	
	public void Move(Vector2 move, bool canMove)
	{
		if (canMove)
		{
			Vector3 targetVelocity = new Vector3(move.x, 0f, move.y) * 10f;
			rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
		}
		else
		{
			rigidbody2D.velocity = Vector2.zero;
		}
	}
}
