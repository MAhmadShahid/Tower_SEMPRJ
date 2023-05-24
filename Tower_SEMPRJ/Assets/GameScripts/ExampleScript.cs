using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{

    [Header("Reference")]
    public PlayerController _myPlayerController;
    // Start is called before the first frame update
    void Start()
    {
        if (_myPlayerController == null)
            Debug.Log("Reference passed successfully"); 
        else 
            Debug.Log("Reference passing failed");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
