// Gavin Dennis
// 2024 October 27
// Handles bullet movement and collisions

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{

    public float speed = 10f;
    public Rigidbody2D rb;
    public GameObject explodePrefab;

    // Start is called before the first frame update
    void Start()
    {
        // set speed and direction of bullet
        rb = GetComponent<Rigidbody2D>();
        // bullet moves upward relative to the player's direction
        rb.velocity = transform.up * speed;

        // destroy bullet after 6 seconds to prevent memory clog
        Destroy(gameObject, 6f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Asteroid")) {
            // handle collision with asteroids

            Instantiate(explodePrefab, other.transform.position, transform.rotation);  // explosion animation
            other.gameObject.SetActive(false);
            gameObject.SetActive(false);
            PlayerScript.count++;
        }
    }
}
