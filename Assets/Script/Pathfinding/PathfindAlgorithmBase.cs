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
}
