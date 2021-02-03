using System;

using Ash.StateMachine;

using UnityEngine;


public class PlayerManager : MonoBehaviour
{
	public StateMachine<PlayerManager> stateMachine;
	
	[SerializeField] private float runSpeed = 40f;
	
	private PlayerAnimationController animController;
	private PlayerHazardTrigger hazardTrigger;
	private PlayerController playerController;
	private PlayerInput playerInput;
	private bool isMoving = true;
	
	[NonSerialized] public readonly PlayerBaseState playerBaseState = new PlayerBaseState();
	[NonSerialized] public readonly PlayerSpecialState playerSpecialState = new PlayerSpecialState();
	[NonSerialized] public readonly PlayerIdleState playerIdleState = new PlayerIdleState();
	
	private void Awake()
	{
		animController = GetComponent<PlayerAnimationController>();
		playerController = GetComponent<PlayerController>();
		playerInput = GetComponent<PlayerInput>();
		hazardTrigger = GetComponentInChildren<PlayerHazardTrigger>();
		stateMachine = new StateMachine<PlayerManager>(this);
		stateMachine.ChangeState(playerBaseState);
	}

	private void Update()
	{
		animController.SpriteFlip(playerInput.GetCursorDirection.x);
		animController.SetSpriteFacingDirection(playerInput.GetCursorDirection);
		animController.IsPlayerMoving(playerInput.GetDirectionAxis);
		stateMachine.Update();

	}
	
	private void FixedUpdate ()
	{
		playerController.Move(playerInput.GetDirectionAxis * (runSpeed * Time.fixedDeltaTime), isMoving);
		stateMachine.FixedUpdate();
	}

	//public void DealWithHazard()
	//{
	//	Hazard hazard = hazardTrigger.GetCurrentHazard;
	//	if(hazard != null)
	//		damageText.text = "Taking " + hazard.typeOfHazard + " damage!";
	//	else
	//	{
	//		Debug.Log("No Hazard");
	//	}
	//}

	//public void DealWithHazardExit()
	//{
	//	damageText.text = "No hazard.";
	//}
}
