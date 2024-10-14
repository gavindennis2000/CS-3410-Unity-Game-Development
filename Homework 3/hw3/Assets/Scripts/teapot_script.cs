using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class teapot_script : MonoBehaviour
{
    public PlayerScript playerScript;
    private Rigidbody rb;
    private bool shot = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
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
    }

    private void OnTriggerEnter(Collider other) {
        // handle collision with cannonball
        if (this.gameObject.CompareTag("Special") && other.gameObject.CompareTag("Cannonball")) {
            playerScript.ActivateSpecial();
            shot = true;
            rb.useGravity = true;
        }
        else if (
            other.gameObject.CompareTag("Cannonball") || 
            (other.gameObject.CompareTag("Teapot") && this.gameObject.CompareTag("Teapot"))
        ) {
            if (!shot) {
                playerScript.IncrementScore();
                shot = true;
                rb.useGravity = true;
            }
        }
        else if (other.gameObject.CompareTag("Wall")) {
            rb.useGravity = true;
        }
        else if (other.gameObject.CompareTag("Floor")) {
            if (!shot) {  // make sure the player gets a point if there is a previous issue with collision
                playerScript.IncrementScore();
                shot = true;
            }
            Destroy(this.gameObject);
        }
    }
}