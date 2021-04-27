using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionDetection : MonoBehaviour
{

    public Vector3 closestCheckPoint;
    public int collisionCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Checkpoint")){
            collisionCount += 1;
            closestCheckPoint = other.transform.position;
        }
    }
}
