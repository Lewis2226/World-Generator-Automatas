using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsBlocks;
    [SerializeField] private int worldSizeX;
    [SerializeField] private int worldSizeY;
    [SerializeField] private float timeWait;
    [SerializeField] private int maxGeneration;
    private GameObject[,] objectsOnLevel;
    private int[,] totalObjects;

    void Start()
    {
        totalObjects = new int[worldSizeX, worldSizeY];
        CreateBlocks();
        RandomBlock();
        if (maxGeneration > 0)
        {
            StartCoroutine(StartSimulation(maxGeneration));
        }
        else
        {
            Debug.LogWarning("maxGeneration debe ser mayor que cero");
        }
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

        if (blocknum == 0 && elementoscerca[0] >= 1)//Revisa la regla en  Agua
        {
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[0].GetComponent<SpriteRenderer>().color;
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[0].GetComponent<SpriteRenderer>().sprite;
            totalObjects[x, y] = 0;
        }
        else if (blocknum == 1 && elementoscerca[0] >= 1 && elementoscerca[1] < 2)//Revisa la regla en caso del que el bloque sea pasto
        {
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[1].GetComponent<SpriteRenderer>().color;
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[1].GetComponent<SpriteRenderer>().sprite;
            totalObjects[x, y] = 1;
        }
        else if (blocknum == 2 && elementoscerca[1] == 0)//Revisa la regla en caso del que el bloque sea roca
        {
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[2].GetComponent<SpriteRenderer>().color;
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[2].GetComponent<SpriteRenderer>().sprite;
            totalObjects[x, y] = 2;
        }
        else if (blocknum == 3 && elementoscerca[2] >= 1)//Revisa la regla en caso del que el bloque sea mineral
        {
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[3].GetComponent<SpriteRenderer>().color;
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[3].GetComponent<SpriteRenderer>().sprite;
            totalObjects[x, y] = 3;
        }
        else //En caso de ningua regla se cumpla el bloque se vuelve vacio
        {
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[4].GetComponent<SpriteRenderer>().color;
            objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[4].GetComponent<SpriteRenderer>().sprite;
            totalObjects[x, y] = 4;
        }
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

        if (executionCount < maxExecutions)
        {
            StopAllCoroutines();
        }
    }
}
