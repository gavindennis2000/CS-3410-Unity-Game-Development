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

    // prefab lists
    public GameObject cube;  // cube prefab/snake body
    public GameObject wall;  // the walls of the arena. Used to check for collisions
    List<Transform> snakes;  // list of snake body parts
    List<Transform> walls;  // list of all wall objects

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
            if (!gameOver) {
                MovePlayer();
                canChangeDirection = true;
            }
        
        }

        // increment the timer
        timer += Time.deltaTime;  
    }

    private void MovePlayer() {
        // moves all player objects including head and every list transform

        // set new position for each snake body part, starting with the tail
        for (int i = snakes.Count-1; i > 0 ; i--) {
            snakes[i].transform.position = snakes[i-1].transform.position;
        }

        // set new position for the snake head
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

        // set new position
        transform.position = newPosition;

        
    }

    private void GameOver() {
        // handles game over: stops player movement, and enables retry button

        gameOver = true;
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

    }

    public void ClickRetry() {
        // on click handler for retry button

        // restart the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
