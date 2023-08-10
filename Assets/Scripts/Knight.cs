using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class Knight : MonoBehaviour
{

    public float walkSpeed = 3f;
    public float walkStopRate = 0f;
    public float distToChasePlayer = 8f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    Animator animator;
    Rigidbody2D rb;
    Damagable damagable;
    Transform target;
    Vector2 moveDirection;
    private Vector2 walkDirectionVector = Vector2.right;    
    public enum walkableDirection {Right, Left};
    private walkableDirection _walkDirection;    
    TouchingDirections _touchingDirections;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damagable = GetComponent<Damagable>();
    }
    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        //if player sprite is detected in enemy detectionzone attack
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        //check if there is a player to target, enemy can move, and enemy is alive, and close to player
        if (target && CanMove && damagable.CharacterAlive && (Vector3.Distance(transform.position, target.transform.position) < distToChasePlayer))
        {
            CloseToPlayer = true;
            //set direction for enemy to go towards player
            rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * walkSpeed;
            Vector2 direction = new Vector2(target.position.x - transform.position.x, transform.position.y).normalized;
            //move towards player
            moveDirection = direction;
            Debug.Log("Moving towards player");
        }
        else
        {
            CloseToPlayer = false;
        }
        if (_touchingDirections.IsOnWall && _touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        //flip enemy sprite in direction of player
        FlipSprite();
        //if (!damagable.LockVelocity)
        //{
        //    if (CanMove)
        //    {
        //        rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        //    }
        //    else
        //    {
        //        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        //    }
        //}
    }
    public walkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            //if walk direction is not already right or left respectively
            if (_walkDirection != value)
            {
                //flip direction
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == walkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;

                }
                else if (value == walkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            else
            {
                _walkDirection = value;
            }
        }
    }
    public bool _hasTarget = false;
    public bool HasTarget 
    {
        get
        {
            return _hasTarget;
        } 
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    } 
    public float AttackCooldown 
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        } 
    }
    //bool for if character can move
    public bool _canMove = true;
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    private void FlipDirection()
    {       
        if (WalkDirection == walkableDirection.Right)
        {
            WalkDirection = walkableDirection.Left;
        }
        else if (WalkDirection == walkableDirection.Left)
        {
            WalkDirection = walkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkableDirection is not set to legal values of right or left");
        }                      
    }
    private void FlipSprite()
    {
        if (transform.position.x > target.transform.position.x && damagable.CharacterAlive)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }
    public bool CloseToPlayer
    {
        get
        {
            return animator.GetBool(AnimationStrings.closeToPlayer);
        }
        set
        {
            animator.SetBool(AnimationStrings.closeToPlayer, value);
        }
    }
    public void OnHit(int damage, Vector2 Knockback)
    {        
        rb.velocity = new Vector2(Knockback.x, rb.velocity.y + Knockback.y);
    }
    public void OnCliffDetected()
    {
        if (_touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
