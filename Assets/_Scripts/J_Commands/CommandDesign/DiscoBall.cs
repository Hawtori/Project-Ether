using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBall : MonoBehaviour
{

    public DiscoBall _woahBall;
    DiscoCycleList _discocyclelist;
    DiscoPositionList _discopositionlist;


    void Start()
    {

        _discocyclelist = new DiscoCycleList();
        _discopositionlist = new DiscoPositionList();
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            InterfaceCommand changeColorCommand = new ChangeColorCommand(_woahBall);
            _discocyclelist.AddCommand(changeColorCommand);


        }

        if (Input.GetMouseButtonDown(0))
        {
            InterfaceCommand changePositionCommand = new MoveCommand(_woahBall);
            _discopositionlist.AddCommand(changePositionCommand);

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _discocyclelist.UndoCommand();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _discopositionlist.UndoCommand();
        }
    }


    public void wow()
    {
        print("Wow");
    }

    public void SetRandomColor()
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }

    public void setClickPosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GetComponent<Transform>().position = hit.point;
                Debug.Log("Reached");
                Debug.DrawLine(Camera.main.transform.position, hit.point);
            }
        }



    }


}
