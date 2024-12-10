/*
    Gavin Dennis
    2024/12/10

    Player Script
    handles player head movement, collision checking, and 
    tail instantiation
*/

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    float timer;  // used to dictate when objects move
    string direction;  // used to dictate direction in which player will move
    public GameObject cube;  // cube prefab/snake body
    public GameObject wall;  // the walls of the arena. Used to check for collisions
    List<Transform> snakes;  // list of snake body parts

    void Start()
    {
        timer = 0f;
        direction = "up";

        snakes = new List<Transform>();
        var position = transform.position;
        var rotation = transform.rotation;
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
            Instantiate(wall.transform, pos, wall.transform.rotation);
            // second set of walls
            pos = new Vector3(i, 5f, -105f);
            Instantiate(wall.transform, pos, wall.transform.rotation);
            // third set of walls
            pos = new Vector3(-105f, 5f, i);
            Instantiate(wall.transform, pos, wall.transform.rotation);
            // fourth set of walls
            pos = new Vector3(105f, 5f, i);
            Instantiate(wall.transform, pos, wall.transform.rotation);
        }
    }

    void Update()
    {
        // get input for player movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        
        

        // handle the timer
        if (timer > 0.2f) {
            // reset the timer
            timer = 0f;  

            // configure the player direction
            if ((direction == "up" || direction == "down") && h != 0) {
                // the player can only change direction to left or right when moving up and down
                direction = (h == -1) ? "left" : "right";
            }
            else if ((direction == "left" || direction == "right") && v != 0) {
                // the player can only change direction to left or right when moving up and down
                direction = (v == -1) ? "down" : "up";
            }

            // move all parts of the snake
            MovePlayer();
        
        }
        timer += Time.deltaTime;  // increment the timer
    }

    private void MovePlayer() {
        // moves all player objects including head and every list transform

        // determine new location
        var previousPosition = transform.position;
        var newPosition = previousPosition;

        // set new position for each snake body part, starting with the tail
        for (int i = 0; i < snakes.Count ; i++) {
            newPosition = previousPosition;
            previousPosition = snakes[i].transform.position;
            snakes[i].transform.position = newPosition;
        }

        // set new position for the head
        newPosition = transform.position;
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
        if (    newPosition == cube.transform.position || 
                newPosition == wall.transform.position ) {
            Debug.Log("Game Over:" + cube.transform.position + newPosition);
        }

        // set new position
        transform.position = newPosition;

        
    }
}
