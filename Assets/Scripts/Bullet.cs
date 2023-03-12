using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    Rigidbody2D rigidBody;
    ParticleSystem pSystem;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(transform.right * speed, ForceMode2D.Impulse);

        pSystem = GameObject.Find("EmitterDestruction").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pSE = pSystem.emission;
        pSE.enabled = true;
        pSystem.Play();
        //Destroy(gameObject);
    }
}
