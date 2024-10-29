// Gavin Dennis
// 2024 October 27
// handles asteroid movement

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroid_script : MonoBehaviour
{
    private float speed = 0.3f;
    private Rigidbody2D rb2d;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        // destroy asteroid after 6 seconds to prevent memory clog
        Destroy(gameObject, 6f);  

        // send asteroids towards player
        rb2d.velocity = -speed * Vector2.Lerp(transform.position, player.transform.position,Time.deltaTime);
    }
}
