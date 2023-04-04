using System.Collections.Generic;
using System;
using UnityEngine;

public class UnityMainThread : MonoBehaviour
{
    private static UnityMainThread instance;
    private readonly Queue<Action> executionQueue = new Queue<Action>();

    public static UnityMainThread Instance()
    {
        if (!instance)
        {
            instance = FindObjectOfType<UnityMainThread>();
            if (!instance)
            {
                var go = new GameObject("UnityMainThread");
                instance = go.AddComponent<UnityMainThread>();
            }
        }

        return instance;
    }

    private void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue().Invoke();
            }
        }
    }

    public void Enqueue(Action action)
    {
        lock (executionQueue)
        {
            Debug.Log("Enqueue'd action: " + action.ToString());
            executionQueue.Enqueue(action);
        }
    }
}