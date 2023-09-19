using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Game : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
    public GameObject planets;
    public GameObject textBox;
    public GameObject endTextBox;
    public GameObject pointer;
    public GameObject asteroids;

    private TextMeshProUGUI screenText;
    private TextMeshProUGUI endText;
    private Renderer pointerRenderer;
    private Camera camComponent;
    private float cameraHeight;
    private float cameraWidth;

    private string moneyText = "";
    private string healthText = "";

    public Mission availableMission;
    private bool pointVisible;
    private Planet target = null;
    private float timestamp;

    public Asteroid asteroidPrefab;

    static float margin = 0.5f;
    static int asteroidCount = 100;

    // Start is called before the first frame update
    void Start()
    {
        pointerRenderer = pointer.GetComponent<Renderer>();
        screenText = textBox.GetComponent<TextMeshProUGUI>();
        endText = endTextBox.GetComponent<TextMeshProUGUI>();

        UpdateMoney(0);
        UpdateHealth(100);

        for (int i = 0; i < asteroidCount; i++)
        {
            Vector3 position = new Vector3(Random.Range(-100, 100), Random.Range(-50, 50), 0);
            Asteroid asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity, asteroids.transform);
        }
        CreateRandomMission();

        camComponent = camera.GetComponent<Camera>();
        cameraHeight = camComponent.orthographicSize;
        cameraWidth = cameraHeight * camComponent.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (availableMission == null && Time.time > timestamp)
        {
            CreateRandomMission();
        }

        camera.transform.Translate(player.transform.position.x - camera.transform.position.x, player.transform.position.y - camera.transform.position.y, 0);

        if (target != null && !target.Renderer.isVisible)
        {
            pointVisible = true;
        } else
        {
            pointVisible = false;
        }

        if (!pointVisible)
        {
            pointerRenderer.enabled = false;
        } else
        {
            pointerRenderer.enabled = true;
            Vector3 direction = player.transform.position - target.transform.position;

            float x_difference = Mathf.Abs(direction.x) - cameraWidth;
            float y_difference = Mathf.Abs(direction.y) - cameraHeight;

            if (x_difference < y_difference)
            {
                float y;

                if (direction.y < 0) y = cameraHeight - margin;
                else y = -cameraHeight + margin;

                float x = direction.x * y / direction.y;
                
                pointer.transform.localPosition = new Vector3(x, y, 0);
            }
            else
            {
                float x;

                if (direction.x < 0) x = cameraWidth - margin;
                else x = -cameraWidth + margin;

                float y = direction.y * x / direction.x;
                

                pointer.transform.localPosition = new Vector3(x, y, 0);
            }
        }

        if (asteroids.transform.childCount < asteroidCount)
        {
            Vector3 position = new Vector3(Random.Range(-100, 100), Random.Range(-50, 50), 0);
            if ((player.transform.position - position).magnitude > cameraHeight + cameraWidth)
            {
                Asteroid asteroid = Instantiate(asteroidPrefab, position, Quaternion.identity, asteroids.transform);
            }
        }

        if (Input.GetKey(KeyCode.Return) && Time.time > timestamp) RestartGame();
    }

    void CreateRandomMission()
    {
        int planetCount = planets.transform.childCount;

        int startIndex = Random.Range(0, planetCount);
        int destIndex = Random.Range(0, planetCount);

        while (startIndex == destIndex)
        {
            destIndex = Random.Range(0, planetCount);
        }

        Planet startPlanet = planets.transform.GetChild(startIndex).gameObject.GetComponent<Planet>();
        Planet destPlanet = planets.transform.GetChild(destIndex).gameObject.GetComponent<Planet>();

        availableMission = new Mission(startPlanet, destPlanet, 100);
        target = availableMission.start;
    }

    public void SetWaitingTime()
    {
        timestamp = Time.time + 2.0f;
    }

    public void StartMission()
    {
        availableMission.StartMission();
        target = availableMission.destination;
    }

    public void FinishMission()
    {
        availableMission.EndMission();
        availableMission = null;
        SetWaitingTime();
        target = null;
    }
    
    public void UpdateMoney(int money)
    {
        moneyText = "Money: " + money;
        screenText.SetText(moneyText + healthText);
    }

    public void UpdateHealth(int health)
    {
        healthText = "\nHealth: " + health;
        screenText.SetText(moneyText + healthText);
    }

    public void RestartGame()
    {
        availableMission = null;
        int number = asteroids.transform.childCount;
        for (int i = 0; i < number; i++)
        {
            Destroy(asteroids.transform.GetChild(i).gameObject);
        }
        UpdateMoney(0);
        UpdateHealth(100);
        player.GetComponent<Player>().RestartPlayer();
        SetWaitingTime();
        ClearEndText();
    }

    public void ClearEndText()
    {
        endText.SetText("");
    }

    public void SetEndText()
    {
        endText.SetText("Oh no!\n\nrestart | enter");
    }

}
