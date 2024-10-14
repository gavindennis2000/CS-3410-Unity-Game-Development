// Gavin Dennis
// 9/29/2024
// player controls, timer, and restart stuff

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;
    public Component cam;
    public float speed;
    public int count;
    public Text timerText;
    public Text countText;
    public Text winText;
    public Text specialText;

    public Button restart;
    public bool gameOver;
    private float timer;
    private float maxTime = 30.0f;
    public Shooter shooterScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        count = 0;

        gameOver = false;
        speed = 2;

        // initialize a timer
        timer = 0.0f;
        timerText.text = "Timer: ";
        countText.text = "Count: ";
        restart.gameObject.SetActive(false);

        // set win text to a null string
        winText.text = "";

        // set special text to a null string
        specialText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // configure player movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(h, v, 0.0f) * speed;

        // check on the timer and increment if it's not finished
        if (timer < maxTime && !gameOver) {
            timer += Time.deltaTime;
            int seconds = 30 - ((int)timer % 60);
            timerText.text = "Timer: " + seconds.ToString();
            countText.text = "Remaining: " + (10-count);
        }
        // player has won game
        else if (timer >= maxTime) {
            countText.text = "";
            specialText.text = "";
            winText.text = "Game Over!";
            gameOver = true;
            restart.gameObject.SetActive(true);
        }

    }

    public void IncrementScore() {
        count++;
        if (count >= 10) { 
            countText.text = "";
            specialText.text = "";
            winText.text = "You Win!";
            gameOver = true;
            restart.gameObject.SetActive(true);
        }
    }

    public void ActivateSpecial() {
        // activate rapidfire when destroying the golden pot
        specialText.text = "Power-up activated!";
        if (shooterScript == null) { Debug.Log("shooterscript is null"); }
        shooterScript.special = true;
        Debug.Log("powerup activated");
    }

    public void PressRestart() {
        // restarts the game after game over or win
        SceneManager.LoadScene("SampleScene");
    }
}

