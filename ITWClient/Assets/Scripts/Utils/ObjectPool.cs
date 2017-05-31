using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool<T> where T : Behaviour
{
    public int RemainCount { get { return stack.Count; } }
    private Stack<T> stack = new Stack<T>();
    
    public void Add(T instance)
    {
        stack.Push(instance);
    }

    public T Get()
    {
        if(stack.Count == 0)
        {
            throw new UnityException("Remain object not found.");
        }

        return stack.Pop();
    }
}