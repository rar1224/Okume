using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour
{
    public Pickup pickupPrefab;

    private Rigidbody2D rigidbody;
    private SpriteRenderer renderer;
    public Sprite[] sprites;
    private bool small = false;

    public bool Small { get => small; set => small = value; }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        int number = Random.Range(0, 3);
        renderer.sprite = sprites[number];

        int spin = Random.Range(-100, 100);
        rigidbody.AddTorque(spin);

        float movex = Random.Range(-1.0f, 1.0f);
        float movey = Random.Range(-1.0f, 1.0f);
        Vector2 move = new Vector2(movex, movey);
        rigidbody.AddForce(move * 200);
    }

    void SetMovement(Vector2 move)
    {
        rigidbody.AddForce(move * 200);
    }

    public void Explode(Vector2 direction)
    {
        if (!small)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Random.Range(0, 6) == 0)
                {
                    Pickup pickup = Instantiate(pickupPrefab, this.gameObject.transform.position, Quaternion.identity);
                    pickup.SetMovement(direction);
                    direction = Quaternion.Euler(120, 0, 0) * direction;

                } else
                {
                    Asteroid asteroid = Instantiate(this, this.gameObject.transform.position, Quaternion.identity, this.transform.parent);
                    asteroid.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    asteroid.small = true;
                    asteroid.SetMovement(direction);
                    direction = Quaternion.Euler(120, 0, 0) * direction;
                }
            }
        }
    }

}
