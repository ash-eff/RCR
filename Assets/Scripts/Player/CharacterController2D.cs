using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private SpriteRenderer spr;
	private Rigidbody2D rigidbody2D;
	private Vector3 velocity = Vector3.zero;

	private void Awake()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	public void Move(Vector2 move, bool dash, bool crawl, bool jump)
	{
		// Move the character by finding the target velocity
		Vector3 targetVelocity = move * 10f;
		// And then smoothing it out and applying it to the character
		rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);
	}

	public void Flip(float xDir)
	{
		if (xDir > 0)
			spr.flipX = false;

		if (xDir < 0)
			spr.flipX = true;
	}
}
