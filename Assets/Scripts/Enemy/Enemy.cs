using System;
using Ash.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngineInternal;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public StateMachine<Enemy> stateMachine;
    public LayerMask visionLayers;
    public Sprite minimapImage;
    private PlayerManager player;
    private SpriteRenderer enemySprite;
    private SpriteRenderer minimapSprite;
    public NavMeshAgent agent;
    
    public float maxAggroRadius;
    public float minAggroRadius;
    public float moveSpeed;
    public float lastAttack = 0;
    public float rateOfAttack;
    
    public int health;

    private Vector3 playerLocation;
    
    private Material matWhite;
    private Material matDefault;
    
    [NonSerialized] public readonly EnemyIdleState enemyIdleState = new EnemyIdleState();
    [NonSerialized] public readonly EnemyMoveState enemyMoveState = new EnemyMoveState();
    [NonSerialized] public readonly EnemyExplodeState enemyExplodeState = new EnemyExplodeState();
    [NonSerialized] public readonly EnemyShootState enemyShootState = new EnemyShootState();
    [NonSerialized] public readonly EnemyAttackState enemyAttackState = new EnemyAttackState();
    [NonSerialized] public readonly EnemyDeathState enemyDeathState = new EnemyDeathState();
    public Vector3 PlayerLocation => playerLocation;
    
    //public UnityEvent OnEnemyDeath;
    
    private void Awake()
    {
        //if(OnEnemyDeath == null) OnEnemyDeath = new UnityEvent();
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = moveSpeed;
        //enemySpawner = FindObjectOfType<EnemySpawner>();
        enemySprite = transform.Find("Enemy_Sprite").GetComponent<SpriteRenderer>();
        minimapSprite = transform.Find("Enemy_Minimap_Sprite").GetComponent<SpriteRenderer>();
        minimapSprite.sprite = minimapImage;
        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material; 
        matDefault = enemySprite.material;
        //OnEnemyDeath.AddListener(enemySpawner.EnemyDead);
        stateMachine = new StateMachine<Enemy>(this);
        SwapState(enemyIdleState);
    }
    
    private void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        //playerLocation = player.transform.position;
        //StartCoroutine(TrackPlayerMovement());
    }

    private void Update()
    {
        stateMachine.Update();
        playerLocation = player.transform.position;
    } 

    private void FixedUpdate() => stateMachine.FixedUpdate();

    #region Customizable Functions

    public virtual void ReactToPlayerInRange()
    {
        
    }

    public virtual void ReactToPlayerLeavingRange()
    {
        
    }

    public virtual void EnemyExplode()
    {
        
    }

    public virtual void EnemyDeath()
    {
        Destroy(gameObject);
    }

    #endregion


    #region Functions for States To Use

    public void SwapState(State<Enemy> enemyState)
    {
        stateMachine.ChangeState(enemyState);
    }
    
    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }
    
    public bool IsPlayerIsInRange()
    {
        var dist = (playerLocation - transform.position).magnitude;
        if (dist <= maxAggroRadius)
        {
            Debug.DrawLine(transform.position, playerLocation, Color.green);
            return true;
        }
            
        Debug.DrawLine(transform.position, playerLocation, Color.red);
        return false;
    }

    public bool IsPlayerInAttackRange()
    {
        var dist = (playerLocation - transform.position).magnitude;
        if (dist <= minAggroRadius)
            return true;

        return false;
    }

    public bool IsPlayerInLineOfSight()
    {
        var dir = playerLocation - transform.position;
       
        RaycastHit hit; 

        if (Physics.Raycast(transform.position, dir.normalized, out hit, Mathf.Infinity, visionLayers))
        {
            Debug.DrawLine(transform.position, hit.point);
            if (hit.transform.CompareTag("Player"))
            {
                return true;
            }

            return false;

        }

        return false;
    }

    #endregion

}
