using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mask : MonoBehaviour
{
    public GameObject[] Object;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log((this.GetComponent<Transform>().parent.parent.childCount) - 1);
        for (int j =0; j < ((this.GetComponent<Transform>().parent.parent.childCount) - 1);j++)
        {
            //Debug.Log(this.GetComponent<Transform>().parent.parent.GetChild(j).gameObject);
            Object[j] = this.GetComponent<Transform>().parent.parent.GetChild(j).gameObject;
            //Debug.Log(Object[j]);
        }
       
        for(int i = 0; i < ((this.GetComponent<Transform>().parent.parent.childCount) - 1) ; i++)
        {
            Object[i].GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
