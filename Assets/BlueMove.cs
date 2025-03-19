using UnityEngine;

public class BlueMove : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] newSprites; // starting position is 1, other position is 0;
    Transform trans;
    CircleCollider2D circleCol;
    float move = (float)0.15;
    Rigidbody2D rb;
    float jump = (float)20;
    bool grounded = true;
    Vector3 startPos = new Vector3(20, 19, 0);
    int direction;
    int spriteNum;
    int bulletSpeed = 25;
    public float bulletForce;
    int powerCount = 0;
    public int lives;
    AudioSource playerSpeaker;
    public AudioClip[] clips;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        trans.localScale = new Vector3((float)1, (float)1, 1);
        trans.position = startPos;
        rb = GetComponent<Rigidbody2D>();
        spriteNum = 1;
        direction = 1;
        rb.gravityScale = 2;
        tag = "Player2";
        circleCol = GetComponent<CircleCollider2D>();
        circleCol.radius = (float)2.53;
        bulletForce = 20;
        lives = 3;
        playerSpeaker = GetComponent<AudioSource>();
        

    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            spriteNum = 1;
            direction = -(spriteNum * 2 - 1);
            spriteRenderer.sprite = newSprites[spriteNum];
            trans.Translate(new Vector3(move * direction, 0, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            spriteNum = 0;
            direction = -(spriteNum * 2 - 1);
            spriteRenderer.sprite = newSprites[spriteNum];
            trans.Translate(new Vector3(move * direction, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            rb.AddForceY(jump, ForceMode2D.Impulse);
            grounded = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.gravityScale *= 4;
            rb.mass = 3;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            rb.gravityScale = 2;
            rb.mass = 1;
        }
        if (rb.linearVelocityY == 0 && !grounded)
        {
            grounded = true;
            playerSpeaker.PlayOneShot(clips[1]);
        }
        if (trans.position.y < -80)
        {
            trans.position = startPos;
            rb.linearVelocity = new Vector3(0,20, 0);
            lives--;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (powerCount <= 0)
            {
                bulletForce = 20;
            }
            GameObject obj = new GameObject("Bullet");
            obj.AddComponent<AudioSource>();
            obj.AddComponent<Bullet>();
            playerSpeaker.PlayOneShot(clips[0]);
            Sprite bullet = newSprites[2];
            SpriteRenderer bullRend = obj.AddComponent<SpriteRenderer>();
            bullRend.sprite = bullet;
            obj.AddComponent<Rigidbody2D>();
            Rigidbody2D rbBullet = obj.GetComponent<Rigidbody2D>();
            rbBullet.bodyType = RigidbodyType2D.Kinematic;
            obj.AddComponent<CircleCollider2D>();
            CircleCollider2D collider = obj.GetComponent<CircleCollider2D>();
            collider.radius = (float)0.5;
            Transform transBullet = obj.GetComponent<Transform>();
            Bullet bullCode = obj.GetComponent<Bullet>();
            bullCode.SetBulletForce(bulletForce);
            transBullet.position = trans.position + new Vector3((float)3.5 * direction, 0, 0);
            rbBullet.gravityScale = 0;
            rbBullet.linearVelocityX = (float)bulletSpeed * (direction);
            collider.isTrigger = true;
            powerCount--;
        }

    }
    public void SetBulletForce(float force, int count)
    {
        bulletForce = force;
        powerCount = count;
    }

}
