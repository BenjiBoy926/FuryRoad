using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCenterManager : MonoBehaviour
{
    private bool obstacleFinished = false;
    private ObstacleCenter obstacle;

    private void OnTriggerEnter(Collider other){

        if (!obstacleFinished){
            obstacle.ObstacleCenterMove();
            obstacleFinished = true;
        }
    }
}
