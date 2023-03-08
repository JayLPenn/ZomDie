using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionRain : MonoBehaviour
{
    Camera mainCamera;
    ParticleSystem pSystem;
    Vector2 screensize;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        pSystem = GetComponent<ParticleSystem>();

        transform.position = mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 1.25f));  // Place weather above camera and center it.
        var pShape = pSystem.shape;
        pShape.enabled = true;
        pShape.scale = new Vector3(900f, 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
