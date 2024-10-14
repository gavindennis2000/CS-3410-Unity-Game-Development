using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter : MonoBehaviour
{

    public Transform spawnPoint;
    public GameObject cannonPrefab;
    private float cannonSpeed;
    // Start is called before the first frame update
    void Start()
    {
        cannonSpeed = 7;
    }

    // Update is called once per frame
    void Update()
    {
        // detect spacebar input for shooting
        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
    }

    void Shoot() {
        // shoots cannonball towards teapots
        var bullet = Instantiate(cannonPrefab, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * cannonSpeed;
    }
}
