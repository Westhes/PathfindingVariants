using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

/// <summary>
/// Collect and sorts nodes in the scene, if there are no nodes it will generate them.
/// </summary>
public class NodeManager : MonoBehaviour
{
    [field: NonSerialized]
    public NodeObject[] NodeObjects { get; set; }
    [field: NonSerialized]
    public Node[] Nodes { get; private set; }
    public int NodeCount => Nodes.Length;

    [SerializeField] Material visitedNodeMaterial;
    [SerializeField] Material unvisitedNodeMaterial;
    [SerializeField] Material startNodeMaterial;
    [SerializeField] Material goalNodeMaterial;
    [SerializeField] Material pathNodeMaterial;
    [SerializeField] Material listedNodeMaterial;
    [SerializeField] bool diagonalMovement = false;


    [SerializeField]
    private int gridWidth = 10;
    [SerializeField]
    private int gridHeight = 10;
    [SerializeField]
    private float nodeScale = 0.5f;

    public bool IsInitialized { get; private set; } = false;

    private void Awake()
    {
        NodeObject.visitedMaterial = visitedNodeMaterial;
        NodeObject.unvisitedMaterial = unvisitedNodeMaterial;
        NodeObject.startMaterial = startNodeMaterial;
        NodeObject.goalMaterial = goalNodeMaterial;
        NodeObject.listedMaterial = listedNodeMaterial;
        NodeObject.pathMaterial = pathNodeMaterial;
        NodeObjects = FindObjectsOfType<NodeObject>();
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (IsInitialized) return;

        if (NodeObjects.Length == 0)
        {
            NodeObjects = new NodeObject[gridWidth * gridHeight];
            Nodes = new Node[gridWidth * gridHeight];
            CreateGrid();
        }
        else
        {
            Nodes = NodeObjects.Where(n => n.isActiveAndEnabled).Select(n => n.Node).ToArray();
            LoadGrid();
        }

        InitializeGrid();
        IsInitialized = true;
    }

    void CreateGrid()
    {
        int i = 0;
        for (int z = 0; z < gridWidth; z++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.parent = transform;
                go.transform.position = new Vector3(x, 0, z);
                go.transform.localScale = new Vector3(nodeScale, nodeScale, nodeScale);
                
                NodeObjects[i] = go.AddComponent<NodeObject>();
                Nodes[i] = NodeObjects[i].Node;
                i++;
            }
        }
    }

    private void LoadGrid()
    {
        //var ordered = nodes.OrderBy(n => n.Position.x).ThenBy(n => n.Position.y).ToArray();
        gridWidth = 0;
        gridHeight = 0;

        // Find highest x and y.
        foreach (Node n in Nodes)
        {
            if (n.Position.x < 0 || n.Position.y < 0) throw new Exception("Negative values not supported!"); 

            if (n.Position.x >= gridWidth) gridWidth = n.Position.x + 1; // Add one since x can be 0.
            if (n.Position.y >= gridHeight) gridHeight = n.Position.y + 1; // Add one since y can be 0.
        }

        // Store the nodes in the expected format.
        Node[] newNodes = new Node[gridWidth * gridHeight];
        foreach(Node n in Nodes)
        {
            int index = n.Position.x + n.Position.y * gridWidth;
            newNodes[index] = n;
            //n.transform.parent = transform;
        }
        Nodes = newNodes;
    }

    void InitializeGrid()
    {
        int[] expectedNeighborIndices;
        List<int> validNeighbors = new List<int>();
        for (int i = 0; i < Nodes.Length; i++)
        {
            // Skip if the current nodes is null
            if (Nodes[i] == null) continue;

            validNeighbors.Clear();
            if (!diagonalMovement)
                expectedNeighborIndices = new int[]
                {
                    i - 1, // Left
                    i + 1, // right
                    i + gridWidth, // up
                    i - gridWidth, // down
                };
            else
                expectedNeighborIndices = new int[]
                {
                    i - 1, // 0 Left
                    i + 1, // 1 right
                    i + gridWidth, // 2 up
                    i - gridWidth, // 3 down
                    i + gridWidth - 1, // 4 up left
                    i + gridWidth + 1, // 5 up right
                    i - gridWidth - 1, // 6 down left
                    i - gridWidth + 1, // 7 down right
                };

            // Filter out all the n(eighborIndices) that are out of bounds, or are null
            int offset;
            for (int j = 0; j < expectedNeighborIndices.Length; j++)
            {
                // Skip out of bounds and null references, and one weird case where -1/width doesn't get floored while with any positive number it works fine..
                int nIndex = expectedNeighborIndices[j];
                if (nIndex < 0 || nIndex >= Nodes.Length || Nodes[nIndex] == null || (nIndex == (gridWidth - 1) && j == 4))
                    continue;

                offset = 0;
                switch(j)
                {
                    // Left or right
                    case 0: goto case 1;
                    case 1:
                        // This works because decimal numbers get rounded down.
                        bool isSameColumn = i / gridWidth == nIndex / gridWidth;
                        if (isSameColumn)
                            validNeighbors.Add(nIndex);
                        break;

                    // Up or down
                    case 2: goto case 3;
                    case 3:
                        bool isSameRow = i % gridWidth == nIndex % gridWidth;
                        if (isSameRow)
                            validNeighbors.Add(nIndex);
                        break;
                    case 4:// add one
                        offset = 2;
                        goto case 5;
                    case 5: // reduce one
                        offset -= 1;
                        bool isColumnAbove = i / gridWidth == (nIndex - gridWidth) / gridWidth; // -1 divided by width = 0.. Ok then. I guess I'll use floats.
                        bool isSameRowAbove = i % gridWidth == (nIndex + offset) % gridWidth;
                        if (isColumnAbove && isSameRowAbove)
                            validNeighbors.Add(nIndex);
                        break;
                    case 6:
                        offset = 2;
                        goto case 7;
                    case 7:
                        offset -= 1;
                        bool isColumnBelow = i / gridWidth == (nIndex + gridWidth) / gridWidth;
                        bool isRowBelow = i % gridWidth == (nIndex + offset) % gridWidth;
                        if (isColumnBelow && isRowBelow)
                            validNeighbors.Add(nIndex);
                        break;
                }
            }

            Nodes[i].Neighbors = new ArrayIterator<Node>(GetNodesByIndices(Nodes, validNeighbors));
        }
    }

    public void ResetNodeMaterials()
    {
        foreach(var node in Nodes)
        {
            node?.SetMaterial(NodeState.Unvisited);
        }
    }

    static Node[] GetNodesByIndices(Node[] nodes, List<int> validIndices)
    {
        Node[] selectedNodes = new Node[validIndices.Count];
        for (int i = 0; i < validIndices.Count; i++)
        {
            int index = validIndices[i];
            selectedNodes[i] = nodes[index];
        }
        return selectedNodes;
    }
}
