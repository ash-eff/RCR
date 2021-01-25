using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ash.MyUtils;
using Ash.StateMachine;
using Microsoft.SqlServer.Server;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
	public StateMachine<PlayerManager> stateMachine;
	[SerializeField] private float runSpeed = 40f;
	private PlayerAnimationController animController;
	private PlayerHazardTrigger hazardTrigger;
	private PlayerController playerController;
	private PlayerInput playerInput;
	public bool specialAvailable = false;
	private bool canCollectSpecialPoints = true;
	private int inventoryIndex = -1;
	private int specialAbilityPoints = 0;
	[SerializeField] private int pointsNeededForSpecial;
	[SerializeField] private Image specialFillBar;
	[SerializeField] private GameObject specialReadyText;
	[SerializeField] private Weapon currentWeapon;
	[SerializeField] private Transform weaponInventory;
	[SerializeField] private Weapon[] allWeapons;
	[SerializeField] private Image weaponImage;
	[SerializeField] private TextMeshProUGUI ammoText;

	private Dictionary<string, Weapon> availableWeaponsDictionary = new Dictionary<string, Weapon>();
	private Dictionary<string, Weapon> ownedWeaponsDictionary = new Dictionary<string, Weapon>();

	[NonSerialized] public readonly PlayerBaseState playerBaseState = new PlayerBaseState();
	[NonSerialized] public readonly PlayerIdleState playerIdleState = new PlayerIdleState();
	
	private void Awake()
	{
		ResetSpecialAbility();
		foreach (Weapon weapon in allWeapons) 
		{
			availableWeaponsDictionary.Add(weapon.name, weapon);
		}
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
		PositionWeaponHolder(playerInput.GetCursorDirection.x, playerInput.GetRotationToCursor);
		stateMachine.Update();
		if (currentWeapon != null)
		{
			string ammo = currentWeapon.currentAmmo + "/" + currentWeapon.totalAmmo;
			ammoText.text = ammo;
		}
	}
	
	private void FixedUpdate ()
	{
		playerController.Move(playerInput.GetDirectionAxis * (runSpeed * Time.fixedDeltaTime));
		stateMachine.FixedUpdate();
	}

	#region Private Functions

	private void EquipWeapon(Weapon weapon)
	{
		// check if you are weapon swapping
		var lastWeapon = currentWeapon;
		if(lastWeapon != null)
			lastWeapon.gameObject.SetActive(false);
		
		currentWeapon = weapon;
		currentWeapon.gameObject.SetActive(true);
		string ammo = currentWeapon.currentAmmo + "/" + currentWeapon.totalAmmo;
		ammoText.text = ammo;
		weaponImage.sprite = currentWeapon.artwork;
	}

	private void MakeWeaponAvailableInInventory(Weapon weapon)
	{
		weapon.isOwned = true;
	}
	
	private void PositionWeaponHolder(float xDir, float rot)
	{
		// do nothing if there is no weapon equipped
		if (currentWeapon == null)
			return;
		
		var weaponTransform = currentWeapon.transform;
		var weaponGunPosition = currentWeapon.gunPosition;
		
		// put weapon to the right or left of player based on cursor position
		if (xDir > 0)
		{
			weaponTransform.localPosition = new Vector3(weaponGunPosition.x, weaponGunPosition.y, 0f);
			weaponTransform.localScale = new Vector3(1, 1, 1);
		}
		if (xDir < 0)
		{
			weaponTransform.localPosition = new Vector3(-weaponGunPosition.x, weaponGunPosition.y, 0f);
			weaponTransform.localScale = new Vector3(1, -1, 1);
		}

		// rotate the weapon
		weaponTransform.rotation = Quaternion.Euler(0,0, rot);
		
		
		// set the sprite order, behind or in front of the player based on cursor position
		if (rot > 0f && rot < 180f)
		{
			currentWeapon.gunSprite.sortingOrder = 3;
		}
		else
		{
			currentWeapon.gunSprite.sortingOrder = 5;
		}
	}

	#endregion
	

	#region Public Functions

	public void SwapWeapon()
	{
		if (ownedWeaponsDictionary.Count <= 1)
			return;
		
		List<Weapon> weaponIndex = ownedWeaponsDictionary.Values.ToList();
		
		Debug.Log("Weapon Index Count: " + weaponIndex.Count);
		int ind = weaponIndex.IndexOf(currentWeapon);
		if (ind + 1 > weaponIndex.Count - 1)
			ind = 0;
		else
			ind++;
		
		EquipWeapon(weaponIndex[ind]);
	}

	public void FireWeapon()
	{
		if (currentWeapon != null)
		{
			currentWeapon.FireWeapon(playerInput.GetRotationToCursor);
		}
	}

	public void SpecialAbility()
	{
		if (currentWeapon != null && specialAvailable)
		{
			specialAvailable = false;
			currentWeapon.SpecialAbility();
		}
	}
	
	public void CollectNewWeapon(Weapon weapon)
	{
		// if you already have the weapon, do nothing
		if (ownedWeaponsDictionary.ContainsKey(weapon.name))
			return;

		// pull weapon from the available inventory of weapons
		Weapon newWeapon = availableWeaponsDictionary[weapon.name];
		newWeapon.gunPosition = newWeapon.transform.localPosition;
		availableWeaponsDictionary.Remove(weapon.name);

		// add to dictionary
		ownedWeaponsDictionary.Add(newWeapon.name, newWeapon);
		Debug.Log(newWeapon.name + " added to Inventory");
		// add to inventory
		MakeWeaponAvailableInInventory(newWeapon);
		
		// if it's the first weapon you pick up, automatically equip it
		if(ownedWeaponsDictionary.Count == 1)
			EquipWeapon(newWeapon);
	}

	public void CollectAmmo(int ammoAmount)
	{
		currentWeapon.currentAmmo += ammoAmount;
	}

	public void GainSpecialAbilityPoints()
	{
		if (canCollectSpecialPoints)
		{
			specialAbilityPoints++;
			float fillAmount = 1f / pointsNeededForSpecial;
			specialFillBar.fillAmount += fillAmount;
			if (specialAbilityPoints > pointsNeededForSpecial)
			{
				specialAbilityPoints = 0;
				canCollectSpecialPoints = false;
				specialAvailable = true;
				specialReadyText.SetActive(true);
			}
		}
	}

	public void ResetSpecialAbility()
	{
		specialFillBar.fillAmount = 0;
		specialReadyText.SetActive(false);
		canCollectSpecialPoints = true;
	}

	//public void PlayerIsFalling(Vector3 atPosition)
	//{
	//	anim.SetBool(Pit, true);
	//	var foo = anim.GetBool(Pit);
	//	Debug.Log(foo + " Falling.");
	//	playerFallTrigger.SetActive(false);
	//	StartCoroutine(PlayerFall());
//
	//	IEnumerator PlayerFall()
	//	{
	//		weapon.HideWeapon(true);
	//		isFalling = true;
	//		//playerSprite.SetActive(false);
//
	//		yield return new WaitForSeconds(1f);
	//		
	//		transform.position = atPosition;
	//		//playerSprite.SetActive(true);
	//		isFalling = false;
	//		anim.SetBool(Pit, false);
	//		playerFallTrigger.SetActive(true);
	//		weapon.HideWeapon(false);
	//		foo = anim.GetBool(Pit);
	//		Debug.Log(foo + " Falling.");
	//	}
	//}

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

	#endregion
}
