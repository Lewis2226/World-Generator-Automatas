using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Quit : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quitgame();
            Debug.Log("Se ha salido del juego");
        }
    }

    public void Quitgame()
    {
        Application.Quit();
        Debug.Log("Se ha salido del juego");
    }

    
}
