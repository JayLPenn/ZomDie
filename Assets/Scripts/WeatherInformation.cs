using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeatherInformation
{
    public string[] coord;
    //public string type;
    //public float windSpeed;
    //public float windDegrees;
    //public int sunriseTime;
    //public int sunsetTime;

    public static WeatherInformation CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<WeatherInformation>(jsonString);
    }
}
