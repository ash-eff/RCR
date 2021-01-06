using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform muzzlePosition;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Animator weaponAnim;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private GameObject reloadUI;
    [SerializeField] private SpriteRenderer gunSpr;
    [SerializeField] private float lastShot = 0;
    [SerializeField] private float rateOfFire;
    [SerializeField] private int ammoAmount;
    [SerializeField] private Image ammoImage;
    [SerializeField] private CameraController camController;
    private int maxAmmo;
    private bool isReloading = false;
    [SerializeField] private Sprite[] ammoSprites;
    private float shakeTimer;
    private Vector2 weaponPosition;
    private PlayerCursor cursor;
    public bool isFiring;
    [SerializeField] private CinemachineVirtualCamera cine;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    private PlayerInputs playerInputs;
    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }
    
    private void Awake()
    {
        camController = FindObjectOfType<CameraController>();
        maxAmmo = ammoAmount;
        ammoImage.sprite = ammoSprites[ammoAmount];
        weaponPosition = gunTransform.localPosition;
        cursor = GetComponent<PlayerCursor>();
        playerInputs = new PlayerInputs();
        playerInputs.Player.Shoot.performed += cxt => isFiring = true;
        playerInputs.Player.Shoot.canceled += cxt => isFiring = false;
        playerInputs.Player.Reload.performed += cxt => Reload();

        Cursor.visible = false;
    }

    private void Update()
    {
        var direction = cursor.GetNonZeroDirection;
        var rot = MyUtils.GetAngleFromVectorFloat(direction.normalized);

        PositionWeapon(direction.normalized.x, rot);

        gunTransform.rotation = Quaternion.Euler(0,0,rot);
        
        if (isFiring && Time.time > rateOfFire + lastShot && ammoAmount > 0)
        {
            //FireWeapon(direction, rot);
            FireWeapon(direction.normalized, rot);

            camController.CameraShake();
        }

        if (ammoAmount <= 0 && !isReloading)
        {
            reloadUI.SetActive(true);
        }
        else
        {
            reloadUI.SetActive(false);

        }
        
        
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0)
            {
                cinemachineBasicMultiChannelPerlin =
                    cine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    void FireWeapon(Vector3 dir, float rot)
    {
        weaponAnim.SetTrigger("Fire");
        UpdateAmmo();
        Debug.DrawRay(muzzlePosition.position,dir.normalized * 6, Color.blue, .5f);
        var offset = Random.Range(-4, 4);
        rot += offset;
        //Instantiate(shellPrefab, startPosition, quaternion.identity);
        Instantiate(projectilePrefab, muzzlePosition.position, Quaternion.Euler(0, 0, rot));
        //shakeTimer = .25f;
        lastShot = Time.time;
    }

    private void Reload()
    {
        isReloading = true;
        ammoAmount = 0;
        ammoImage.sprite = ammoSprites[ammoAmount];
        
        StartCoroutine(IeReload());

        IEnumerator IeReload()
        {
            int reloadCount = 0;
            while (reloadCount < maxAmmo)
            {
                yield return new WaitForSeconds(.33f);
                reloadCount++;
                ammoImage.sprite = ammoSprites[reloadCount];
            }

            ammoAmount = maxAmmo;
            isReloading = false;
        }
    }

    private void UpdateAmmo()
    {
        if (ammoAmount > 0)
        {
            ammoAmount--;
            ammoImage.sprite = ammoSprites[ammoAmount];
        }
    }
    
    private void PositionWeapon(float xDir, float rot)
    {
        if (xDir > 0)
        {
            gunTransform.localPosition = new Vector3(weaponPosition.x, weaponPosition.y, 0f);
            gunTransform.localScale = new Vector3(1, 1, 1);
        }


        if (xDir < 0)
        {
            gunTransform.localPosition = new Vector3(-weaponPosition.x, weaponPosition.y, 0f);
            gunTransform.localScale = new Vector3(1, -1, 1);
        }

        if (rot > 0f && rot < 180f)
        {
            gunSpr.sortingOrder = 3;
        }
        else
        {
            gunSpr.sortingOrder = 5;
        }
            
    }
}
