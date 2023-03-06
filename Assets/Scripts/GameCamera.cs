using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public int offsetX = 0;
    public int offsetY = 0;
    public int height = -10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePosition(Vector3 position)
    {
        transform.position = new Vector3(position.x + offsetX, position.y + offsetY, position.z + height);
    }
}
