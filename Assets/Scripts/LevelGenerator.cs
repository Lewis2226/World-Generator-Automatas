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
    private int[,] totalObjects;

    
    void Start()
    {
        PerlinNoise();
    }

   
    void Update()
    {
        
    }
    
    void PerlinNoise()
    {
       for(int x = 0;  x < worldSizeX; x++)
       {
         for (int y = 0; y < worldSizeY; y++)
         {
          float CoordX = (float)x/ worldSizeX * scale;
          float CoordY = (float)y/ worldSizeY * scale;
          float perlinValue = Mathf.PerlinNoise(CoordX, CoordY);

          Vector3 position = new Vector3 (x,y,0);
          if(perlinValue <= 0.2f) 
          {
           Debug.Log("Esto es agua");      
          }
          else if(perlinValue <= .4f)
          {
           Debug.Log("Esto es pasto");
          }
          else if (perlinValue <= .6f)
          {
           Debug.Log("Esto es roca");
          }
          else if(perlinValue <= .8f)
          {
           Debug.Log("Esto es mineral");
          }
          else
          {
           Debug.Log("Esto es vacio");
          }
         }
       }
    }

    int WhatisNext()
    {
        return 0;
    }

    void ApplyRules()
    {

    }

    void ShowLevel()
    {

    }


}
