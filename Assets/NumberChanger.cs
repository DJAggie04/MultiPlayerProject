using UnityEngine;

public class NumberChanger : MonoBehaviour
{
    public Sprite[] numbers;
    Sprite sprite;
    public GameObject player;
    RedMove red;
    BlueMove blue;
    SpriteRenderer rend;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = numbers[0];
        if (player.tag == "Player1")
        {
            red = player.GetComponent<RedMove>();
        }
        if(player.tag == "Player2")
        {
            blue = player.GetComponent<BlueMove>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.tag == "Player1")
        {
            if (red.lives == 2)
            {
                rend.sprite = numbers[1];
            }
            if (red.lives == 1)
            {
                rend.sprite = numbers[2];
            }
        }
        if (player.tag == "Player2")
        {
            if (blue.lives == 2)
            {
                rend.sprite = numbers[1];
            }
            if (blue.lives == 1)
            {
                rend.sprite = numbers[2];
            }
        }
        
    }
}
