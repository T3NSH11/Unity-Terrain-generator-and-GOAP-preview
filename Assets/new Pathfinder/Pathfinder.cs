using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Grid grid;

    void Start()
    {
        grid = GetComponent<Grid>();
    }

    public Stack<Vector3> FindPath(Vector3 originPos, Vector3 targetPos)
    {
        Node originNode = grid.GetNearestNode(originPos);
        Node targetNode = grid.GetNearestNode(targetPos);

        if (originNode == targetNode || IsAdjacent(originNode, targetNode))
        {
            Stack<Vector3> path = new Stack<Vector3>();
            path.Push(targetPos);
            path.Push(originPos);
            return path;
        }

        HashSet<Node> closedList = new HashSet<Node>();
        Dictionary<Node, int> gCosts = new Dictionary<Node, int>();
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();
        PriorityQueue<Node> openList = new PriorityQueue<Node>();

        gCosts[originNode] = 0;
        parents[originNode] = null;
        openList.Enqueue(originNode, FindHcost(originNode, targetNode));

        while (openList.Count > 0)
        {
            Node currentNode = openList.Dequeue();

            if (currentNode == targetNode)
            {
                return GetPath(originNode, targetNode, parents);
            }

            closedList.Add(currentNode);

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                int newGcost = gCosts[currentNode] + GetGcost(currentNode, neighbour);

                if (!gCosts.ContainsKey(neighbour) || newGcost < gCosts[neighbour])
                {
                    parents[neighbour] = currentNode;
                    gCosts[neighbour] = newGcost;

                    int fScore = newGcost + FindHcost(neighbour, targetNode);

                    if (!openList.Contains(neighbour))
                    {
                        openList.Enqueue(neighbour, fScore);
                    }
                }
            }
        }

        return null;
    }

    Stack<Vector3> GetPath(Node startNode, Node endNode, Dictionary<Node, Node> parents)
    {
        Stack<Vector3> path = new Stack<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Push(currentNode.worldPosition);
            currentNode = parents[currentNode];
        }

        path.Push(startNode.worldPosition);
        return path;
    }

    int GetGcost(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    int FindHcost(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);
        int diagonal = Mathf.Min(dstX, dstY);
        int straight = Mathf.Abs(dstX - dstY);

        return 14 * diagonal + 10 * straight;
    }

    bool IsAdjacent(Node nodeA, Node nodeB)
    {
        return Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x) <= 1 && Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y) <= 1;
    }
}
