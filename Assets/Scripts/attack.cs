using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{

    Collider2D attackCollider;
    public int attackDamage = 10;
    public Vector2 Knockback = Vector2.zero;

    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //see if it can be hit
        Damagable damageable = collision.GetComponent<Damagable>();
        if (damageable != null)
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? Knockback : new Vector2(-Knockback.x, Knockback.y);
            bool gothit = damageable.Hit(attackDamage, deliveredKnockback);
            if (gothit)
            {
                Debug.Log($"{collision.name} hit for {attackDamage}");
            }
        }
    }
}
