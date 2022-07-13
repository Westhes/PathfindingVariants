#define VISUALS

using System.Collections.Generic;
using UnityEngine;

namespace projectName.Pathfinding
{
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
                Node currentNode = openNodes.RemoveFirst();
                closedNodes.Add(currentNode);
#if VISUALS
                currentNode.SetMaterial(NodeState.Visited);
#endif
                // Verify that the current node isn't the goal.
                if (IsGoal(currentNode, goal))
                {
                    break;
                }

                // Add all of the neighbors
                foreach (var neighbor in currentNode.Neighbors)
                {
                    if (closedNodes.Contains(neighbor)) continue;


                    float travelCost = currentNode.G_cost + Heuristic.Distance(currentNode, neighbor) + neighbor.Weight;

                    bool isNewNeighbor = !openNodes.Contains(neighbor);
                    if (travelCost < neighbor.G_cost || isNewNeighbor)
                    {
                        neighbor.G_cost = travelCost;
                        neighbor.F_cost = travelCost; // F=G+H. Set F_Cost in order to avoid additional branches in the Node.CompareTo method
                        neighbor.Parent = currentNode;

                        if (isNewNeighbor)
                        {
#if VISUALS
                            neighbor.SetMaterial(NodeState.Listed);
#endif
                            openNodes.Add(neighbor);
                        }
                    }
                }
            }
        }
    }
}