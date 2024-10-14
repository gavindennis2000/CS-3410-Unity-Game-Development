using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public Transform spawnPoint;
    public GameObject cannonPrefab;
    private float cannonSpeed;
    public bool special;
    private int i = 0;  // iterator for powerup

    // Start is called before the first frame update
    void Start()
    {
        cannonSpeed = 7;
        special = false;
    }

    // Update is called once per frame
    void Update()
    {
        // increment iterator
        i++;

        // detect spacebar input for shooting
        if ((special && i % 20 == 0) | Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
    }

    public void Shoot() {
        // shoots cannonball towards teapots
        var bullet = Instantiate(cannonPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * cannonSpeed;
    }

    public void ActivateSpecial() {
        special = true;
    }
}
