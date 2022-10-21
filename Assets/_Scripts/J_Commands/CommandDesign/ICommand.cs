using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Abstract
public interface InterfaceCommand
{
    void Execute();
    void Undo();
}
