using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Heuristic
{
    /// <summary> 
    /// Search weight
    /// 0        = Dijkstra.
    /// 1        = half Dijkstra, half Greedy Best First Search.
    /// Infinity = Greedy Best First Search.
    /// </summary>
    /// <see href="http://theory.stanford.edu/~amitp/GameProgramming/Variations.html#weighted-a-star"/>
    public static float SearchWeight = 2f;

    /// <summary> Heuristic for calculating travel cost</summary>
    /// <param name="a"> Current node </param>
    /// <param name="b"> Neighbor node </param>
    public static float Distance(Node a, Node b) => SearchWeight * Vector2.Distance(a.Position, b.Position);

    /// <summary> Tiebreaker heuristic, only use for calculating H_Cost </summary>
    /// <param name="a"> The current node </param>
    /// <param name="b"> The goal node </param>
    /// <param name="dir"> A direction from the starting node towards the goal </param>
    static float TieBreaker(Node a, Node b, Vector2 dir)
    {
        float dx = a.Position.x - b.Position.x;
        float dy = a.Position.y - b.Position.y;
        float cross = Mathf.Abs(dx * dir.y - dir.x * dy);

        float distance = Vector2.Distance(a.Position, b.Position);
        return SearchWeight * distance + cross * 0.001f;
    }
}
