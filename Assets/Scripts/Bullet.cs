using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 400.0f;
    public float maxLifetime = 10.0f;
    private Rigidbody2D rigidbody;
    private Vector2 direction;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        this.direction = direction;
        rigidbody.AddForce(direction * this.speed);
        Destroy(this.gameObject, this.maxLifetime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);

        if (collision.gameObject.tag == "Asteroid")
        {
            collision.gameObject.GetComponent<Asteroid>().Explode(direction);
            Destroy(collision.gameObject);
        }
    }
}
