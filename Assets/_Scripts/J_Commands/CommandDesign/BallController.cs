
using System.Collections.Generic;

public class BallController
{

    InterfaceCommand _onCommand;

    public BallController(InterfaceCommand onCommand)
    {
        _onCommand = onCommand;
    }

    public void BC()
    {
        _onCommand.Execute();

    }

}