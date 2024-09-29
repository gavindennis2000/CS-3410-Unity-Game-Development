// Gavin Dennis
// 9/29/2024
// player controls, timer, and restart stuff

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public Component cam;
    public float speed;
    private int count;
    public Text timerText;
    public Text winText;
    public Text specialText;

    public Button restart;
    public bool gameOver;
    public bool special;  // controls powerup with special pickup
    private float timer;
    private float maxTime = 6.0f;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        count = 0;

        gameOver = false;
        special = false;

        // initialize a timer
        timer = 0.0f;
        timerText.text = "";
        // restart.gameObject.SetActive(false);

        // set win text to a null string
        winText.text = "";

        // set special text to a null string
        specialText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rb.AddForce(cam.transform.forward * v * speed);
        cam.transform.Rotate(new Vector3(0f, 1f, 0f) * h, Space.World);

        // check on the timer and increment if it's not finished
        if (timer < maxTime && !gameOver) {
            timer += Time.deltaTime;
            int seconds = 60 - ((int)timer % 60);
            timerText.text = "Timer: " + seconds.ToString();
        }
        // player has won game
        else if (timer >= maxTime) {
            winText.text = "Game Over!";
            gameOver = true;
            // restart.gameObject.SetActive(true);
        }

    }

    private void OnTriggerEnter(Collider other) {
        // handle collision with generic pickup objects
        if (other.gameObject.CompareTag("Pickup")) {
            other.gameObject.SetActive(false);  // pickup disappears
            count++;

            if (count >= 8) { 
                winText.text = "You Win!";
                gameOver = true;
            }
        }
        // handle collision with special pickup objects
        if (other.gameObject.CompareTag("Special")) {
            other.gameObject.SetActive(false);  // pickup disappears
            specialText.text = "You have super speed!";
            speed *= 3;
            count++;

            if (count >= 8) { 
                winText.text = "You Win!";
                gameOver = true;
            }
        }
        
    }
}

