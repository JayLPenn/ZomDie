using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight : MonoBehaviour
{
    private int nextUpdate;

    GameController gameController;
    Light2D lightComp;
    DateTime sunriseTime;
    DateTime[] sunriseTimeRange = new DateTime[2];
    DateTime sunsetTime;
    DateTime[] sunsetTimeRange = new DateTime[2];

    DateTime currentTime;

    enum DayState
    {
        Sunrise = 0,
        Day = 1,
        Sunset = 2,
        Evening = 3
    }
    DayState currentDayState = DayState.Sunrise;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        lightComp = GetComponent<Light2D>();
        currentTime = DateTime.Now;
        sunriseTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 6, 0, 0);
        sunsetTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 18, 0, 0);

        sunriseTimeRange[0] = sunriseTime.AddMinutes(-15);
        sunriseTimeRange[1] = sunriseTime.AddMinutes(15);

        sunsetTimeRange[0] = sunsetTime.AddMinutes(-15);
        sunsetTimeRange[1] = sunsetTime.AddMinutes(15);

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;   // Update time to next update.

            SetTimeOfDay();
        }
    }
    public void SetLight(Color lColour, float lIntensity)
    {
        lightComp.color = lColour;
        lightComp.intensity = lIntensity;
    }

    public void SetTimeOfDay()
    {
        currentTime = gameController.currentTime;

        if (currentTime >= sunriseTimeRange[0] && currentTime <= sunriseTimeRange[1])
            currentDayState = DayState.Sunrise;
        else if (currentTime > sunriseTimeRange[1] && currentTime < sunsetTimeRange[0])
            currentDayState = DayState.Day;
        else if (currentTime >= sunsetTimeRange[0] && currentTime <= sunsetTimeRange[1])
            currentDayState = DayState.Sunset;
        else if (currentTime > sunsetTimeRange[1])
            currentDayState = DayState.Evening;
        else
            currentDayState = DayState.Day;

        switch (currentDayState)
        {
            case DayState.Sunrise:
                SetLight(Color.red, 0.5f);
                break;
            case DayState.Day:
                SetLight(Color.white, 1f);
                break;
            case DayState.Sunset:
                SetLight(Color.magenta, 0.5f);
                break;
            case DayState.Evening:
                SetLight(Color.blue, 0.5f);
                break;
        }
    }
}
