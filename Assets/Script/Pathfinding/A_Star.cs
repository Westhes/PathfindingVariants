#define VISUALS

using System.Collections.Generic;
using UnityEngine;

namespace projectName.Pathfinding
{
    public class A_Star : PathfindAlgorithmBase, IPathfindAlgorithm
    {
        public A_Star(int mapSize) : base(mapSize)
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
            //Vector2 direction = ((Vector2)(goal.Position - start.Position)).normalized;
            start.G_cost = 0;
            start.Parent = null;
            openNodes.Add(start);

            //Node currentNode = start;
            while (openNodes.Count > 0)
            {
                Node currentNode = openNodes.RemoveFirst();
                closedNodes.Add(currentNode);
#if VISUALS
                // Set processed color
                currentNode.SetMaterial(NodeState.Visited);
#endif
                // Verify that the current node isn't the goal.
                if (IsGoal(currentNode, goal))
                {
                    return;
                }

                // Add all of the neighbors
                foreach (var neighbor in currentNode.Neighbors)
                {
                    if (closedNodes.Contains(neighbor)) continue;


                    float travelCost = currentNode.G_cost + Heuristic.Distance(currentNode, neighbor) + neighbor.Weight;

                    bool isNewNeighbor = !openNodes.Contains(neighbor);
                    if (travelCost < neighbor.G_cost || isNewNeighbor)
                    {
                        neighbor.Parent = currentNode;
                        neighbor.G_cost = travelCost;
                        neighbor.H_cost = Heuristic.Distance(neighbor, goal);

                        /*
                         * // Normal A* 
                         *  neighbor.F_cost = neighbor.G_cost + neighbor.H_cost;
                         * // A* weighted
                         *  neighbor.F_cost = neighbor.G_cost + neighbor.H_cost * Heuristic.SearchWeight; 
                        */

                        // Weighted A* (pwXU):
                        neighbor.F_cost = neighbor.G_cost < (2 * Heuristic.SearchWeight - 1) * neighbor.H_cost
                            ? neighbor.G_cost / (2 * Heuristic.SearchWeight - 1) + neighbor.H_cost
                            : (neighbor.G_cost + neighbor.H_cost) / Heuristic.SearchWeight;

                        if (isNewNeighbor)
                        {
#if VISUALS
                            neighbor.SetMaterial(NodeState.Listed);
#endif
                            openNodes.Add(neighbor);
                        }
                        else
                            openNodes.UpdateItem(neighbor);
                    }
                }
            }
        }
    }

}