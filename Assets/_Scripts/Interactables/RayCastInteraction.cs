/*
 * REDO THIS WITH VIDEO FROM: https://www.youtube.com/watch?v=zgCV26yFAiU
 * 
 * My own implementation is really down bad 
 * 
 * Modify it to be tagged based still
 * 
 */




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastInteraction : MonoBehaviour
{

    private GameObject lastHit;
    private Vector3 lastCollide;

    public GameObject pickupTransform;
    public bool HoldingItem = false;
    public GameObject heldObject;
    
    //Last hit needs to be cleared after X time or after X happens because its stored and can be fired from afar
    void rayCasting()
    {
        var ray = new Ray(this.transform.position, this.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 6.0f)){
           // Debug.Log(hit.collider.gameObject.name);
            Debug.DrawLine(ray.origin, hit.point, new Color(25,25,0));
            lastHit = hit.collider.gameObject;
            lastCollide = hit.point;
          
        }

    }

    void rayCastDoor()
    {
        //NOT DRY. Too hard coded. Will always look for door buttons and only do this.
        //Requires the door to be a child to button.
        if (lastHit.tag == "Button" && lastHit.transform.GetChild(0).tag == "Door" && Input.GetKeyDown(KeyCode.E))
        {
            print("This is a button. You can press it.");
            lastHit.GetComponent<Renderer>().material.color = Color.blue;

            //Only moves 1 frame when button is pressed
            lastHit.transform.GetChild(0).transform.position += EnvironmentReact.openDoorInstant(Vector3.up, 2);

        }
    }

    //Colliding the held object with another has weird interactions. 
    //Must have rigidbody and physics tag
    void rayCastItems()
    {
        if (lastHit.tag == "PhysicsItem" && Input.GetKeyDown(KeyCode.V))
        {

            heldObject = lastHit;
            print("This is a physics Item. You can pick it up.");
            heldObject.GetComponent<Renderer>().material.color = Random.ColorHSV();
            HoldingItem = !HoldingItem;
        }

        if (HoldingItem && lastHit.tag == "PhysicsItem")
        {
            heldObject.transform.position = pickupTransform.transform.position;
        //    heldObject.GetComponent<Rigidbody>().useGravity = false;
            heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            heldObject.GetComponent<Collider>().enabled = false;
        }
        else
        {
            heldObject.GetComponent<Rigidbody>().useGravity = true;
            heldObject.GetComponent<Collider>().enabled = true;

        }

    }


    // Start is called before the first frame update
    void Start()
    {

        //So the console doesn't yell at me
        lastHit = new GameObject("TempLastHit");
        heldObject = new GameObject("TempHeldObject");
        heldObject.AddComponent<Rigidbody>();
        heldObject.AddComponent<Collider>(); 
    }

    // Update is called once per frame
    void Update()
    {

        rayCasting();
        rayCastDoor();
        rayCastItems();
    }
}
