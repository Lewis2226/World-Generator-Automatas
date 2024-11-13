using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsBlocks;
    [SerializeField] private GameObject prefabsWay;
    [SerializeField] private GameObject prefabsTrap;
    [SerializeField] private GameObject prefabsFlag;
    [SerializeField] private GameObject player;
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
            player.SetActive(true);
            Vector3 flagPos = new Vector3(endPos.position.x, endPos.position.y + 1, 0);
            prefabsFlag.transform.position = flagPos;
            prefabsFlag.SetActive(true);
            StopAllCoroutines();
        }
    }

    void ShowRoute()
    {
        foreach (Node node in ruteNode)
        {
            int random = Random.Range(0, 4);
            Vector3 vectorPosition = new Vector3(node.position.x, node.position.y, 0);
            Vector3 PositionTrap = new Vector3(node.position.x, node.position.y +1 , 0);
            Instantiate(prefabsWay, vectorPosition, Quaternion.identity);
            if(random == 1 && node.position.x != 0)
            {
                if (node.position.x != worldSizeX-1)
                {
                    Instantiate(prefabsTrap, PositionTrap, Quaternion.identity);
                }
            }

            
        }
    }
}
 
    

