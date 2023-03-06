using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera gameCamera;
    public GameObject prefabUnit;
    public GameObject prefabBuilding;
    public GameObject playerUnit;

    public void PlayerInput()
    {
        // Movement.
        var inputDirX = Input.GetAxisRaw("Horizontal");
        var inputDirY = Input.GetAxisRaw("Vertical");
        playerUnit.GetComponent<Unit>().MoveDirection = new Vector2(inputDirX, inputDirY);

        // Aiming.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var lookDirection = mousePosition - playerUnit.GetComponent <Rigidbody2D>().position;
        playerUnit.GetComponent<Unit>().LookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        // Shooting.
        if(Input.GetMouseButtonDown(0)) // (0) is for left hand button.
        {
            playerUnit.GetComponent<Unit>().Shoot();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerUnit = Instantiate(prefabUnit, new Vector3(2, 2, 0), Quaternion.identity);
        GenerateBuilding(new Vector2(0, 0), new Vector2(3, 34));
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        gameCamera.GetComponent<GameCamera>().ChangePosition(new Vector3(playerUnit.transform.position.x, playerUnit.transform.position.y, -10));
    }

    void GenerateBuilding(Vector3 location, Vector2 size)
    {
        GameObject building = Instantiate(prefabBuilding, location, Quaternion.identity);
    }

    // Converts and angle into a Vector3.
    public static Vector3 GetPositionFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
