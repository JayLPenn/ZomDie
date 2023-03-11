using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public GameController gameController;

    public GameObject gameCanvas;
    public GameObject pauseCanvas;

    public GameObject weatherAutomaticDropdown;
    public GameObject latitudeInput;
    public GameObject longitudeInput;
    public GameObject weatherTypeDropdown;
    public GameObject windSpeedInput;
    public GameObject windDegreeInput;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        //GetPauseMenuValues();
    }

    // Update is called once per frame
    void Update()
    {
        //SetPauseMenuValues();
    }

    public void TogglePauseCanvas(bool pauseCanvasState)
    {
        pauseCanvas.SetActive(pauseCanvasState);
    }

    public void GetPauseMenuValues()
    {
        windDegreeInput.GetComponent<Slider>().value = gameController.weatherWindDegrees;
        //latitudeInput.GetComponent<InputField>().text = gameController.latitude.ToString();
    }
    public void SetPauseMenuValues()
    {
        gameController.weatherWindDegrees = (int)(windDegreeInput.GetComponent<Slider>().value * 360);
        //gameController.SetWeatherManual();
    }
}
