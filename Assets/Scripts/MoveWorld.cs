using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWorld : MonoBehaviour
{
    
    private float worldSpeed = -15f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 0, worldSpeed*GameManager.instance.GetWorldSpeedModifier()) * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DestroyTrigger"))
        {
            Destroy(gameObject);
        }
    }
}
