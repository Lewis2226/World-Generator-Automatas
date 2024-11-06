using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node 
{
    public float goatCost;
    public float manhattanDistance;
    public int type; 
    public float finalCost => goatCost +  manhattanDistance;
    public Node parent;
    public Vector2Int position;

    public Node(Vector2Int _position, float gCost = float.MaxValue, float _manhattanDistance = 0)
    {
        position = _position;
        goatCost = gCost;
        manhattanDistance = _manhattanDistance;

    }

    

   

}
