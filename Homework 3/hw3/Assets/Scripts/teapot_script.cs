using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class teapot_script : MonoBehaviour
{
    public PlayerController playerScript;
    private Rigidbody rb;
    private bool shot = false;
    private bool hitWall = false;
    UnityEngine.Vector3 fakeGravity = new UnityEngine.Vector3(0, -20, 0);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // set the initial rotation angle to random
        var rand = Random.Range(0, 359);
        transform.Rotate(new UnityEngine.Vector3(0, rand, 0));
        
    }

    // Update is called once per frame
    void Update()
    {
        // rotate the teapot on its y axis if it hasn't been shot
        if (!shot) { 
            transform.Rotate(new UnityEngine.Vector3(0, 60, 0) * Time.deltaTime); 
        }
        else if (hitWall) {
            rb.velocity += fakeGravity * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        // handle collision with cannonball
        if (other.gameObject.CompareTag("Cannonball")) {
            if (!shot) {
                shot = true;
                playerScript.count++;
            }
        }
        else if (other.gameObject.CompareTag("Wall")) {
            if (!hitWall) {
                hitWall = true;
            }
        }
        else if (other.gameObject.CompareTag("Floor")) {
            Destroy(this.gameObject);
        }
    }
}