using UnityEngine;
public class ChangeColorCommand : InterfaceCommand
{

    DiscoBall _discoBall;
    Color _prevColor;


    public ChangeColorCommand(DiscoBall discoBall)
    {
        _discoBall = discoBall;
        _prevColor = discoBall.GetComponent<Renderer>().material.color;
    }

    public void Execute()
    {
        _discoBall.SetRandomColor();
    }

    public void Undo()
    {
        _discoBall.GetComponent<Renderer>().material.color = _prevColor;
    }
}