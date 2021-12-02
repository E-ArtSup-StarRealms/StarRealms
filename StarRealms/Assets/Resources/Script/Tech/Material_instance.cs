using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_instance : MonoBehaviour
{
    // Start is called before the first frame update
    public Material material;
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
