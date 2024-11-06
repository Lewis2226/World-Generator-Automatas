using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsBlocks;
    [SerializeField] private GameObject prefabsWay;
    [SerializeField] private int worldSizeX;
    [SerializeField] private int worldSizeY;
    [SerializeField] private float timeWait;
    [SerializeField] private int maxGeneration;
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    private GameObject[,] objectsOnLevel;
    private int[,] totalObjects;
    private WayCreator wayCreator;
    Vector2Int positionStart;
    Vector2Int positionEnd;
    List<Node> ruteNode;

    void Start()
    {
        wayCreator = new WayCreator(worldSizeX, worldSizeY);
        totalObjects = new int[worldSizeX, worldSizeY];
        CreateBlocks();
        RandomBlock();
        positionStart = Vector2Int.RoundToInt((Vector2)startPos.position);
        positionEnd = Vector2Int.RoundToInt((Vector2)endPos.position);

        if (maxGeneration > 0)
        {
            StartCoroutine(StartSimulation(maxGeneration));
        }

        ruteNode = wayCreator.FindWay(positionStart, positionEnd);
    }

    void RandomBlock()
    {
        int randomNum = 0;
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int y = 0; y < worldSizeY; y++)
            {
                randomNum = Random.Range(0, 5);
                objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[randomNum].GetComponent<SpriteRenderer>().color;
                objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[randomNum].GetComponent<SpriteRenderer>().sprite;
                totalObjects[x, y] = randomNum;
                wayCreator.nodes[x, y].type = randomNum; // Setea el tipo de bloque en el nodo
            }
        }
    }
    int[] WhatisNext(int x, int y)
    {
        int[] elementoscerca = new int[5];

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int Posx = x + i;
                int Posy = y + j;

                if (Posx >= 0 && Posx < worldSizeX && Posy >= 0 && Posy < worldSizeY)
                {
                    if (totalObjects[Posx, Posy] == 0)
                        elementoscerca[0]++;
                    else if (totalObjects[Posx, Posy] == 1)
                        elementoscerca[1]++;
                    else if (totalObjects[Posx, Posy] == 2)
                        elementoscerca[2]++;
                    else if (totalObjects[Posx, Posy] == 3)
                        elementoscerca[3]++;
                    else
                        elementoscerca[4]++;
                }
            }
        }
        return elementoscerca;
    }

    void ApplyRules(int blocknum, int x, int y)
    {
        int[] elementoscerca = WhatisNext(x, y);

        if (blocknum == 0 && elementoscerca[0] >= 1)
        {
            SetBlock(x, y, 0);
        }
        else if (blocknum == 1 && elementoscerca[0] >= 1 && elementoscerca[1] < 2)
        {
            SetBlock(x, y, 1);
        }
        else if (blocknum == 2 && elementoscerca[1] == 0)
        {
            SetBlock(x, y, 2);
        }
        else if (blocknum == 3 && elementoscerca[2] >= 1)
        {
            SetBlock(x, y, 3);
        }
        else
        {
            SetBlock(x, y, 4);
        }
    }

    void SetBlock(int x, int y, int type)
    {
        objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[type].GetComponent<SpriteRenderer>().color;
        objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[type].GetComponent<SpriteRenderer>().sprite;
        totalObjects[x, y] = type;
        wayCreator.nodes[x, y].type = type; // Actualiza el tipo del nodo también en WayCreator
    }

    void ShowLevel()
    {
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int y = 0; y < worldSizeY; y++)
            {
                ApplyRules(totalObjects[x, y], x, y);
            }
        }
    }

    void CreateBlocks()
    {
        objectsOnLevel = new GameObject[worldSizeX, worldSizeY];
        for (int i = 0; i < worldSizeX; i++)
        {
            for (int j = 0; j < worldSizeY; j++)
            {
                Vector3 position = new Vector3(i, j, 0);
                GameObject block = Instantiate(prefabsBlocks[4], position, Quaternion.identity);
                objectsOnLevel[i, j] = block;
            }
        }
    }

    IEnumerator StartSimulation(int maxExecutions)
    {
        int executionCount = 0;

        while (executionCount < maxExecutions)
        {
            ShowLevel();
            executionCount++;
            yield return new WaitForSeconds(timeWait);
        }

        if (executionCount == maxExecutions)
        {
            ShowRoute();
            StopAllCoroutines();
        }
    }

    void ShowRoute()
    {
        foreach (Node node in ruteNode)
        {
            Vector3 vectorPosition = new Vector3(node.position.x, node.position.y, 0);
            Instantiate(prefabsWay, vectorPosition, Quaternion.identity);
        }
    }

 
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
}
    

