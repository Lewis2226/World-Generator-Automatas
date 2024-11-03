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


    void Start()
    {
        PerlinNoise();
    }


    void Update()
    {

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
                    Debug.Log($"Esto es agua, ya que su valor es: {perlinValue}");
                }
                else if (perlinValue <= .4f) //Bloque de esto
                {
                    Debug.Log($"Esto es pasto, ya que su valor es: {perlinValue}");
                }
                else if (perlinValue <= .6f) //Bloque de roca
                {
                    Debug.Log($"Esto es roca, ya que su valor es: {perlinValue}");
                }
                else if (perlinValue <= .8f) //Bloque de mineral
                {
                    Debug.Log($"Esto es mineral, ya que su valor es: {perlinValue}");
                }
                else  //Bloque de vacio
                {
                    Debug.Log($"Esto es vacio, ya que su valor es: {perlinValue}");
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

    void ApplyRules(float noisePerlin)
    {
        if (elementoscerca[4] <= 4)
        {
            Debug.Log("El bloque se vuelve de vacio");
        }
        else if(noisePerlin <= 0.2f && elementoscerca[0] > 2)//Si el bloque es de agua
        {
            Debug.Log("Se queda como agua");
        }
        else if(noisePerlin <= .4f && elementoscerca[0] > 2 && elementoscerca[1] < 4)//Si el bloque es de pasto
        {
            Debug.Log("Se queda como pasto");
        }
        else if(noisePerlin <=.6f && elementoscerca[1] == 0)//Si el bloque es de roca
        {
            Debug.Log("Se queda como roca");
        }
        else if (noisePerlin <= .8f && elementoscerca[2] <= 2)//Si el bloque es de mineral
        {
            Debug.Log("Se queda como mineral");
        }
        else//Si el bloque es de vacio
        {
            Debug.Log("Se queda como vacio");
        }
    }

    void ShowLevel()
    {

    }
}
