using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aStarNode  
{
    public bool walkable;
    public Vector3 worldPosition;

    public aStarNode(bool _walkable, Vector3 _worldPos)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
    }
}
