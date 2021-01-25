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
    public new string name;
    public string description;

    public Sprite artwork;

    public int damage;
    public float rateOfFire;
    public float reloadTime;
    public float lastShot = 0;
    public float totalAmmo;
    public float currentAmmo;

    public SpriteRenderer gunSprite;
    public Vector2 gunPosition;
    public Transform muzzlePosition;
    public GameObject projectilePrefab;
    public GameObject shellPrefab;
    public GameObject impactPrefab;

    public CameraController cam;

    private void Awake()
    {
        cam = FindObjectOfType<CameraController>();
    }

    public virtual void FireWeapon(float rot)
    {
        if (Time.time > rateOfFire + lastShot && currentAmmo > 0)
        {
            cam.CameraShake();
            var offset = Random.Range(-4, 4);
            rot += offset;
            Instantiate(shellPrefab, transform.position, Quaternion.identity);
            GameObject obj = Instantiate(projectilePrefab, muzzlePosition.position, Quaternion.Euler(0, 0, rot));
            currentAmmo--;
            lastShot = Time.time;
        }
    }

    public virtual void SpecialAbility()
    {
        if (currentAmmo >= 50)
        {
            StartCoroutine(IeSpecialAbility());
        
            IEnumerator IeSpecialAbility()
            {
                var rot = 0f;
                cam.CameraShakeTimed(2.5f);
                for (int i = 0; i < 50; i++)
                {
                    rot = Random.Range(0, 361);
                    Instantiate(shellPrefab, transform.position, Quaternion.identity);
                    GameObject obj = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, rot));
                    obj.GetComponent<Projectile>().damageAmount = 3;
                    currentAmmo--;

                    yield return new WaitForSeconds(.05f);
                }
                
                PlayerManager playerManager = GetComponentInParent<PlayerManager>(); 
                playerManager.ResetSpecialAbility();
            }    
        }
    }

    //private void Reload()
    //{
    //    reloadBar.gameObject.SetActive(true);
    //    ammoAmount = 0;
    //    ammoImage.sprite = ammoSprites[ammoAmount];
    //    
    //    StartCoroutine(IeReload());
    //    StartCoroutine(TimerBar());
//
    //    IEnumerator TimerBar()
    //    {
    //        var timer = 0f;
    //        reloadBar.fillAmount = timer;
    //        reloadBarHolder.SetActive(true);
    //        
    //        while (timer < reloadTime)
    //        {
    //            timer += Time.deltaTime / reloadTime;
    //            reloadBar.fillAmount = timer;
    //            yield return null;
    //        }
    //        
    //        reloadBarHolder.SetActive(false);
    //    }
    //    
    //    IEnumerator IeReload()
    //    {
    //        var reloadCount = 0;
    //        var reloadInterval = reloadTime / maxAmmo;
    //        //reloadCount++;
    //        //ammoImage.sprite = ammoSprites[reloadCount];
    //        
    //        while (reloadCount < maxAmmo)
    //        {
    //            reloadCount++;
    //            ammoImage.sprite = ammoSprites[reloadCount];
    //            yield return new WaitForSeconds(reloadInterval);
    //        }
    //        
    //        ammoAmount = maxAmmo;
    //        isReloading = false;
    //        canReload = true;
    //    }
    //}
    
    //public void HideWeapon(bool isHidden)
    //{
    //    gunSpr.enabled = !isHidden;
    //    fistSprite.enabled = !isHidden;
    //}
}
