using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WayCreator 
{
    Node [,] nodes;

    public void SetNodes(int width, int height)//Crea los nodos
    {
        nodes = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y] = new Node(new Vector2Int(x, y));
            }
        }
    }

    public float CalculateHeuristic(Vector2Int start, Vector2Int end)//Calcula la distancia Manhattan
    {
        return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y);
    }


    public List<Node> CheckNeighbors(Node node)//Revisa los vecinos del nodo
    {
       List<Node> listNeightbors = new List<Node>();
       int x = node.position.x;
       int y = node.position.y;
       for (int i = -1; i < 1; i++)
       {
          for(int j = -1; j < 1; j++)
          {
            int posx = x + i;
            int posy = y + j;
            if (posx >= 0 && posx < nodes.GetLength(0) && posy >= 0 && posy < nodes.GetLength(1))
            {
                listNeightbors.Add(nodes[posx, posy]);
            }
          }
       }
        return listNeightbors;
    }

    
   
}
