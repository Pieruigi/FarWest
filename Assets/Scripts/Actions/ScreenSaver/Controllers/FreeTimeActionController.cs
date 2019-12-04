using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class FreeTimeActionController : MonoBehaviour
{
    public virtual void ActionEnterStart(FreeTimeAction action) { }

    public virtual void ActionEnterCompleted(FreeTimeAction action) { }

    public virtual void ActionLoopStart(FreeTimeAction action, int loopId) { }

    public virtual void ActionLoopCompleted(FreeTimeAction action, int loopId) { }

    public virtual void ActionExitStart(FreeTimeAction action) { }

    public virtual void ActionExitCompleted(FreeTimeAction action) { }

    public virtual void ActionMessage(string message) { }
}
