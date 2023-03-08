using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.PlayerLoop.PreLateUpdate;

public class Clock : MonoBehaviour
{
    private int nextUpdate;

    public GameObject bigHand;
    public GameObject smallHand;
    GameController gameController;
    DateTime currentTime;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        SetTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;   // Update time to next update.

            SetTime();
        }

        
    }

    void SetTime()
    {
        currentTime = gameController.currentTime;
        bigHand.transform.eulerAngles = new Vector3(0, 0, -(360 / 60) * currentTime.Minute);
        smallHand.transform.eulerAngles = new Vector3(0, 0, -(360 / 12) * currentTime.Hour);
    }
}
