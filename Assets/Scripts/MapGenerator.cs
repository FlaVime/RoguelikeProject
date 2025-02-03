using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int rows = 4;
    public int cols = 3;
    public float xSpacing = 3f;
    public float ySpacing = 2f;

    private List<Node> nodes = new List<Node>();
    public GameObject nodePrefab;
    public GameObject linePrefab;
    
    void Start()
    {
        GenerateMap();
        DrawMap();
    }

    void GenerateMap()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector2 pos = new Vector2(col * xSpacing, -row * ySpacing);
                Node newNode = new Node(pos, "Battle");
                nodes.Add(newNode);

                if (row > 0)
                {
                    int parentIndex = Random.Range(0, cols);
                    nodes[parentIndex].connections.Add(newNode);
                }
            }
        }
    }

    void DrawMap()
    {
    foreach (Node node in nodes)
    {
        GameObject newNodeObj = Instantiate(nodePrefab, node.position, Quaternion.identity);
        
        foreach (Node connectedNode in node.connections)
        {
            GameObject newLine = Instantiate(linePrefab);
            newLine.transform.position = (node.position + connectedNode.position) / 2;
            newLine.transform.right = connectedNode.position - node.position;
            newLine.transform.localScale = new Vector3(Vector2.Distance(node.position, connectedNode.position), 1, 1);
        }
    }
    }
}
