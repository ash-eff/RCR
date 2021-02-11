using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class Weapon : MonoBehaviour
{
    public bool isOwned = false;
    public bool isEquipped = false;
    public new string name;
    public string description;
    public bool isUsingSpecial;
    public bool isSpecialCooledDown = false;
    public Sprite artwork;

    public int damage;
    public float rateOfFire;
    public float lastShot = 0;
    public float totalAmmo;
    public float currentAmmo;
    public float cooldownTimer;
    public float currentCooldownTimer;
    public int minAmmoForSpecial;


    public SpriteRenderer gunSprite;
    public Vector3 gunPosition;
    public Transform muzzlePosition;
    public GameObject bulletPrefab;
    public Animator anim;

    public CameraController cam;

    private void Awake()
    {
        cam = FindObjectOfType<CameraController>();
    }

    public virtual void FireWeapon(float rot)
    {
        if (isOwned && isEquipped)
        {
            if (Time.time > rateOfFire + lastShot && currentAmmo > 0)
            {
                cam.CameraShake();
                GameObject obj = Instantiate(bulletPrefab, muzzlePosition.position, Quaternion.identity);
                obj.GetComponent<Bullet>().FireBullet(rot, transform.position);
                currentAmmo--;
                anim.SetTrigger("Fire");
                lastShot = Time.time;
            }
        }
    }

    public virtual void SpecialAbility()
    {
        if (isOwned && isEquipped)
        {
            if (currentAmmo >= minAmmoForSpecial)
            {
                StartCoroutine(IeSpecialAbility());
                StartCooldownTimer();
            
                IEnumerator IeSpecialAbility()
                {
                    isUsingSpecial = true;
                
                    var rot = 0f;
                    cam.CameraShakeTimed(2.5f);
                    for (int i = 0; i < 50; i++)
                    {
                        rot = Random.Range(0, 361);
                        GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        obj.GetComponent<Bullet>().FireBullet(rot, transform.position);
                        //obj.GetComponent<Projectile>().damageAmount = 3;
                        currentAmmo--;

                        yield return new WaitForSeconds(.05f);
                    }

                    isUsingSpecial = false;
                }    
            }
        }
    }

    public void StartCooldownTimer()
    {
        StartCoroutine(SpecialAbilityCooldownTimer());
    }

    IEnumerator SpecialAbilityCooldownTimer()
    {
        isSpecialCooledDown = false;

        var timer = 0f;
        while (timer < cooldownTimer)
        {
            timer += Time.deltaTime;
            currentCooldownTimer = timer;
            yield return null;
        }

        isSpecialCooledDown = true;
    }

    public void EquipWeapon(bool _isEquipped)
    {
        gunSprite.enabled = _isEquipped;
        isEquipped = _isEquipped;
        //fistSprite.enabled = !isHidden;
    }

    public void HideWeapon(bool isHidden)
    {
        gunSprite.enabled = !isHidden;
    }
}
