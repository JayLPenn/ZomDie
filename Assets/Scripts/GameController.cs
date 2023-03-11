using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.Android;

public class GameController : MonoBehaviour
{
    public Camera gameCamera;
    public GUIController guiController;
    public GameObject worldLight;
    public GameObject cloudShader;
    Material cloudShaderMaterial;
    public GameObject rainEmission;
    ParticleSystem rainEmissionParticleSystem;
    public GameObject prefabUnit;
    public GameObject prefabBuilding;
    public GameObject playerUnit;

    public bool gamePaused = false;

    public DateTime currentTime;

    readonly string ipAddressURL = "https://api.ipify.org/";
    private string ipAddress;

    public string weatherRaw;
    public JSONNode weatherInformation;
    public int latitude;
    public int longitude;
    public WeatherType weatherType;
    public int weatherWindSpeed;
    public float weatherWindDegrees = 160f;
    public int sunriseTime;
    public int sunsetTime;

    public enum WeatherType
    {
        Sunny,
        Overcast,
        Drizzling,
        Raining,
        Thunderstorm
    }

    public void PlayerInput()
    {
        // Movement.
        var inputDirX = Input.GetAxisRaw("Horizontal");
        var inputDirY = Input.GetAxisRaw("Vertical");
        playerUnit.GetComponent<Unit>().MoveDirection = new Vector2(inputDirX, inputDirY);

        // Aiming.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var lookDirection = mousePosition - playerUnit.GetComponent<Rigidbody2D>().position;
        playerUnit.GetComponent<Unit>().LookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // Shooting.
        if (Input.GetMouseButtonDown(0)) // (0) is for left hand button.
        {
            playerUnit.GetComponent<Unit>().Shoot();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        guiController = GameObject.Find("GUIController").GetComponent<GUIController>();
        cloudShaderMaterial = cloudShader.GetComponent<SpriteRenderer>().material;
        Debug.Log(cloudShaderMaterial.GetFloat("cloudSize"));
        rainEmissionParticleSystem = rainEmission.GetComponent<ParticleSystem>();

        SetTimeToCurrent();

        playerUnit = Instantiate(prefabUnit, new Vector3(2, 2, 0), Quaternion.identity);
        GenerateBuilding(new Vector2(0, 0), new Vector2(3, 34));

        StartCoroutine(GetWeather());
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle Pause Menu.
        if (Input.GetButtonDown("Cancel"))
        {
            if (gamePaused)
            {
                gamePaused = false;
                guiController.TogglePauseCanvas(false);
            }
            else
            {
                gamePaused = true;
                guiController.TogglePauseCanvas(true);
                guiController.GetPauseMenuValues(); // Sets the values to current values.
            }
        }

        if (!gamePaused)
        {
            SetTimeToCurrent();

            PlayerInput();
            gameCamera.GetComponent<GameCamera>().ChangePosition(new Vector3(playerUnit.transform.position.x, playerUnit.transform.position.y, -10));

        }

    }

    void SetTimeToCurrent()
    {
        currentTime = DateTime.Now;
    }

    void GenerateBuilding(Vector3 location, Vector2 size)
    {
        GameObject building = Instantiate(prefabBuilding, location, Quaternion.identity);
    }

    IEnumerator GetIP()
    {
        UnityWebRequest website = UnityWebRequest.Get(ipAddressURL);

        yield return website.SendWebRequest();

        // Check if there is an error.
        if (website.result == UnityWebRequest.Result.ConnectionError || website.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(website.error);
        }
        else
        {
            ipAddress = website.downloadHandler.text;
            Debug.Log(ipAddress);
        }
    }

    IEnumerator GetWeather()
    {
        //string webAddress = $"https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid=3e8ac582f21905a8fc97a11db83021b7", latitude, longitude;
        string webAddress = "https://api.openweathermap.org/data/2.5/weather?lat=" + latitude + "&lon=" + longitude + "&appid=3e8ac582f21905a8fc97a11db83021b7";
        UnityWebRequest website = UnityWebRequest.Get(webAddress);

        yield return website.SendWebRequest();

        // Check if there is an error.
        if (website.result == UnityWebRequest.Result.ConnectionError || website.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(website.error);
        }
        else
        {
            weatherRaw = website.downloadHandler.text;
            //Debug.Log(weatherRaw);
        }
    }

    void SetWeatherAutomatic()
    {
        weatherInformation = JSON.Parse(weatherRaw);
        var coord = weatherInformation["coord"];
        Debug.Log(coord);
    }

    public void AdjustLatitude(string newLat)
    {
        latitude = int.Parse(newLat);
    }

    public void AdjustLongitude(string newLong)
    {
        longitude = int.Parse(newLong);
    }

    public void AdjustWeatherType(int newState)
    {
        var pSE = rainEmissionParticleSystem.emission;
        weatherType = (WeatherType)newState;

        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

        switch (weatherType)
        {
            case WeatherType.Sunny:
                rainEmission.SetActive(false);
                cloudShader.SetActive(false);
                break;
            case WeatherType.Overcast:
                rainEmission.SetActive(false);
                cloudShader.SetActive(true);
                materialPropertyBlock.SetFloat("cloudSize", 1);
                cloudShader.GetComponent<SpriteRenderer>().SetPropertyBlock(materialPropertyBlock);
                break;
            case WeatherType.Drizzling:
                rainEmission.SetActive(true);
                cloudShader.SetActive(true);
                pSE.rateOverTime = 200;
                materialPropertyBlock.SetFloat("cloudSize", 1);
                cloudShader.GetComponent<SpriteRenderer>().SetPropertyBlock(materialPropertyBlock);
                break;
            case WeatherType.Raining:
                rainEmission.SetActive(true);
                cloudShader.SetActive(true);
                pSE.rateOverTime = 2000;
                cloudShaderMaterial.SetFloat("cloudSize", 2);
                break;
            case WeatherType.Thunderstorm:
                rainEmission.SetActive(true);
                cloudShader.SetActive(true);
                pSE.rateOverTime = 5000;
                materialPropertyBlock.SetFloat("cloudSize", 3);
                cloudShader.GetComponent<SpriteRenderer>().SetPropertyBlock(materialPropertyBlock);
                break;
            default: break;
        }
    }

    public void AdjustWeatherWindSpeed(string newSpeed)
    {
        weatherWindSpeed = int.Parse(newSpeed);
        var pSForce = rainEmissionParticleSystem.forceOverLifetime;

        // Change force depending upon direction.
        if (weatherWindDegrees<= 180)    // Wind is coming from east.
            pSForce.x = -weatherWindSpeed * 100;
        else    // Wind is coming from west.
            pSForce.x = weatherWindSpeed * 100;
    }
    public void AdjustWeatherWindDegrees(float newDegrees)
    {
        weatherWindDegrees = newDegrees;
    }
}