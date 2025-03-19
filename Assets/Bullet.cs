using UnityEngine;

public class Bullet : MonoBehaviour
{
    Transform trans;
    int direction = 0;
    Rigidbody2D rb;   
    float bulletForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trans.position.x < -95 || trans.position.x > 95)
        {
            Destroy(this.gameObject);
        }
        if (direction == 0)
        {
            if(rb.linearVelocityX > 0)
            {
                direction = 1;
            }
            if(rb.linearVelocityX < 0)
            {
                direction = -1;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2" || collision.gameObject.tag == "Platform")
        {
            if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
            {
                Rigidbody2D rbCharacter = collision.gameObject.GetComponent<Rigidbody2D>();
                rbCharacter.AddForceX(bulletForce * direction, ForceMode2D.Impulse);
            }
            Destroy(this.gameObject);
        }
    }

    public void SetBulletForce(float force)
    {
        bulletForce = force;
    }
}


