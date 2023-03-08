using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Unit : MonoBehaviour
{
    public GameObject prefabBullet;
    public GameObject fieldOfView;
    public GameObject firingPoint;

    public Rigidbody2D rigidBody;

    public float moveSpeed = 5f;
    public Vector2 firePointRelative = Vector2.zero;

    public Vector2 MoveDirection { get; set; }
    public float LookAngle = 0f;
    public int HealthMax { get; set; }
    public int HealthCurrent { get; set; }

    void Start()
    {
        // Set firing point location.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        firingPoint.transform.position += new Vector3(spriteSize.x, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Move the unit using physics and parsed values.
    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(MoveDirection.x * moveSpeed, MoveDirection.y * moveSpeed);
        rigidBody.rotation = LookAngle;
    }

    public GameObject Shoot()
    {
        var bullet = Instantiate(prefabBullet, firingPoint.transform.position, transform.rotation); ;
        return bullet;
    }
}
