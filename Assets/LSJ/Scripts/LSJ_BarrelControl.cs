using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSJ_BarrelControl : MonoBehaviour
{
    float previousXPosition, previousYPosition;
    float newXPosition;
    float newYPosition;
 
    [SerializeField]
    Transform cube;
 
    // Start is called before the first frame update
    void Start()
    {
        previousXPosition = Input.mousePosition.x;
    }
 
    private void Update()
    {
        newXPosition = previousXPosition - Input.mousePosition.x;
 
        cube.rotation = Quaternion.Euler(newXPosition, newYPosition, 0);
 
        previousXPosition = Input.mousePosition.x;
    }
}
