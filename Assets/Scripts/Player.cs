using UnityEngine;

public class Player : MonoBehaviour
{
    public float thrustSpeed;
    public float turnSpeed;
    public float cooldown = 0.5f;

    public Bullet bulletPrefab;

    private new Rigidbody2D rigidbody;

    private bool thrust = false;
    private float turn = 0.0f;
    private float timestamp;
    private float pickupCooldown;
    private bool pickupEnabled = false;

    public Game game;

    private bool visitingNow = false;
    private bool atStation = false;
    private Planet visitingPlanet;
    private Mission currentMission;
    private Station visitingStation;

    private int money;
    private int health;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        health = 100;
        money = 0;
    }

    private void Update()
    {
        if (health <= 0)
        {
            game.SetEndText();
            thrust = false;
            turn = 0;
            return;
        }

        if (pickupEnabled == true && Time.time > pickupCooldown)
        {
            pickupEnabled = false;
            cooldown = 0.5f;
        }
        
        thrust = Input.GetKey(KeyCode.W);

        if (Input.GetKey(KeyCode.A))
        {
            turn = 1.0f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            turn = -1.0f;
        }
        else
        {
            turn = 0.0f;
        }

        if (Input.GetKey(KeyCode.Space) && Time.time >= timestamp)
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.E) && visitingNow && game.availableMission != null)
        {
            if (currentMission == null && visitingPlanet == game.availableMission.start)
            {
                // start mission

                currentMission = game.availableMission;
                game.StartMission();
            }
            else if (currentMission != null && visitingPlanet == currentMission.destination)
            {
                // end mission

                money += currentMission.price;
                game.UpdateMoney(money);

                currentMission = null;
                game.FinishMission();
            }
            
        } else if (Input.GetKey(KeyCode.E) && atStation && visitingStation.IsReady())
        {
            if (money < 50)
            {
                visitingStation.FailUseStation();
            }
            else
            {
                money = money - 50;
                health = 100;
                game.UpdateMoney(money);
                game.UpdateHealth(health);
                visitingStation.UseStation();
            }
        }

    }

    private void FixedUpdate()
    {
        if (thrust)
        {
            rigidbody.AddForce(this.transform.up * thrustSpeed);
        }

        if (turn != 0.0f)
        {
            rigidbody.AddTorque(turn * turnSpeed);
        }
    }

    public void RestartPlayer()
    {
        this.transform.position = new Vector3(0, 0, 0);
        this.transform.rotation = Quaternion.identity;
        money = 0;
        health = 100;
    }

    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
        timestamp = Time.time + cooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Planet")
        {
            visitingPlanet = other.gameObject.GetComponent<Planet>();
            visitingNow = true;
        } else if (other.gameObject.tag == "Station")
        {
            visitingStation = other.gameObject.GetComponent<Station>();
            atStation = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Planet")
        {
            visitingNow = false;
        } else if (other.gameObject.tag == "Station")
        {
            atStation = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "Asteroid")
            {
                health -= 10;
                game.UpdateHealth(health);
            }

       else if (collision.gameObject.tag == "Pickup")
        {
            Destroy(collision.gameObject);
            Pickup.PickupType pType = collision.gameObject.GetComponent<Pickup>().PType;
            if (pType == Pickup.PickupType.Money)
            {
                money += 20;
                game.UpdateMoney(money);
            } else if (pType == Pickup.PickupType.Repair)
            {
                health += 30;
                game.UpdateHealth(health);

            } else if (pType == Pickup.PickupType.Weapon)
            {
                pickupCooldown = Time.time + 20;
                pickupEnabled = true;
                cooldown = 0.1f;
            }
        }
    }

}
