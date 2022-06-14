//#define VISUALS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathfindAlgorithmBase : IPathfindAlgorithm
{
    public PathfindAlgorithmBase(int mapSize)
    {
        MapSize = mapSize;
    }

    public int MapSize { get;  set; }
    public abstract void Search(Node start, Node finish);

    public virtual Node[] SearchPath(Node start, Node finish)
    {
        Search(start, finish);
        return Traceback(finish);
    }

    /// <summary> Traces back the steps the pathfinding alogithm found optimal. </summary>
    /// <returns> An array of nodes that begins from the Finish and ends at the start. (So inverted!)</returns>
    public static Node[] Traceback(Node finish)
    {
        List<Node> path = new List<Node>();
        Node n = finish;
        while (n != null)
        {
            path.Add(n);
            n = n.Parent;
        }
        return path.ToArray();
    }

    protected static bool IsGoal(Node cNode, Node endNode)
    {
        bool isGoal = cNode == endNode;
#if VISUALS && UNITY_EDITOR
        if (isGoal)
        {
            endNode.SetMaterial(NodeState.Goal);
            var tmp = endNode;
            while (tmp.Parent != null)
            {
                tmp = tmp.Parent;
                tmp.SetMaterial(NodeState.Path);
            }
            tmp.SetMaterial(NodeState.Start);
        }
#endif
        return isGoal;
    }

    /// <summary> Draws the path that the pathfinding algorithm found. </summary>
    /// <param name="goal"></param>
    /// <param name="interval"> When the next </param>
    /// <returns></returns>
    public virtual IEnumerator DebugTravelPath(Node goal, float interval)
    {
        if (goal.Parent == null)
        {
            yield break;
        }

        var path = Traceback(goal);

        // Reset the path colors incase the VISUALS pragma was set.
        foreach (var p in path) p.SetMaterial(NodeState.Unvisited);

        // Color the start and goal node.
        path[0].SetMaterial(NodeState.Goal);
        path[^1].SetMaterial(NodeState.Start);
        
        // Iterate over the items coloring them one at a time.
        yield return new WaitForSeconds(interval);
        for (int i = path.Length - 2; i >= 1; i--)
        {
            yield return new WaitForSeconds(interval);
            path[i].SetMaterial(NodeState.Path);
        }
    }

    /// <summary> Clean up the parent references. </summary>
    public virtual void ResetPath(Node goal)
    {
        var path = Traceback(goal);
        foreach (Node n in path)
            n.Parent = null;
    }
}
