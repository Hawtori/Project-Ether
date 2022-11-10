public class DoSomethingCommand : InterfaceCommand
{
    //Concrete Command
    DiscoBall _discoball;

    public DoSomethingCommand(DiscoBall discoBall)
    {
        _discoball = discoBall;
    }

    public void Execute()
    {
        _discoball.wow();
    }

    public void Undo()
    {

    }
}