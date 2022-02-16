using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int Damage;
    [SerializeField] private Vector2 Knockback;
    [SerializeField] private Vector2 FireForce;

    public void Fire(int xMultiplier)
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(FireForce.x * xMultiplier, FireForce.y), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<IHealth>().TakeDamage(Damage, Knockback);
        }
        Destroy(gameObject);   
    }
}
