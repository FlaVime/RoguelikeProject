using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance { get; private set; }

    public int floors = 5;
    public int columns = 3;
    public float horizontalSpacing = 3f;
    public float verticalSpacing = 3f;
    [Range(0f, 1f)]
    public float skipProbability = 0.3f;

    public Node nodePrefab;

    [Range(0, 1)] public float battleProbability = 0.5f;
    [Range(0, 1)] public float treasureProbability = 0.2f;
    [Range(0, 1)] public float restProbability = 0.1f;
    [Range(0, 1)] public float shopProbability = 0.2f;

    public Material lineMaterial;
    public float lineWidth = 0.1f;
    public Color lineColor = Color.white;

    public List<Node> nodes = new List<Node>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GenerateMap();
        CreateConnections();
    }

    private void GenerateMap()
    {
        nodes.Clear();

        for (int row = 0; row < floors; row++)
        {
            // If this is the top floor, create exactly one node (boss) in the center of the floor
            if (row == floors - 1)
            {
                float yPos = row * verticalSpacing;
                Vector3 position = new Vector3(0f, yPos, 0f);

                Node newNode = Instantiate(nodePrefab, transform);
                newNode.transform.localPosition = position;
                newNode.row = row;
                newNode.column = columns / 2;
                newNode.gridPosition = new Vector2(newNode.column, row);
                newNode.nodeType = Node.NodeType.BOSS;
                nodes.Add(newNode);
            }
            else
            {
                List<Node> nodesInRow = new List<Node>();

                float totalRowWidth = (columns - 1) * horizontalSpacing;
                float startX = -totalRowWidth / 2f;

                for (int col = 0; col < columns; col++)
                {
                    // If it's not the first row, maybe skip a cell
                    bool skip = (row != 0) && (Random.value < skipProbability);
                    if (skip)
                        continue;

                    Node newNode = Instantiate(nodePrefab, transform);
                    float xPos = startX + col * horizontalSpacing;
                    float yPos = row * verticalSpacing;
                    Vector3 position = new Vector3(xPos, yPos, 0f);

                    newNode.transform.localPosition = position;
                    newNode.row = row;
                    newNode.column = col;
                    newNode.gridPosition = new Vector2(col, row);

                    // Assignment of node type by row
                    if (row == 0)
                    {
                        newNode.nodeType = Node.NodeType.BATTLE;
                    }
                    else if (row == floors - 2)
                    {
                        newNode.nodeType = Node.NodeType.SHOP;
                    }
                    else
                    {
                        float rand = Random.value;
                        float cumulative = battleProbability;
                        if (rand < cumulative)
                        {
                            newNode.nodeType = Node.NodeType.BATTLE;
                        }
                        else
                        {
                            cumulative += treasureProbability;
                            if (rand < cumulative)
                            {
                                newNode.nodeType = Node.NodeType.TREASURE;
                            }
                            else
                            {
                                cumulative += restProbability;
                                if (rand < cumulative)
                                {
                                    newNode.nodeType = Node.NodeType.REST;
                                }
                                else
                                {
                                    newNode.nodeType = Node.NodeType.SHOP;
                                }
                            }
                        }
                    }

                    nodesInRow.Add(newNode);
                }

                // If no node is created in the row, create one node by force (centered)
                if (nodesInRow.Count == 0)
                {
                    int centralCol = columns / 2;
                    float xPos = -totalRowWidth / 2f + centralCol * horizontalSpacing;
                    float yPos = row * verticalSpacing;
                    Vector3 position = new Vector3(xPos, yPos, 0f);

                    Node newNode = Instantiate(nodePrefab, transform);
                    newNode.transform.localPosition = position;
                    newNode.row = row;
                    newNode.column = centralCol;
                    newNode.gridPosition = new Vector2(centralCol, row);

                    if (row == 0)
                        newNode.nodeType = Node.NodeType.BATTLE;
                    else if (row == floors - 2)
                        newNode.nodeType = Node.NodeType.SHOP;
                    else
                    {
                        float rand = Random.value;
                        float cumulative = battleProbability;
                        if (rand < cumulative)
                        {
                            newNode.nodeType = Node.NodeType.BATTLE;
                        }
                        else
                        {
                            cumulative += treasureProbability;
                            if (rand < cumulative)
                            {
                                newNode.nodeType = Node.NodeType.TREASURE;
                            }
                            else
                            {
                                cumulative += restProbability;
                                if (rand < cumulative)
                                {
                                    newNode.nodeType = Node.NodeType.REST;
                                }
                                else
                                {
                                    newNode.nodeType = Node.NodeType.SHOP;
                                }
                            }
                        }
                    }

                    nodesInRow.Add(newNode);
                }

                nodes.AddRange(nodesInRow);
            }
        }
    }

    private void CreateConnections()
    {
        foreach (Node node in nodes)
        {
            if (node.row < floors - 1)
            {
                // Look for all nodes of the next row that are available for transition (column difference is not more than 1)
                foreach (Node candidate in nodes)
                {
                    if (candidate.row == node.row + 1 && Mathf.Abs(candidate.column - node.column) <= 1)
                    {
                        CreateConnectionLine(node.transform.localPosition, candidate.transform.localPosition);
                    }
                }
            }
        }
    }

    private void CreateConnectionLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        lineObj.transform.SetParent(transform, false);
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;

        if (lineMaterial != null)
            lr.material = lineMaterial;
        else
            lr.material = new Material(Shader.Find("Sprites/Default"));

        lr.startColor = lineColor;
        lr.endColor = lineColor;

        lr.sortingOrder = -1;

        lr.useWorldSpace = false;
    }

    public void OnNodeSelected(Node selectedNode)
    {
        selectedNode.selected = true;
        Debug.Log("Node selected: " + selectedNode.nodeType + " (Row: " + selectedNode.row + ", Column: " + selectedNode.column + ")");
    }
}
