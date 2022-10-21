using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : InterfaceCommand
{

    DiscoBall _thingToMove;
    Vector3 _prevPosition;

    public MoveCommand(DiscoBall thingToMove)
    {
        _thingToMove = thingToMove;
        _prevPosition = thingToMove.GetComponent<Transform>().transform.position;

    }

    public void Execute()
    {
        _thingToMove.setClickPosition();
    }

    public void Undo()
    {
        _thingToMove.transform.position = _prevPosition;
    }



}
