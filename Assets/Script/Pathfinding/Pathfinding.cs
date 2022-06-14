#define VISUALS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PathfindingAlgorithms
{
    A_Star,
    Dijkstra,
}

/// <summary> Manager class that allows for different pathfinding algorithms to be executed. </summary>
public class Pathfinding : MonoBehaviour
{
    [SerializeField] NodeObject startNode;
    [SerializeField] NodeObject goalNode;
    [Range(0.05f, 30f)]
    [SerializeField] float SearchWeight = 2f;

    [SerializeField] PathfindingAlgorithms selectedAlgorithm;
    private IPathfindAlgorithm algorithm;
    private Coroutine pathTraversal;

    private static NodeManager nodeManager;

    private NodeObject StartNode
    {
        set
        {
            if (startNode == value || (startNode == goalNode && startNode != null)) return;
            startNode = value;
            Search();
        }
    }

    private NodeObject GoalNode
    {
        set
        {
            if (goalNode == value || value == startNode) return;
            goalNode = value;
            Search();
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
        ApplyAlgorithm();

        if (!startNode || !goalNode)
        {
            // Grab random nodes.
            while (startNode == null) StartNode = nodeManager.NodeObjects[UnityEngine.Random.Range(0, nodeManager.NodeObjects.Length)];
            while (goalNode == null) GoalNode = nodeManager.NodeObjects[UnityEngine.Random.Range(0, nodeManager.NodeObjects.Length)];
        }
    }

    private void Search()
    {
        nodeManager.ResetNodeMaterials();
        // Note: while this doesn't do anything incase goalNode gets set, it still fixes the issue that a path gets drawn that does not exist.
        // It's also a better solution that resetting every node material
        algorithm.ResetPath(goalNode.Node);
        if (pathTraversal != null) StopCoroutine(pathTraversal);
        if (startNode && goalNode) algorithm.Search(startNode.Node, goalNode.Node);
        pathTraversal = StartCoroutine(algorithm.DebugTravelPath(goalNode.Node, 0.01f));
    }

    private void OnValidate()
    {
        Heuristic.SearchWeight = SearchWeight;
        ApplyAlgorithm();
    }

    private void ApplyAlgorithm()
    {
        if (nodeManager != null)
        {
            // Apply the correct algorithm
            algorithm = selectedAlgorithm switch
            {
                PathfindingAlgorithms.A_Star => new A_Star(nodeManager.NodeCount),
                PathfindingAlgorithms.Dijkstra => new Dijkstra(nodeManager.NodeCount),
                _ => new A_Star(nodeManager.NodeCount),
            };
            nodeManager.ResetNodeMaterials();
            // Redo the pathfinding.
            if (startNode && goalNode) Search();
        }
    }
}
