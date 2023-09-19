using UnityEngine;
using TMPro;
using System;

public class Planet : MonoBehaviour
{
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI missionText;
    Renderer renderer;
    
    public bool hasMission = false;
    private float timestamp;
    private float cooldown = 2.0f;

    public Renderer Renderer { get => renderer; set => renderer = value; }

    void Start()
    {
        nameText  = this.transform.Find("Canvas/NameText").gameObject.GetComponent<TextMeshProUGUI>();
        missionText = this.transform.Find("Canvas/MissionText").gameObject.GetComponent<TextMeshProUGUI>();

        nameText.SetText(this.name);
        Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!hasMission && Time.time >= timestamp)
        {
            RemoveMissionText();
        }
    }

    public void SetMissionText(string text)
    {
        missionText.SetText(text);
        hasMission = true;
    }

    public void SetMissionStartText()
    {
        missionText.SetText("Thanks! Let's go!!");
        hasMission = false;
        timestamp = Time.time + cooldown;
    }

    public void SetMissionEndText(int price)
    {
        missionText.SetText("Here's your " + price + "! Bye!");
        hasMission = false;
        timestamp = Time.time + cooldown;
    }

    public void RemoveMissionText()
    {
        missionText.SetText("");
        hasMission = false;
    }


}
