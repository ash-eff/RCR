using System;

using Ash.StateMachine;

using UnityEngine;


public class PlayerManager : MonoBehaviour
{
	public StateMachine<PlayerManager> stateMachine; 
	public MessageSystem messageSystem;
	[SerializeField] private float runSpeed = 40f;
	[SerializeField] private SpriteRenderer minimapSprite;

	private PlayerAnimationController animController;
	private PlayerController playerController;
	private PlayerInput playerInput;
	public PlayerWeaponManager playerWeaponManager;
	private bool isMoving = true;
	private ObjectiveArrow objectiveArrow;
	
	[NonSerialized] public readonly PlayerBaseState playerBaseState = new PlayerBaseState();
	[NonSerialized] public readonly PlayerSpecialState playerSpecialState = new PlayerSpecialState();
	[NonSerialized] public readonly PlayerIdleState playerIdleState = new PlayerIdleState();
	
	private void Awake()
	{
		animController = GetComponent<PlayerAnimationController>();
		playerController = GetComponent<PlayerController>();
		playerInput = GetComponent<PlayerInput>();
		playerWeaponManager = GetComponent<PlayerWeaponManager>();
		messageSystem = FindObjectOfType<MessageSystem>();
		objectiveArrow = GetComponentInChildren<ObjectiveArrow>();
		minimapSprite.enabled = true;
	}

	private void Start()
	{
		stateMachine = new StateMachine<PlayerManager>(this);
		stateMachine.ChangeState(playerBaseState);
		//SendMessageToMessageSystem("Kill everything and make your escape.", 2);
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

	public void SendMessageToMessageSystem(string message, int times)
	{
		messageSystem.DisplayMessage(message, times);
	}

	public void TrackObjective(GameObject obj)
	{
		objectiveArrow.SetObjective = obj.transform;
	}

	public void PlayerIdleState()
	{
		// something is triggering idle when it's not needed
		//stateMachine.ChangeState(playerIdleState);
	}
	
	public void PlayerBaseState()
	{
		//stateMachine.ChangeState(playerBaseState);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("DigSite"))
		{
			Debug.Log("Can Dig");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("DigSite"))
		{
			Debug.Log("Can No Longer Dig");
		}
	}
}
