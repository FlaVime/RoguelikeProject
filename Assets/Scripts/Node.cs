using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector2 position; // Node coordinates
    public List<Node> connections = new List<Node>(); // Connection with nodes
}
