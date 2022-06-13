#define VISUALS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] NodeObject startNode;
    [SerializeField] NodeObject goalNode;
    private static NodeManager nodeManager;

    IPathfindAlgorithm algorithm;

    private NodeObject StartNode
    {
        set
        {
            if (startNode == value) return;
            startNode = value;
            nodeManager.ResetNodes();
            if (startNode && goalNode) algorithm.Search(startNode.Node, goalNode.Node);
        }
    }

    private NodeObject GoalNode
    {
        set
        {
            if (goalNode == value) return;
            goalNode = value;
            nodeManager.ResetNodes();
            if (startNode && goalNode) algorithm.Search(startNode.Node, goalNode.Node);
        }
    }

    void Start()
    {
        nodeManager = FindObjectOfType<NodeManager>();
        StartCoroutine(DelayedStart());
    }

    private void Update()
    {
        bool leftPressed = Input.GetMouseButton(0);
        bool rightPressed = Input.GetMouseButton(1);
        if (leftPressed || rightPressed)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.transform.TryGetComponent<NodeObject>(out NodeObject n))
                {
                    if (leftPressed)
                        StartNode = n;
                    if (rightPressed)
                        GoalNode = n;
                }
            }
        }
    }

    IEnumerator DelayedStart()
    {
        while (!nodeManager.IsInitialized) yield return new WaitForFixedUpdate();
        algorithm = new A_Star(nodeManager.NodeCount);

        if (!startNode || !goalNode)
        {
            // Grab random nodes.
            var nodeManager = FindObjectOfType<NodeManager>();

            int startIndex = UnityEngine.Random.Range(0, nodeManager.Nodes.Length);
            int goalIndex = UnityEngine.Random.Range(0, nodeManager.Nodes.Length);
            if (goalIndex == startIndex) goalIndex++;

            var start = nodeManager.Nodes[startIndex];
            var goal = nodeManager.Nodes[goalIndex];

            //direction = new Vector2(Mathf.Abs(start.Position.x - goal.Position.x), Mathf.Abs(start.Position.y - goal.Position.y));
            algorithm.Search(start, goal);
        }
        else
        {
            //direction = new Vector2(Mathf.Abs(startNode.node.Position.x - goalNode.node.Position.x), Mathf.Abs(startNode.node.Position.y - goalNode.node.Position.y));
            algorithm.Search(startNode.Node, goalNode.Node);
        }
    }
}
