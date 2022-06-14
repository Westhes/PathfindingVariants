#define VISUALS

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFirstSearch : PathfindAlgorithmBase, IPathfindAlgorithm
{
    public DepthFirstSearch(int mapSize) : base(mapSize) { }

    public override void Search(Node start, Node finish)
    {
        FindPath(start, finish);
    }

    static void FindPath(Node start, Node goal)
    {
        Stack<Node> openSet = new Stack<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        start.Parent = null;
        openSet.Push(start);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.Pop();
            closedSet.Add(currentNode);
#if VISUALS
            currentNode.SetMaterial(NodeState.Visited);
#endif

            if (IsGoal(currentNode, goal))
            {
                break;
            }

            foreach (Node neighbor in currentNode.Neighbors)
            {
                if (closedSet.Contains(neighbor)) continue;

#if VISUALS
                neighbor.SetMaterial(NodeState.Listed);
#endif
                openSet.Push(neighbor);
                neighbor.Parent = currentNode;
            }
        }
    }
}
