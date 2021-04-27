using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public int collisionCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        collisionCount += 3;
        Debug.Log("This works");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("This works");
    }

    void OnTriggerEnter(Collider other){
        collisionCount += 1;
        Debug.Log("plz work");
    }
}
