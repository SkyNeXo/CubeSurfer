using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        float newYRotation = currentRotation.y + (-30 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(currentRotation.x, newYRotation, currentRotation.z);
    }
}
