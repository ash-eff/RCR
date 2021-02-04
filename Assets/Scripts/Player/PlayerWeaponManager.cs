using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ash.MyUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponManager : MonoBehaviour
{
	[SerializeField] private Weapon currentWeapon;
	[SerializeField] private Weapon startingWeapon;
	[SerializeField] private Transform weaponInventory;
	[SerializeField] private Weapon[] allWeapons;
	[SerializeField] private Image weaponImage;
	[SerializeField] private TextMeshProUGUI ammoText;
	[SerializeField] private int pointsNeededForSpecial;
	[SerializeField] private Image specialCooldownBar;
	[SerializeField] private TextMeshProUGUI specialReadyText;

	private int inventoryIndex = -1;
	private PlayerInput playerInput;
	//private PlayerMessageHandler messageHandler;

	private Dictionary<string, Weapon> availableWeaponsDictionary = new Dictionary<string, Weapon>();
	private Dictionary<string, Weapon> ownedWeaponsDictionary = new Dictionary<string, Weapon>();

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();
		//messageHandler = GetComponent<PlayerMessageHandler>();
		foreach (Weapon weapon in allWeapons) 
		{
			availableWeaponsDictionary.Add(weapon.name, weapon);
		}
		CollectNewWeapon(startingWeapon);
	}

	private void Update()
	{
		PositionWeaponHolder(playerInput.GetCursorDirection.x, playerInput.GetRotationToCursor);
		if (currentWeapon != null)
		{
			if (!currentWeapon.isSpecialCooledDown)
			{
				specialCooldownBar.fillAmount = (currentWeapon.currentCooldownTimer / currentWeapon.cooldownTimer);
				specialReadyText.text =
					MyUtils.GetTimeString(currentWeapon.currentCooldownTimer);
			}
			else
			{
				specialCooldownBar.fillAmount = 1f;
				specialReadyText.text = "READY!";
			}
			
			string ammo = currentWeapon.currentAmmo + "/" + currentWeapon.totalAmmo;
			ammoText.text = ammo;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("AmmoPickUp"))
		{
			CollectAmmo(other.GetComponent<Ammo>().CollectAmmo());
		}

		if (other.CompareTag("WeaponPickUp"))
		{
			CollectNewWeapon(other.GetComponent<WeaponPickup>().CollectWeapon());
		}
	}

	#region Public Functions

	// called from an event in PlayerInputManager
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

	// called from an event in PlayerInputManager
	public void FireWeapon()
	{
		if (currentWeapon != null)
		{
			currentWeapon.FireWeapon(playerInput.GetRotationToCursor);
		}
	}

	// called from an event in PlayerInputManager
	public void SpecialAbility()
	{
		if (currentWeapon != null && !currentWeapon.isUsingSpecial && currentWeapon.isSpecialCooledDown)
		{
			//isMoving = false;
			currentWeapon.SpecialAbility();
		}
	}

	// called from a trigger on WeaponPickup
	public void CollectNewWeapon(Weapon weapon)
	{
		// if you already have the weapon, do nothing
		if (ownedWeaponsDictionary.ContainsKey(weapon.name))
			return;

		//messageHandler.CreateFloatingText(weapon.name);
		// pull weapon from the available inventory of weapons
		Weapon newWeapon = availableWeaponsDictionary[weapon.name];
		newWeapon.gunPosition = newWeapon.transform.localPosition;
		availableWeaponsDictionary.Remove(weapon.name);

		// add to dictionary
		ownedWeaponsDictionary.Add(newWeapon.name, newWeapon);
		newWeapon.StartCooldownTimer();
		// add to inventory
		MakeWeaponAvailableInInventory(newWeapon);
		
		// if it's the first weapon you pick up, automatically equip it
		if(ownedWeaponsDictionary.Count == 1)
			EquipWeapon(newWeapon);
	}

	// called from a trigger on Ammo
	public void CollectAmmo(int ammoAmount)
	{
		currentWeapon.currentAmmo += ammoAmount;
		//messageHandler.CreateFloatingText("+ " + ammoAmount.ToString() + " ammo");
	}

	#endregion

	#region Private Functions
	
	private void EquipWeapon(Weapon weapon)
	{
		// check if you are weapon swapping
		var lastWeapon = currentWeapon;
		if(lastWeapon != null)
			lastWeapon.EquipWeapon(false);
		
		currentWeapon = weapon;
		currentWeapon.EquipWeapon(true);
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
		weaponTransform.rotation = Quaternion.Euler(45,0, rot);
		
		
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
}
