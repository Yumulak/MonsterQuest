using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class Knight : MonoBehaviour
{

    public float walkSpeed = 3;
    public float walkStopRate = 0.2f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    Animator animator;
    Rigidbody2D rb;
    Damagable damagable;

    public enum walkableDirection {Right, Left};

    private walkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    TouchingDirections _touchingDirections;

    public walkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if(_walkDirection != value)
            {
                //flip direction
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if(value == walkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;

                }
                else if (value == walkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
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
    public bool _canMove = true;
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damagable = GetComponent<Damagable>();
    }
    private void FixedUpdate()
    {
        if (_touchingDirections.IsOnWall && _touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
        if(!damagable.LockVelocity)
        {
            if (CanMove)
            {
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }        
    }
    private void FlipDirection()
    {
        if(WalkDirection == walkableDirection.Right)
        {
            WalkDirection = walkableDirection.Left;
        }
        else if(WalkDirection == walkableDirection.Left)
        {
            WalkDirection = walkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkableDirection is not set to legal values of right or left");
        }
    }
    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        if(AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
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
