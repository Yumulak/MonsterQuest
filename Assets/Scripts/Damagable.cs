using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;

public class Damagable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; 
    Animator animator;
    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }
    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            //if health < 0 character characterAlive = false
            if(value <= 0)
            {
                CharacterAlive = false;

            }
        }
    }
    [SerializeField]
    private bool _characterAlive = true;
    public bool CharacterAlive 
    {
        get
        {
            return _characterAlive;
        } 
        private set
        {
            _characterAlive = value;
            animator.SetBool(AnimationStrings.characterAlive, value);
            Debug.Log($"ow! CharacterAlive: {value} ");
        } 
    }
    private bool _isInvincible = false;
    public bool IsInvincible
    {
        get
        {
            return _isInvincible;
        }
        set
        {
            _isInvincible = value;
        }
    }
    private float TimeSinceHit = 0;
    public float InvincibilityTime = .25f;
  
    private void Awake()
    {
            animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (_isInvincible)
        {
            if(TimeSinceHit > InvincibilityTime)
            {
                IsInvincible = false;
                TimeSinceHit = 0;
            }
            TimeSinceHit += Time.deltaTime;
        }        
    }
    //velocity should not be changed while this is true but needs to be respected by other physics components like the player controller
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }
    public bool Hit(int damage, Vector2 Knockback)
    {
        if(CharacterAlive && !IsInvincible)
        {
            Health -= damage;
            IsInvincible = true;
            //notify other subscribed components that hte damageable was hit to handle the knockback
            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, Knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            //hit
            return true;
        }
        else
        {
            //unable to be hit
            return false;
        }
    }  
}
