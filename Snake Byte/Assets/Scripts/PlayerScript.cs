/*
    Gavin Dennis
    2024/12/10

    Player Script
    handles player head movement, collision checking, and 
    tail instantiation
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    // movement stuff
    float timer;  // used to dictate when objects move
    string direction;  // used to dictate direction in which player will move
    Boolean canChangeDirection;  // used to let the player change direction between movements

    // prefabs and prefab lists
    public GameObject cube;  // cube prefab/snake body
    public GameObject wall;  // the walls of the arena. Used to check for collisions
    public GameObject apple;  // apples that make the snake grow score increase
    List<Transform> snakes;  // list of snake body parts
    List<Transform> walls;  // list of all wall objects

    // sound effects
    public AudioSource biteApple, zap;

    // game functionality
    Boolean gameOver;  // set to true when player hits a wall or body part
    public Text gameOverText;
    public Button retryButton;
    
    private int score;
    public Text scoreText;

    void Start()
    {
        // initialize movement stuff
        timer = 0f;
        direction = "up";
        canChangeDirection = true;

        // initalize canvas stuff
        gameOver = false;
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        score = 0;
        scoreText.text = "Score: " + score;

        // initialize the lists for snake and wall transforms
        snakes = new List<Transform>();
        walls = new List<Transform>();

        // position and rotation for snake body parts
        var position = transform.position;
        var rotation = transform.rotation;

        snakes.Add(transform);  // add self to list
        position.z -= 10;  // increment body cube position
        snakes.Add(Instantiate(cube.transform, position, rotation));  // create first body cube
        position.z -= 10;  // increment body cube position
        snakes.Add(Instantiate(cube.transform, position, rotation));  // create first body cube
        position.z -= 10;  // increment body cube position
        snakes.Add(Instantiate(cube.transform, position, rotation));  // create first body cube
        
        // instantiate the walls
        for (float i = -105f; i <= 105f; i += 10f) {
            // first set of walls
            var pos = new Vector3(i, 5f, 105f);
            walls.Add(Instantiate(wall.transform, pos, rotation));
            // second set of walls
            pos = new Vector3(i, 5f, -105f);
            walls.Add(Instantiate(wall.transform, pos, rotation));
            // third set of walls
            pos = new Vector3(-105f, 5f, i);
            walls.Add(Instantiate(wall.transform, pos, rotation));
            // fourth set of walls
            pos = new Vector3(105f, 5f, i);
            walls.Add(Instantiate(wall.transform, pos, rotation));
        }

        // move the apple to a random spot
        MoveApple();
    }

    void Update()
    {
        // update the score
        scoreText.text = "Score: " + score;

        // get input for player movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        // configure the player's direction
        if (canChangeDirection) {
            if ((direction == "up" || direction == "down") && h != 0) {
                // the player can only change direction to left or right when moving up and down
                direction = (h == -1) ? "left" : "right";
                canChangeDirection = false;
            }
            else if ((direction == "left" || direction == "right") && v != 0) {
                // the player can only change direction to left or right when moving up and down
                direction = (v == -1) ? "down" : "up";
                canChangeDirection = false;
            }
        }

        // handle the timer
        if (timer > 0.2f) {
            // reset the timer
            timer = 0f;  

            // move all parts of the snake
            MovePlayer();
            canChangeDirection = true;
        
        }

        // increment the timer
        timer += Time.deltaTime;  
    }

    private void MoveApple() {
        // creates an apple at a random location; cannot be the same location
        // as any part of the snake; must be 5 spaces away from the head

        float posX, posZ;
        double distance;  // distance between moved apple and snake head
        Boolean colliding = false;  // flags if moved apple is touching snake
        Vector3 newPosition;

        // find a new position where there isn't snake body
        do {
            // random x and z positions
            posX = -95 + UnityEngine.Random.Range(0, 19) * 10; 
            posZ = -95 + UnityEngine.Random.Range(0, 19) * 10; 

            // position vector
            newPosition = new Vector3(posX, 5, posZ);

            // check collision with each body part
            foreach (var snake in snakes) {
                if (newPosition == snake.position) {
                    colliding = true; }
            }

            // get the distance from the head
            var head = snakes.First().position;
            distance = ( 
                Math.Sqrt(
                    Math.Pow(newPosition.x - head.x, 2) + 
                    Math.Pow(newPosition.z - head.z, 2)
                )
            );
            Debug.Log(distance);

        } while (colliding || distance < 50);

        // move the apple
        apple.transform.position = newPosition;
    }

    private void EatApple() {
        // handles apple consumption

        score++;  // increment score
        biteApple.Play();  // play the sound effect
        MoveApple();  // move apple

        // create 3 new snake parts
        var tailPosition = snakes.Last().position;
        var rotation = transform.rotation;
        for (int i = 0; i < 3; i++) { 
            snakes.Add(Instantiate(cube.transform, tailPosition, rotation)); }
    }

    private void MovePlayer() {
        // moves all player objects including head and every list transform

        // find new position for the snake head
        // (the body will move first but we need to check for collisions)
        var newPosition = transform.position;
        switch (direction) {
            case "up":
            default:
                newPosition.z += 10;
                break;
            case "down":
                newPosition.z -= 10;
                break;
            case "left":
                newPosition.x -= 10;
                break;
            case "right":
                newPosition.x += 10;
                break;
        }

        // check for collisions that would prompt a game over
        for (int i = 1; i < snakes.Count; i++) {
            // check snake collision
            if (newPosition == snakes[i].position) { 
                GameOver(); }
        }
        foreach (Transform wall in walls) {
            // check wall collision
            if (newPosition == wall.position) { 
                GameOver(); }
        }

        // exit the function if there's a game over
        if (gameOver) { 
            return;
        }

        // set new position for each snake body part, starting with the tail
        for (int i = snakes.Count-1; i > 0 ; i--) {
            snakes[i].transform.position = snakes[i-1].transform.position;
        }

        // set new position
        transform.position = newPosition;

        // check for apple collision
        if (transform.position == apple.transform.position) {
            EatApple();
        }
        
    }

    private void GameOver() {
        // handles game over: stops player movement, and enables retry button

        // enable the text/buttons
        gameOver = true;
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        // play the sound effect
        zap.Play();  
    }

    public void ClickRetry() {
        // on click handler for retry button

        // restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
