using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusaderPathFinder : PathFinder
{
    
    public new void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePathA",false,SendMessageOptions.DontRequireReceiver);
    }

    
}
