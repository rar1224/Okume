using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private PickupType pType;
    private Rigidbody2D rigidbody;
    private SpriteRenderer renderer;
    public Sprite[] sprites;
    private float timeStamp;

    public PickupType PType { get => pType; set => pType = value; }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        int number = Random.Range(0, 3);
        renderer.sprite = sprites[number];
        PType = (PickupType)number;

        int spin = Random.Range(-100, 100);
        rigidbody.AddTorque(spin);

        float movex = Random.Range(-1.0f, 1.0f);
        float movey = Random.Range(-1.0f, 1.0f);
        Vector2 move = new Vector2(movex, movey);
        rigidbody.AddForce(move * 200);

        timeStamp = Time.time + 10f;
    }

    public void SetMovement(Vector2 move)
    {
        rigidbody.AddForce(move * 200);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeStamp)
        {
            Destroy(this.gameObject);
        }
    }

    public enum PickupType
    {
        Money = 0,
        Repair = 1,
        Weapon = 2
    }
}
