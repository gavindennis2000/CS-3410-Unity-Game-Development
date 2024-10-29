// Gavin Dennis
// 2024 October 27
// handles asteroid spawning

using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class game_controller : MonoBehaviour
{
    public Transform asteroidPrefab;
    private float spawnTime = 1f;  // seconds between asteroid spawns
    private float delay = 1f;  // how long before game starts spawning

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", delay, spawnTime);
    }

    void Spawn() {
        // spawn asteroid prefab

        int side = Random.Range(1,4);  // which side of the screen the asteroid comes from
        float x = Random.Range(-6f, 6f);  // which part of the side
        switch(side) {
            case 1:
                asteroidPrefab.position = new Vector3(x, 6, 0);
                break;
            case 2:
                asteroidPrefab.position = new Vector3(x, -6, 0);
                break;
            case 3:
                asteroidPrefab.position = new Vector3(6, x, 0);
                break;
            default:
                asteroidPrefab.position = new Vector3(-6, x, 0);
                break;
        }
        Instantiate(asteroidPrefab, asteroidPrefab.position, asteroidPrefab.rotation);
    }
}
