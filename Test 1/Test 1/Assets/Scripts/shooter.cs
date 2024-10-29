// Gavin Dennis
// 2024 October 27
// handles bullet instantiation

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
    }

    void Shoot() {
        // shoot bullet in direction of player
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
