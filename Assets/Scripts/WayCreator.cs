using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayCreator
{
   public Node[,] nodes;

   public WayCreator(int width, int height)
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

   public float CalculateHeuristic(Vector2Int start, Vector2Int end, int type)
   {
       return Mathf.Abs(start.x - end.x) + Mathf.Abs(start.y - end.y) + type;
   }

   public List<Node> CheckNeighbors(Node node)
   {
       List<Node> listNeighbors = new List<Node>();
       int x = node.position.x;
       int y = node.position.y;

       for (int i = -1; i <= 1; i++)
       {
           for (int j = -1; j <= 1; j++)
           {
               if (i == 0 && j == 0) continue;
               int posX = x + i;
               int posY = y + j;
               if (posX >= 0 && posX < nodes.GetLength(0) && posY >= 0 && posY < nodes.GetLength(1))
               {
                   listNeighbors.Add(nodes[posX, posY]);
               }
           }
       }
       return listNeighbors;
   }

   public List<Node> FindWay(Vector2Int startPos, Vector2Int endPos)
   {
       Node startNode = nodes[startPos.x, startPos.y];
       Node endNode = nodes[endPos.x, endPos.y];

       List<Node> toCheck = new List<Node> { startNode };
       List<Node> checkedNode = new List<Node>();

       startNode.goatCost = 0;
       startNode.manhattanDistance = CalculateHeuristic(startPos, endPos, startNode.type);

       while (toCheck.Count > 0)
       {
           Node currentNode = toCheck[0];
           foreach (Node node in toCheck)
           {
               if (node.finalCost < currentNode.finalCost ||
                   (node.finalCost == currentNode.finalCost && node.manhattanDistance < currentNode.manhattanDistance))
               {
                   currentNode = node;
               }
           }

           toCheck.Remove(currentNode);
           checkedNode.Add(currentNode);

           if (currentNode == endNode)
           {
               return FinalPath(startNode, endNode);
           }

           foreach (Node neighbor in CheckNeighbors(currentNode))
           {
               if (checkedNode.Contains(neighbor)) continue;

               float tentativeGCost = currentNode.goatCost + Vector2Int.Distance(currentNode.position, neighbor.position);
               if (tentativeGCost < neighbor.goatCost)
               {
                   neighbor.goatCost = tentativeGCost;
                   neighbor.manhattanDistance = CalculateHeuristic(neighbor.position, endPos, neighbor.type);
                   neighbor.parent = currentNode;

                   if (!toCheck.Contains(neighbor))
                   {
                       toCheck.Add(neighbor);
                   }
               }
           }
       }

       return null; // No hay camino
   }

   private List<Node> FinalPath(Node startNode, Node endNode)
   {
       List<Node> path = new List<Node>();
       Node current = endNode;

       while (current != startNode)
       {
           path.Add(current);
           current = current.parent;
       }
       path.Add(startNode);
       path.Reverse();
       return path;
   }
}


