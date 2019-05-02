using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    // Start is called before the first frame update
    public int health;
    public float speed;
    public Transform wallCheck;
    private bool facingRight = true;
    private SpriteRenderer sprite;
    private Rigidbody2D rb2d;
    private bool touchedWall = false;
    private Animator anim;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        touchedWall = Physics2D.Linecast(transform.position, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (touchedWall)
        {
            Flip();
        }
    }

    void FixedUpdate()
    {
        rb2d.velocity = new Vector2(speed,rb2d.velocity.y);     
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        speed *= -1;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            DamageEnemy();
        }
    }

//retarda um pouco a execusão do que está nela
    IEnumerator DamageEffect()
    {
        float actualSpeed = speed; //velocidade atual
        speed = speed * -1 ;
        anim.SetBool("Died", true);

        sprite.color = Color.red;
        rb2d.AddForce(new Vector2(0f,200f)); //adicionando força no eixo y para o enemy levantar um pouco
        yield return new WaitForSeconds(0.1f); //segundos para o enemy receber o proximo dano
        speed = actualSpeed; //voltando o E para a velocidade normal
        sprite.color = Color.white;
        anim.SetBool("Died", false);

    }

    void DamageEnemy()
    {
        health--;
        StartCoroutine(DamageEffect());
        if (health < 1)
        {
            Destroy(gameObject);
        }
    }

}
