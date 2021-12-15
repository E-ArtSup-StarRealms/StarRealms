using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{

    public GameObject hand;
    public bool inHand;
    public bool released;
    public GameObject parent;
    private float currentTime;
    public float timer;
    public Vector3 origin;
    public Vector3 releaseLoc;
    public bool held;
    public AnimationCurve curve;
    public bool overEngage;
    public GameObject cardTopEngage;
    public GameObject engage;


    void Start()
    {
        
    }

    void Update()
    {

        engage= GameObject.Find("");

        cardTopEngage = GameObject.Find("CardTopEngaged");
        hand = GameObject.Find("Hand");
        parent = transform.parent.gameObject;
     
        
        if(hand == parent)
        {
            inHand = true;
        }
        else
        {
            inHand = false;

        }


        if (inHand && released && held )
        {
            if(overEngage)
            {
                releaseLoc = transform.position;
                currentTime += Time.deltaTime;
                float percent = currentTime / timer;

                transform.position = Vector3.Slerp(releaseLoc, cardTopEngage.transform.position, curve.Evaluate(percent));

                if (percent >= 1)
                {
                    currentTime = 0;
                    held = false;
                    released = false;
                    this.transform.SetParent(engage.transform);

                    // this.transform.SetParent(hand.transform);

                }
            }
            else
            {
                releaseLoc = transform.position;
                currentTime += Time.deltaTime;
                float percent = currentTime / timer;

                transform.position = Vector3.Slerp(releaseLoc, origin, curve.Evaluate(percent));

                if (percent >= 1)
                {
                    currentTime = 0;
                    held = false;
                    released = false;

                    // this.transform.SetParent(hand.transform);

                }
            }
            
        }
    }
    private void OnMouseDown()
    {
        if (inHand)
        {
         origin = transform.position;
       
        }
    }

    private void OnMouseDrag()
    {
        held = true;
        if (inHand)
        {
            transform.position = new Vector3(transform.position.x + Input.GetAxis("Mouse X"), transform.position.y + Input.GetAxis("Mouse Y"), transform.position.z);

        }
    }


    private void OnMouseUp()
    {
        if (inHand)
        {

            released = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "Engaged")
        {
            overEngage = true;
          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Engaged")
        {
            overEngage = false;
          
        }
    }
}
