using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController2D : SimpleEntity, IHealth
{
    [SerializeField] private UIHadler UIHadler;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform BarsHolder;

    [SerializeField] private int Hp;
    [SerializeField] private Image HpBar;

    [SerializeField] private float MoveSpeed;
    [SerializeField] private float SmoothTime;
    [SerializeField] private float JumpForce;

    [SerializeField] private float FireCoolDown;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private GameObject BulletPref;
    [SerializeField] private Image ChargeBar;

    [SerializeField] private Transform FeetPos;
    [SerializeField] private float GroundCheckRadius;
    [SerializeField] private LayerMask GroundLayer;

    private int CurrentHp;
    private float InputX;
    private Vector2 CurrentVelocity;
    private bool Jump;
    private bool Grounded;
    private float FireCoolDownCounter;

    [SerializeField] private bool BlockMovement;

    private void Start()
    {
        CurrentHp = Hp;
    }

    private void Update()
    {
        if (FireCoolDownCounter > 0)
        {
            FireCoolDownCounter -= Time.deltaTime;
            ChargeBar.fillAmount = 1 - (FireCoolDownCounter / FireCoolDown);
        }

        InputX = Input.GetAxis("Horizontal");
        if ((InputX < 0 && facingRight) || (InputX > 0 && !facingRight))
            Flip();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump = true;

        if (Input.GetKeyDown(KeyCode.Mouse0) && FireCoolDownCounter <= 0)
        {
            GameObject bullet = Instantiate(BulletPref, FirePoint.position, FirePoint.rotation);
            bullet.GetComponent<Bullet>().Fire(facingRight ? 1 : -1);
            FireCoolDownCounter = FireCoolDown;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            UIHadler.Win();
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Grounded = Physics2D.OverlapCircle(FeetPos.position, GroundCheckRadius, GroundLayer);
        if (BlockMovement && rb.velocity.y <= 0)
            BlockMovement = false;

        if (!BlockMovement)
        {
            Vector2 targetVelocity = new Vector2(InputX * MoveSpeed, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref CurrentVelocity, SmoothTime);
        }

        if (Jump && (Grounded || BlockMovement))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            Jump = false;

            BlockMovement = false;
        }

        Jump = false;
    }

    public override void Flip()
    {
        base.Flip();
        BarsHolder.eulerAngles = Vector3.zero;
    }

    public void TakeDamage(int damage, Vector2 knockBack)
    {
        CurrentHp -= damage;
        HpBar.fillAmount = (float)CurrentHp / (float)Hp;

        rb.velocity = Vector2.zero;
        rb.AddForce(knockBack, ForceMode2D.Impulse);

        if (CurrentHp <= 0)
        {
            enabled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        BlockMovement = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(FeetPos.position, GroundCheckRadius);
    }
}
