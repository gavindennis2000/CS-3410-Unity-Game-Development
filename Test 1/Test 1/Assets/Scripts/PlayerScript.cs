// Gavin Dennis
// 2024 October 27
// Handles player controls, collisions, score, and game over status

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float speed;
    public GameObject explodePrefab;

    public Text countText;
    public static int count;
    public Text gameOverText;
    public static bool gameOver;
    public Button restart;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        speed = 2f;

        count = 0;
        countText.text = "";
        gameOver = false;
        gameOverText.text = "";
        restart.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal") * speed;
        transform.Rotate(0, 0, -speed*h);

        // update count text
        countText.text = "" + count + "";
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Asteroid")) {
            // handle collisions with asteroids

            Instantiate(explodePrefab, transform.position, transform.rotation);  // explosion animation
            gameObject.SetActive(false);

            gameOverText.text = "Game Over";
            restart.gameObject.SetActive(true);
        }
    }

    public void PressRestart() {
        // restarts the game after game over

        SceneManager.LoadScene("SampleScene");
    }
}