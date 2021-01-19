using System.Collections;
using Ash.MyUtils;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform muzzlePosition;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject casingPrefab;
    [SerializeField] private Animator weaponAnim;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private GameObject reloadUI;
    [SerializeField] private SpriteRenderer gunSpr;
    [SerializeField] private SpriteRenderer fistSprite;
    [SerializeField] private float lastShot = 0;
    [SerializeField] private float rateOfFire;
    [SerializeField] private float reloadTime;
    [SerializeField] private int ammoAmount;
    [SerializeField] private Image ammoImage;
    [SerializeField] private CameraController camController;
    [SerializeField] private Image reloadBar;
    [SerializeField] private GameObject reloadBarHolder;

    private int maxAmmo;
    private bool isReloading = false;
    private bool canReload = true;
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
        playerInputs.Player.Reload.performed += cxt => isReloading = true;

        Cursor.visible = false;
    }

    private void Update()
    {
        var direction = cursor.GetCursorDirection;
        var rot = MyUtils.GetAngleFromVectorFloat(direction.normalized);

        if (rot > 45f && rot < 135f)
        {
            gunSpr.sortingOrder = 3;
            fistSprite.sortingOrder = 3;
        }
        else
        {
            gunSpr.sortingOrder = 4;
            fistSprite.sortingOrder = 5;
        }

        PositionWeapon(direction.normalized.x, rot);

        gunTransform.rotation = Quaternion.Euler(0,0,rot);

        if (isFiring && Time.time > rateOfFire + lastShot && ammoAmount > 0)
        {
            //FireWeapon(direction, rot);
            FireWeapon(direction.normalized, rot);

            camController.CameraShake();
        }

        if (isReloading && canReload)
        {
            canReload = false;
            Reload();
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
        Instantiate(casingPrefab, transform.position, Quaternion.identity);
        Instantiate(projectilePrefab, muzzlePosition.position, Quaternion.Euler(0, 0, rot));
        //shakeTimer = .25f;
        lastShot = Time.time;
    }

    public void HideWeapon(bool isHidden)
    {
        gunSpr.enabled = !isHidden;
        fistSprite.enabled = !isHidden;
    }

    private void Reload()
    {
        reloadBar.gameObject.SetActive(true);
        ammoAmount = 0;
        ammoImage.sprite = ammoSprites[ammoAmount];
        
        StartCoroutine(IeReload());
        StartCoroutine(TimerBar());

        //IEnumerator IeReload()
        //{
        //    int reloadCount = 0;
        //    while (reloadCount < maxAmmo)
        //    {
        //        yield return new WaitForSeconds(.33f);
        //        reloadCount++;
        //        ammoImage.sprite = ammoSprites[reloadCount];
        //    }
//
        //    ammoAmount = maxAmmo;
        //    isReloading = false;
        //    canReload = true;
        //}
        
        

        IEnumerator TimerBar()
        {
            var timer = 0f;
            reloadBar.fillAmount = timer;
            reloadBarHolder.SetActive(true);
            
            while (timer < reloadTime)
            {
                timer += Time.deltaTime / reloadTime;
                reloadBar.fillAmount = timer;
                yield return null;
            }
            
            reloadBarHolder.SetActive(false);
        }
        
        IEnumerator IeReload()
        {
            var reloadCount = 0;
            var reloadInterval = reloadTime / maxAmmo;
            //reloadCount++;
            //ammoImage.sprite = ammoSprites[reloadCount];
            
            while (reloadCount < maxAmmo)
            {
                reloadCount++;
                ammoImage.sprite = ammoSprites[reloadCount];
                yield return new WaitForSeconds(reloadInterval);
            }
            
            ammoAmount = maxAmmo;
            isReloading = false;
            canReload = true;
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
