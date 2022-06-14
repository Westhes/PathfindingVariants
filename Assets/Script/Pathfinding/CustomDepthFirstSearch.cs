#define VISUALS
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CustomDepthFirstSearch : PathfindAlgorithmBase, IPathfindAlgorithm
{

    public CustomDepthFirstSearch(int mapSize) : base(mapSize) => predicate = IsNewNeighbor;

    public int MaxDepth { get; set; } = 9;
    public Node[][] Paths { get; private set; }

    private static Predicate<Node> predicate;

    public override void Search(Node start, Node finish)
    {
        var foundPaths = FindPath(start, finish, MaxDepth);
        Paths = foundPaths.ToArray();
        Debug.Log($"Paths found {Paths.Length}");
    }

    static Stack<Node[]> FindPath(Node start, Node goal, int maxDepth)
    {
        Stack<Node[]> foundPaths = new Stack<Node[]>();
        Node currentNode = start;
        currentNode.Parent = null;
        int depth = 0;

        while (currentNode != null)
        {
            currentNode.G_cost = depth + 1;
            bool isGoal = IsGoal(currentNode, goal);
            if (isGoal || currentNode.G_cost == maxDepth || !TryGetNewNeighbor(currentNode, out Node neighbor))
            {
                // Go back. We either found a deadend, or the goal.
                if (isGoal)
                {
                    foundPaths.Push(Traceback(currentNode, maxDepth));
                }
                
                depth--;
                currentNode = ReturnToPreviousNode(currentNode);
                continue;
            }

            // Go deeper
            neighbor.Parent = currentNode;
            currentNode = neighbor;
            /* 
             * I know it's very appealing to move this depth increment up above the if statement, but it's here for a reason.
             * Placing the depth above the if statement would require a check whether it was an isGoal case or it was a deadend (no new neighbors), in which depth is reduced by one
             * Otherwise it would need to be reduced by 2 if we found a maxDepth.
             */
            depth++; 
            
        }
        start.G_cost = 0;
        return foundPaths; 
    }

    private static bool TryGetNewNeighbor(Node node, out Node neighbor) => node.Neighbors.TryFindNext(out neighbor, predicate);

    private static bool IsNewNeighbor(Node node) => node.G_cost == 0;

    private static Node ReturnToPreviousNode(Node node)
    {
        node.G_cost = 0;
        node.Neighbors.ResetIterator();
        return node.Parent;
    }

    /// <summary> Draws the path that the pathfinding algorithm found. </summary>
    /// <param name="goal"></param>
    /// <param name="interval"> When the next </param>
    /// <returns></returns>
    public override IEnumerator DebugTravelPath(Node goal, float interval)
    {
        if (Paths.Length == 0)
        {
            yield break;
        }
#if DEBUG_TEXT
        System.Text.StringBuilder sb = new();
#endif
        for (int iPath = 0; iPath < Paths.Length; iPath++)
        {
            var path = Paths[iPath];
#if DEBUG_TEXT
            sb.Clear();
#endif
            // Color the start and goal node.
            path[0].SetMaterial(NodeState.Goal);
            path[^1].SetMaterial(NodeState.Start);

#if DEBUG_TEXT
            sb.Append($"Path {iPath} {path[^1].Position}, ");
#endif

            // Iterate over the items coloring them one at a time.
            yield return new WaitForSeconds(interval);
            for (int i = path.Length - 2; i >= 1; i--)
            {
                yield return new WaitForSeconds(interval);
                path[i].SetMaterial(NodeState.Path);
#if DEBUG_TEXT
                sb.Append($"{path[i].Position}, ");
#endif
            }

#if DEBUG_TEXT
            Debug.Log(sb.ToString());
#endif

            yield return new WaitForSeconds(2f);
            foreach (var p in path) p.SetMaterial(NodeState.Unvisited);
            // Infinite loop
            if (iPath == Paths.Length -1)
            {
                Debug.Log("All paths shown, resetting loop");
                iPath = -1;
            }
        }
    }
}