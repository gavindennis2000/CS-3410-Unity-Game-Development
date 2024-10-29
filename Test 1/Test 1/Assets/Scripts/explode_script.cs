// Gavin Dennis
// 2024 October 27
// Handles player controls, collisions, score, and game over status

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explode_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2f);  // destroy after 2 seconds
    }
}
