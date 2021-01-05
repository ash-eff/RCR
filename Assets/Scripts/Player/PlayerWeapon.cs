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
    [SerializeField] private float lastShot = 0;
    [SerializeField] private float rateOfFire;
    [SerializeField] private int ammoAmount;
    [SerializeField] private Image ammoImage;
    private int maxAmmo;
    private bool isReloading = false;
    [SerializeField] private Sprite[] ammoSprites;
    private float shakeTimer;
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
        maxAmmo = ammoAmount;
        ammoImage.sprite = ammoSprites[ammoAmount];
        
        cursor = GetComponent<PlayerCursor>();
        playerInputs = new PlayerInputs();
        playerInputs.Player.Shoot.performed += cxt => isFiring = true;
        playerInputs.Player.Shoot.canceled += cxt => isFiring = false;
        playerInputs.Player.Reload.performed += cxt => Reload();

        Cursor.visible = false;
    }

    private void Update()
    {
        var direction = cursor.GetCursorPosition - transform.position;
        var rot = MyUtils.GetAngleFromVectorFloat(direction.normalized);
        FlipWeapon(cursor.AimDirection.x);

        gunTransform.rotation = Quaternion.Euler(0,0,rot);
        
        if (isFiring && Time.time > rateOfFire + lastShot && ammoAmount > 0)
        {
            FireWeapon(direction, rot);
            ShakeCamera(.2f, .15f);
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
    
    private void FlipWeapon(float xDir)
    {
        if (xDir > 0)
            gunTransform.localScale = new Vector3(1, 1, 1);

        if (xDir < 0)
            gunTransform.localScale = new Vector3(1, -1, 1);
    }

    void ShakeCamera(float _intensity, float _time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _intensity;
        shakeTimer = _time;
    }
    
}
