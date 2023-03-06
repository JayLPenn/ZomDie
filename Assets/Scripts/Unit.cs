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
        Debug.Log("Current Location: " + transform.position.ToString());


        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector3 bulletSpawnPoint = GameController.GetPositionFromAngle(LookAngle);
        Vector3 bSP2 = transform.position + bulletSpawnPoint - (Vector3)spriteSize;


        var bullet = Instantiate(prefabBullet, bSP2, Quaternion.identity); ;
        Debug.Log("Bullet Spawn Location: " + bullet.transform.position.ToString());
        bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.up *  moveSpeed, ForceMode2D.Impulse);
        return bullet;
    }

    private void UpdateFiringPoint()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        //firingPoint.transform.position = new Vector2(transform.forward.x + spriteSize.x, transform.position.y);
    }
}
