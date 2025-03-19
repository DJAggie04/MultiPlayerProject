using Unity.VisualScripting;
using UnityEngine;

public class PowerUp2_0 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    bool on;
    SpriteRenderer sprite;
    int timeInterval;
    int rand;
    void Start()
    {
        on = true;
        sprite = GetComponent<SpriteRenderer>();
        rand = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        timeInterval = (int)(Time.time % (rand * 5));
        if (timeInterval == 0)
        {
            on = true;
        }
        if (!on)
        {
            sprite.color = new Color(1f, 1f, 1f, 0f);
        }
        else
        {
            sprite.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player2" && on)
        {
            on = false;
            GameObject player1 = other.gameObject;
            BlueMove pl = player1.GetComponent<BlueMove>();
            pl.SetBulletForce(50, 1);
            rand = Random.Range(1, 5);
        }
        if (other.gameObject.tag == "Player1" && on)
        {
            on = false;
            GameObject player1 = other.gameObject;
            RedMove pl = player1.GetComponent<RedMove>();
            pl.SetBulletForce(50, 1);
            rand = Random.Range(1, 5);
        }

    }
}