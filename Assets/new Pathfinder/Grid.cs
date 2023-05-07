using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;

    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, new Vector2Int(x, y));
            }
        }
    }

    public Node GetNearestNode(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public Node[] GetNeighbours(Node node)
    {
        Node[] neighbours = new Node[8];
        Vector2Int nodeGridPosition = node.gridPosition;

        // horizontal and vertical neighbours
        if (nodeGridPosition.x > 0)
        {
            neighbours[0] = grid[nodeGridPosition.x - 1, nodeGridPosition.y];
        }
        if (nodeGridPosition.x < gridSizeX - 1)
        {
            neighbours[1] = grid[nodeGridPosition.x + 1, nodeGridPosition.y];
        }
        if (nodeGridPosition.y > 0)
        {
            neighbours[2] = grid[nodeGridPosition.x, nodeGridPosition.y - 1];
        }
        if (nodeGridPosition.y < gridSizeY - 1)
        {
            neighbours[3] = grid[nodeGridPosition.x, nodeGridPosition.y + 1];
        }

        // diagonal neighbours
        if (nodeGridPosition.x > 0 && nodeGridPosition.y > 0)
        {
            neighbours[4] = grid[nodeGridPosition.x - 1, nodeGridPosition.y - 1];
        }
        if (nodeGridPosition.x < gridSizeX - 1 && nodeGridPosition.y > 0)
        {
            neighbours[5] = grid[nodeGridPosition.x + 1, nodeGridPosition.y - 1];
        }
        if (nodeGridPosition.x > 0 && nodeGridPosition.y < gridSizeY - 1)
        {
            neighbours[6] = grid[nodeGridPosition.x - 1, nodeGridPosition.y + 1];
        }
        if (nodeGridPosition.x < gridSizeX - 1 && nodeGridPosition.y < gridSizeY - 1)
        {
            neighbours[7] = grid[nodeGridPosition.x + 1, nodeGridPosition.y + 1];
        }

        return neighbours;
    }

    public void DrawGrid()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
}
