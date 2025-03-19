using TMPro;
using UnityEngine;
using Unity.Netcode;
using System.Globalization;
using Unity.Netcode.Components;
using UnityEngine.UIElements;

public class BlueMoveNetwork : NetworkBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] newSprites;

    CircleCollider2D circleCol;
    Transform trans;
    float move = 0.15f;
    Rigidbody2D rb;
    float jump = 20f;
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

    // Reference to NetworkTransform
    private NetworkTransform networkTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!IsOwner) return; // Ensure only the owner controls the object

        spriteRenderer = GetComponent<SpriteRenderer>();
        trans = GetComponent<Transform>();
        networkTransform = GetComponent<NetworkTransform>();  // Initialize NetworkTransform for sync
        trans.localScale = new Vector3(1, 1, 1);
        trans.position = startPos;
        rb = GetComponent<Rigidbody2D>();
        spriteNum = 1;
        direction = -1;
        rb.gravityScale = 2;
        tag = "Player2";
        circleCol = GetComponent<CircleCollider2D>();
        circleCol.radius = 2.53f;
        bulletForce = 20f;
        lives = 3;
        playerSpeaker = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return; // Ensure only the owner controls the movement

        HandleMovement();
        HandleActions();
    }

    // Handle movement inputs and translation
    private void HandleMovement()
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

        // Notify server to perform jump action
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            JumpServerRpc();
        }

        // Modify gravity while pressing DownArrow
        if (Input.GetKeyDown(KeyCode.S))
        {
            rb.gravityScale *= 4;
            rb.mass = 2;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            rb.gravityScale = 2;
            rb.mass = 1;
        }

        // Handle grounded state and play sound when landing
        if (rb.linearVelocity.y == 0 && !grounded)
        {
            grounded = true;
            playerSpeaker.PlayOneShot(clips[1]);
        }

        // Reset position if the player falls too low
        if (trans.position.y < -80)
        {
            ResetPosition();
        }
    }

    // Handle shooting and other actions
    private void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (powerCount <= 0)
            {
                bulletForce = 20;
            }
            ShootBulletServerRpc();
        }
    }

    // ServerRpc to make the jump action
    [ServerRpc]
    private void JumpServerRpc()
    {
        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        grounded = false;
    }

    // ServerRpc to spawn and shoot bullets
    [ServerRpc]
    private void ShootBulletServerRpc()
    {
        // Create Bullet
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
        collider.radius = 0.5f;
        Transform transBullet = obj.GetComponent<Transform>();
        Bullet bullCode = obj.GetComponent<Bullet>();
        bullCode.SetBulletForce(bulletForce);
        transBullet.position = trans.position + new Vector3(3.5f * direction, 0, 0);
        rbBullet.gravityScale = 0;
        rbBullet.linearVelocity = new Vector2(bulletSpeed * direction, 0);
        collider.isTrigger = true;
        powerCount--;
    }

    // Reset position and decrement lives if the player falls too low
    private void ResetPosition()
    {
        trans.position = startPos;
        rb.linearVelocity = new Vector2(0, 20);
        lives--;
    }

    // Set bullet force and power count
    public void SetBulletForce(float force, int count)
    {
        bulletForce = force;
        powerCount = count;
    }
}
