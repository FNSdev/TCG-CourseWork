using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DraggingActionTest : MonoBehaviour
{
    public abstract void OnStartDragging();
    public abstract void OnEndDragging();
    public abstract void OnDraggingInUpdate();

    public virtual bool CanDrag
    {
        get
        {
            return true;
        }
    }

    protected abstract bool DragSuccesful();
}
