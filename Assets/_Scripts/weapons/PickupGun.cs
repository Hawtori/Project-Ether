using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is a tempporary script just for debugging and testing purposes
//will probably have this implemented antoher way later
public class PickupGun : MonoBehaviour
{
    public Camera camera;
    public Transform leftPosition, rightPosition;

    private RaycastHit hit;

    private int gunLayer = 8;

    private void Update()
    {
        int bitMask = 1 << gunLayer;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Input.GetKeyDown(KeyCode.H))
            Debug.DrawRay(camera.transform.position, ray.direction * 20f, Color.yellow, 5f);

        if (Input.GetKeyDown(KeyCode.E) && Physics.Raycast(camera.transform.position, ray.direction, out hit, 20f, bitMask))
        {
            //disown right children
            if (rightPosition.transform.childCount > 0)
            {
                for (int i = 0; i < rightPosition.transform.childCount; i++)
                {
                    Transform child = rightPosition.transform.GetChild(i);
                    child.parent = null;
                    child.position = new Vector3(8f, 0.5f, 5f);
                }
            }

            hit.transform.parent = rightPosition.transform;

            if (leftPosition.transform.childCount > 0)
            {
                for (int i = 0; i < leftPosition.transform.childCount; i++)
                {
                    Transform child = leftPosition.transform.GetChild(i);
                    child.parent = null;
                    child.position = new Vector3(8f, 0.5f, 5f);
                }
            }

            rightPosition.transform.GetChild(0).transform.localPosition = Vector3.zero;
            rightPosition.transform.GetChild(0).transform.localRotation = Quaternion.identity;
            rightPosition.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
            //rightPosition.transform.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
            //rightPosition.transform.GetChild(0).GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.Q) && Physics.Raycast(camera.transform.position, ray.direction, out hit, 10f, bitMask))
        {
            if (leftPosition.transform.childCount > 0)
            {
                for (int i = 0; i < leftPosition.transform.childCount; i++)
                {
                    Transform child = leftPosition.transform.GetChild(i);
                    child.parent = null;
                    child.position = new Vector3(8f, 0.5f, 5f);
                }
            }

            hit.transform.parent = leftPosition.transform;

            if (rightPosition.transform.childCount > 0)
            {
                for (int i = 0; i < rightPosition.transform.childCount; i++)
                {
                    Transform child = rightPosition.transform.GetChild(i);
                    child.parent = null;
                    child.position = new Vector3(8f, 0.5f, 5f);
                }
            }

            leftPosition.transform.GetChild(0).transform.localPosition = Vector3.zero;
            leftPosition.transform.GetChild(0).transform.localRotation = Quaternion.identity;

        }
    }
}
