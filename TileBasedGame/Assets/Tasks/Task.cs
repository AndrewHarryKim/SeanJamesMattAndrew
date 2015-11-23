using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class Task
{
    private bool active = false;
    public Task() { }

    public virtual void OnEnter() { }
    public bool update()
    {
        if (!active)
        {
            active = true;
            OnEnter();
        }
        return OnUpdate();
    }
    public virtual bool OnUpdate() { return true; }
    public virtual void OnExit() { }
}