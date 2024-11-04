using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsBlocks;
    [SerializeField] private int worldSizeX;
    [SerializeField] private int worldSizeY;
    [SerializeField] private int scale;
    private GameObject[,] objectsOnLevel;
    private float[,] totalObjects;
    int[] elementoscerca = new int[5];
    int numbertoGenerate = 0;

    void Start()
    {
      totalObjects= new float [worldSizeX,worldSizeY];
      CreateBlocks();
      PerlinNoise();
      StartCoroutine(ChangeLevel());
    }

    void PerlinNoise()
    {
     for (int x = 0; x < worldSizeX; x++)
     {
      for (int y = 0; y < worldSizeY; y++)
      {
       float CoordX = (float)x / worldSizeX * scale;
       float CoordY = (float)y / worldSizeY * scale;
       float perlinValue = Mathf.PerlinNoise(CoordX, CoordY);
       Vector3 position = new Vector3(x, y, 0);
       if (perlinValue <= 0.2f) //Bloque de agua
       {
           objectsOnLevel[x,y].GetComponent<SpriteRenderer>().color = prefabsBlocks[0].GetComponent<SpriteRenderer>().color;
           objectsOnLevel[x,y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[0].GetComponent<SpriteRenderer>().sprite;
       }
       else if (perlinValue <= .4f) //Bloque de esto
       {
           objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[1].GetComponent<SpriteRenderer>().color;
           objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[1].GetComponent<SpriteRenderer>().sprite;
       }
       else if (perlinValue <= .6f) //Bloque de roca
       {
           objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[2].GetComponent<SpriteRenderer>().color;
           objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[2].GetComponent<SpriteRenderer>().sprite;
       }
       else if (perlinValue <= .8f) //Bloque de mineral
       {
           objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[3].GetComponent<SpriteRenderer>().color;
           objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[3].GetComponent<SpriteRenderer>().sprite;
       }
       else  //Bloque de vacio
       {
           objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[4].GetComponent<SpriteRenderer>().color;
           objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[4].GetComponent<SpriteRenderer>().sprite;
       }
       totalObjects[x, y] = perlinValue;
      }
     }
    }

    int[] WhatisNext(int x, int y)
    {
     for (int i = -1; i < 1; i++)
     {
      for (int j = -1; i < 1; i++)
      {
          if (i == 0 && j == 0)
              continue;
        int Posx = x + i;
        int Posy = y + j;
        
        if( Posx >= 0 && Posx <= worldSizeX &&  Posy >= 0 && Posy <= worldSizeY)
        {
         if(totalObjects[x, y] <= .2f)//Verfica si hay agua cerca
         {
           elementoscerca[0]++;
         }
         else if(totalObjects[x, y] <= .4f)//Verfica si hay pasto cerca
         {
           elementoscerca[1]++;
         }
         else if(totalObjects[x, y] <= .6f)//Verifica si hay roca cerca
         {
           elementoscerca[2]++;
         }
         else if(totalObjects[x,y] <= .8f)//Verfica si hay mineral cerca
         {
           elementoscerca[3]++;
         }
         else//Verifica si hay vacio cerca
         {
           elementoscerca[4]++;
         }
        }
      }
     }
     return elementoscerca;
    }

    void ApplyRules(float noisePerlin, int x, int y)
    {
     if (elementoscerca[4] <= 4)//Ve si hay más de 4 bloques de vacio
     {
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[4].GetComponent<SpriteRenderer>().color;
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[4].GetComponent<SpriteRenderer>().sprite;
     }
     else if(noisePerlin <= 0.2f && elementoscerca[0] > 2)//Si el bloque es de agua
     {
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[0].GetComponent<SpriteRenderer>().color;
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[0].GetComponent<SpriteRenderer>().sprite;
     }
     else if(noisePerlin <= .4f && elementoscerca[0] > 2 && elementoscerca[1] < 4)//Si el bloque es de pasto
     {
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[1].GetComponent<SpriteRenderer>().color;
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[1].GetComponent<SpriteRenderer>().sprite;
     }
     else if(noisePerlin <=.6f && elementoscerca[1] == 0)//Si el bloque es de roca
     {
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[2].GetComponent<SpriteRenderer>().color;
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[2].GetComponent<SpriteRenderer>().sprite;
     }
     else if (noisePerlin <= .8f && elementoscerca[2] <= 2)//Si el bloque es de mineral
     {
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[3].GetComponent<SpriteRenderer>().color;
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[3].GetComponent<SpriteRenderer>().sprite;
     }
     else//Si el bloque es de vacio
     {
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().color = prefabsBlocks[4].GetComponent<SpriteRenderer>().color;
      objectsOnLevel[x, y].GetComponent<SpriteRenderer>().sprite = prefabsBlocks[4].GetComponent<SpriteRenderer>().sprite;
     }
    }

    void ShowLevel()
    {
     for(int x = 0; worldSizeX > 0; x++)
     {
      for (int y = 0; y < worldSizeY; y++)
      {
       WhatisNext(x,y);
      }
     }

     for (int x = 0; worldSizeX > 0; x++)
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

    IEnumerator ChangeLevel()
    {
     while(numbertoGenerate < 5)
     {
      ShowLevel();
      numbertoGenerate++;
      yield return null;
     }
    }
}
