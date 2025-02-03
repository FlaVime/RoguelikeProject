using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject nodePrefab;  // Префаб узла
    public GameObject linePrefab;  // Префаб линии (с LineRenderer)

    [Header("Grid Settings")]
    public int columns = 7;  // Количество колонок (X)
    public int rows = 15;     // Количество рядов (Y)
    public float xSpacing = 2.0f;
    public float ySpacing = 2.5f;

    private List<Node> nodes = new List<Node>();

    void Start()
    {
        GenerateNodes();
        GeneratePaths();
    }

    void GenerateNodes()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 position = new Vector2(x * xSpacing, y * ySpacing);

                GameObject newNodeObj = Instantiate(nodePrefab, position, Quaternion.identity, transform);
                newNodeObj.name = $"Node {x},{y}";
                Node newNode = newNodeObj.AddComponent<Node>();  // Добавляем компонент Node
                newNode.position = position;
                nodes.Add(newNode);
            }
        }
    }

    void GeneratePaths()
    {
        foreach (Node node in nodes)
        {
            List<Node> possibleConnections = nodes.FindAll(n => n.position.y > node.position.y);
            possibleConnections.Sort((a, b) => a.position.y.CompareTo(b.position.y)); // Сортируем по Y (ближайшие сверху)

            int maxConnections = Random.Range(1, 3); // 1-3 пути
            int connections = 0;

            foreach (Node target in possibleConnections)
            {
                if (connections >= maxConnections) break;
                if (!node.connections.Contains(target)) // Проверка на дублирование
                {
                    node.connections.Add(target);
                    target.connections.Add(node);
                    DrawLine(node, target);
                    connections++;
                }
            }
        }
    }

    void DrawLine(Node start, Node end)
    {
        GameObject lineObj = Instantiate(linePrefab);
        LineRenderer line = lineObj.GetComponent<LineRenderer>();

        line.positionCount = 2;
        line.SetPosition(0, start.position);
        line.SetPosition(1, end.position);
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
    }
}