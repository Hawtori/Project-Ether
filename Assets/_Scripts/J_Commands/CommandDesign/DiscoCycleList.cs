using System.Collections.Generic;

//Invoker

public class DiscoCycleList
{
    Stack<InterfaceCommand> _commandList;

    public DiscoCycleList()
    {
        _commandList = new Stack<InterfaceCommand>();
    }

    public void AddCommand(InterfaceCommand newCommand)
    {
        newCommand.Execute();
        _commandList.Push(newCommand);
    }

    public void UndoCommand()
    {
        if(_commandList.Count > 0)
        {
            InterfaceCommand latestCommand = _commandList.Pop();
            latestCommand.Undo();
        }
    }

}