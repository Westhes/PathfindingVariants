using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Store Node in a seperate class to avoid class bloat.
/// </summary>
[Serializable]
public class Node : IHeapItem<Node>
{
    [field: SerializeField]
    public Vector3 WorldPosition { get; set; }

    [field: SerializeField]
    public Vector2Int Position { get; set; }

    [field: NonSerialized]
    public Node[] Neighbors { get; set; }

    /// <summary> The previous & shortest path towards this node. </summary>
    [field: NonSerialized]
    public Node Parent { get; set; }

    [field: SerializeField]
    [field: Tooltip("How heavy it is to travel over this node.")]
    [field: Range(0f, 20f)]
    public float Weight { get; set; } = 0f;

    /// <summary> Distance cost from starting node. </summary>
    [field: SerializeField]
    [field: Tooltip("Distance cost from starting node")]
    public float G_cost { get; set; }

    /// <summary> Distance cost to end node based off of the heuristic. </summary>
    [field: SerializeField]
    [field: Tooltip("Distance cost to end node based off of heuristic")]
    public float H_cost { get; set; }

    /// <summary> Total cost (for finding the next best node). </summary>
    [field: SerializeField]
    public float F_cost { get; set; }

    /// <summary> 0 = unvisited, 1 = visited, 2 = start, 3 = goal. </summary>
    public Action<int> SetMaterial { get; set; }

    int IHeapItem<Node>.HeapIndex { get; set; }

    int IComparable<Node>.CompareTo(Node other)
    {
        int compare = F_cost.CompareTo(other.F_cost);
        if (compare == 0) compare = H_cost.CompareTo(other.H_cost);
        return -compare;
    }
}
public class NodeObject : MonoBehaviour
{
    public static Material visitedMaterial;
    public static Material unvisitedMaterial;
    public static Material startMaterial;
    public static Material goalMaterial;
    public static Material listedMaterial;
    private Renderer renderer;

    [field: SerializeField]
    public Node Node { get; set; }
    

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        if (Node == null) Node = new Node();
        Node.WorldPosition = transform.position;
        Node.Position = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        Node.SetMaterial = SetMaterial;
        name = $"Node {Node.Position.x}_{Node.Position.y}";
    }

    public void SetMaterial(int index)
    {
        renderer.material = index switch
        {
            0 => unvisitedMaterial,
            1 => visitedMaterial,
            2 => startMaterial,
            3 => goalMaterial,
            4 => listedMaterial,
            _ => unvisitedMaterial,
        };
    }
}
