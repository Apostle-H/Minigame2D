using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticEnemy : SimpleEntity, IHealth
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private int Hp;
    [SerializeField] private int Damage;
    [SerializeField] private Vector2 Knockback;

    [SerializeField] private Transform BarsHolder;
    [SerializeField] private Image HpBar;

    private int CurrentHp;

    private void Start()
    {
        CurrentHp = Hp;
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        CurrentHp -= damage;
        HpBar.fillAmount = (float)CurrentHp / (float)Hp;

        rb.velocity = Vector2.zero;
        rb.AddForce(knockback, ForceMode2D.Impulse);

        if (CurrentHp <= 0)
        {
            BarsHolder.gameObject.SetActive(false);
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CurrentHp > 0 && collision.gameObject.CompareTag("Player"))
        {
            SimpleEntity player = collision.gameObject.GetComponent<SimpleEntity>();

            if ((transform.position.x <= collision.transform.position.x && player.facingRight) || (transform.position.x >= collision.transform.position.x && !player.facingRight))
            {
                player.Flip();
            }

            collision.gameObject.GetComponent<IHealth>().TakeDamage(Damage, new Vector2(Knockback.x * (player.facingRight ? -1 : 1), Knockback.y));
        }
    }

    public override void Flip()
    {
        base.Flip();
        BarsHolder.eulerAngles = Vector3.zero;
    }
}
