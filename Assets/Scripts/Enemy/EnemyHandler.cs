using System;
using System.Collections;
using System.Collections.Generic;
using Ash.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private PlayerManager player;
    [SerializeField] private SpriteRenderer spr;
    [SerializeField] private GameObject enemyDeathPrefab;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private float maxRadius;
    [SerializeField] private float minRadius;
    [SerializeField] private LayerMask visionLayers;
    public BaseRoom spawnedFromRoom;
    public StateMachine<EnemyHandler> stateMachine;
    [SerializeField] private GameObject projectilePrefab;
    private float lastShot = 0;
    [SerializeField] private float rateOfFire;
    [SerializeField] private int ammoAmount;
    [SerializeField] private GameObject reloadUI;
    public float moveSpeed;
    [SerializeField] private int health;
    private EnemySpawner enemySpawner;
    
    public UnityEvent OnEnemyDeath;

    private int maxAmmo;
    private bool isReloading = false;
    private bool isShooting = false;

    private Material matWhite;
    private Material matDefault;

    //[NonSerialized] public readonly EnemyIdleState enemyIdleState = new EnemyIdleState();
    //[NonSerialized] public readonly EnemyShootState enemyShootState = new EnemyShootState();
    [NonSerialized] public readonly EnemyMoveState enemyMoveState = new EnemyMoveState();

    public float GetMaxRadius => maxRadius;
    public float GetMinRadius => minRadius;

    public bool IsReloading => isReloading;

    public bool IsShooting
    {
        get => isShooting;
        set => isShooting = value;
    }

    public float GetRateOfFire => rateOfFire;
    public float GetLastShot => lastShot;

    public float GetAmmoCount => ammoAmount;
    
    public Vector3 GetPlayerPosition => player.transform.position;
    
    private void Awake()
    {
        if(OnEnemyDeath == null) OnEnemyDeath = new UnityEvent();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        OnEnemyDeath.AddListener(enemySpawner.EnemyDead);
        maxAmmo = ammoAmount;
        player = FindObjectOfType<PlayerManager>();
        stateMachine = new StateMachine<EnemyHandler>(this);
        stateMachine.ChangeState(enemyMoveState);
    }

    private void Start()
    {
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material; 
        matDefault = spr.material;
    }

    private void Update() => stateMachine.Update();
    private void FixedUpdate() => stateMachine.FixedUpdate();



    //takedamage

    //die
    
    //public bool CheckLineOfSightToPlayer()
    //{
    //    var dir = GetPlayerPosition - transform.position;
    //   
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, dir.normalized, dir.magnitude, visionLayers);
    //   
    //    if (hit)
    //    {
    //        Debug.DrawLine(transform.position, hit.point);
    //        if (hit.transform.CompareTag("Player"))
    //        {
    //            return true;
    //        }
//
    //        return false;
    //    }
//
    //    
    //    return false;
    //}

    //public float CheckDistanceToPlayer()
    //{
    //    var dist = player.transform.position - transform.position;
    //    return dist.magnitude;
    //}
    
    //public void FireWeapon(Vector3 dir, float rot)
    //{
    //    UpdateAmmo();
    //    var offset = Random.Range(-12, 12);
    //    rot += offset;
    //    //Instantiate(shellPrefab, startPosition, quaternion.identity);
    //    Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, rot));
    //    //shakeTimer = .25f;
    //    lastShot = Time.time;
    //}
//
    //public void Reload()
    //{
    //    isReloading = true;
    //    reloadUI.SetActive(true);
    //    ammoAmount = 0;
    //    
    //    StartCoroutine(IeReload());
//
    //    IEnumerator IeReload()
    //    {
    //        int reloadCount = 0;
    //        while (reloadCount < maxAmmo)
    //        {
    //            yield return new WaitForSeconds(.33f);
    //            reloadCount++;
    //        }
//
    //        ammoAmount = maxAmmo;
    //        isReloading = false;
    //        reloadUI.SetActive(false);
    //    }
    //}
    //
    //private void UpdateAmmo()
    //{
    //    if (ammoAmount > 0)
    //    {
    //        ammoAmount--;
    //    }
    //}
    
    public void Move(Vector2 velocity)
    {
        rigidbody2D.MovePosition(rigidbody2D.position + velocity * Time.fixedDeltaTime);
    }

    private void TakeDamage(int dmgAmount)
    {
        health -= dmgAmount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //spawnedFromRoom.SpawnKey();
        var chance = Random.value;
        if (chance <= .2f)
        {
            Instantiate(ammoPrefab, transform.position, Quaternion.identity);
        }
        GameObject obj = Instantiate(enemyDeathPrefab, transform.position, Quaternion.identity);
        Destroy(obj, 1.25f);
        Destroy(gameObject);
        OnEnemyDeath.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            TakeDamage(collision.GetComponent<Projectile>().damageAmount);
            Destroy(collision.gameObject);
            
            spr.material = matWhite;
            
            Invoke("SwapMaterialToDefault", .1f);
        }
    }
    
    private void SwapMaterialToDefault()
    {
        spr.material = matDefault;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minRadius);
    }
}
