using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Station : MonoBehaviour
{
    private TextMeshProUGUI commText;

    private float timestamp;
    private float cooldown = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        commText = this.transform.Find("Canvas/Text").gameObject.GetComponent<TextMeshProUGUI>();
        commText.SetText("Come find me if you need repairs!");
        timestamp = Time.time + cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timestamp)
        {
            commText.SetText("");
        }
    }

    public void UseStation()
    {
        commText.SetText("Here you go! Good luck!!");
        timestamp = Time.time + cooldown;
    }

    public void FailUseStation()
    {
        commText.SetText("Sorry! You need more cash :(");
        timestamp = Time.time + cooldown;
    }

    public bool IsReady()
    {
        if (Time.time > timestamp) return true;
        else return false;
    }
}
