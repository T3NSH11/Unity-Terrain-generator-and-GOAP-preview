using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public Vector2Int gridPosition;

    public Node(bool walkable, Vector3 worldPosition, Vector2Int gridPosition)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridPosition = gridPosition;
    }
}
