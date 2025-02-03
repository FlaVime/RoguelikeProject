using UnityEngine;

public class Node : MonoBehaviour
{
    public enum NodeType { NOT_ASSIGNED, BATTLE, TREASURE, REST, SHOP, BOSS }
    
    public NodeType nodeType = NodeType.NOT_ASSIGNED;
    public int row;
    public int column;   
    public Vector2 gridPosition;
    public Node nextNode;    
    public bool selected = false;

    private void OnMouseDown()
    {
        MapGenerator.Instance.OnNodeSelected(this);
    }
}
