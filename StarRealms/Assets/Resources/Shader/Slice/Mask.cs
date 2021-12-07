using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public GameObject[] Object;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < Object.Length; i++)
        {
            Object[i].GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
