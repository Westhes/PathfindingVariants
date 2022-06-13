#define VISUALS

using System.Collections.Generic;
using UnityEngine;

public class Dijkstra : PathfindAlgorithmBase, IPathfindAlgorithm
{
    public Dijkstra(int mapSize) : base(mapSize)
    {
    }

    public override void Search(Node start, Node finish)
    {
        FindPath(start, finish, MapSize);
    }

    static void FindPath(Node start, Node goal, int heapCapacity)
    {
        Heap<Node> openNodes = new Heap<Node>(heapCapacity);
        HashSet<Node> closedNodes = new HashSet<Node>();
        start.G_cost = 0;
        start.Parent = null;
        openNodes.Add(start);

        //Node currentNode = start;
        while (openNodes.Count > 0)
        {
            Node current = openNodes.RemoveFirst();
            closedNodes.Add(current);
#if VISUALS
            current.SetMaterial(1);
#endif
            // Verify that the current node isn't the goal.
            if (current == goal)
            {
#if VISUALS
                current.SetMaterial(3);
                start.SetMaterial(2);
#endif
                // Traceback.
                break;
            }

            // Add all of the neighbors
            foreach (var neighbor in current.Neighbors)
            {
                if (closedNodes.Contains(neighbor)) continue;


                float travelCost = current.G_cost + Heuristic.Distance(current, neighbor) + neighbor.Weight;

                bool isNewNeighbor = !openNodes.Contains(neighbor);
                if (travelCost < neighbor.G_cost || isNewNeighbor)
                {
                    neighbor.G_cost = travelCost;
                    neighbor.F_cost = travelCost; // F=G+H. Set F_Cost in order to avoid additional branches in the Node.CompareTo method
                    neighbor.Parent = current;

                    if (isNewNeighbor)
                    {
#if VISUALS
                        neighbor.SetMaterial(4);
#endif
                        openNodes.Add(neighbor);
                    }
                }
            }
        }
    }
}
