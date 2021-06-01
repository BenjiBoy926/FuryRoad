using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCenter : MonoBehaviour
{
    public void ObstacleCenterMove(){
        transform.position = new Vector3(transform.position.x, 0 , transform.position.z);
    }
}
