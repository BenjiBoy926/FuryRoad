using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwampManager : MonoBehaviour
{
    private bool movingDown = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y >= 25 || movingDown == true){
            movingDown = true;
            transform.Translate(Vector3.down * Time.deltaTime);
        }
        

        if(transform.position.y <= 0 || movingDown == false){
            movingDown = false;
            transform.Translate(Vector3.up * Time.deltaTime);
        }
    }
}
