using UnityEngine;
using Component = System.ComponentModel.Component;

public class SelfCompoment<T> where T : Component
{
    private T save;
    private GameObject self;
    public delegate T CompomentGet(GameObject self);
    CompomentGet Action;
    public SelfCompoment(GameObject gameObject,CompomentGet action)
    {
        Action = action;
        self = gameObject;
    }
    
    public T Get()
    {
        if (save == null) save = Action.Invoke(self);
        return save;
    }
}