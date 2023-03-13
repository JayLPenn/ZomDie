using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    public GameObject pauseCanvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePauseCanvas(bool pauseCanvasState)
    {
        pauseCanvas.SetActive(pauseCanvasState);
    }
}
