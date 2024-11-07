using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int totalhp = 1;
    private int hp;
    private SpriteRenderer _renderer;
    public Transform respawnPoint;
    private float maxRespawnTime = 0.25f;
    public float respawnTime;
    public bool dead;
    private float maxWaitTime = 0.25f;
    public float waitTime;
    public PlayerController playerController;
    public GameObject gameOver;
    
    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        hp = totalhp; 
        dead = false;
        respawnTime = maxRespawnTime;
        waitTime = maxWaitTime;
    }

    private void FixedUpdate()
    {

      if (dead)
      {
        waitTime -= Time.deltaTime;
        if(waitTime<= 0)
        {
          respawnTime -= Time.deltaTime;
          Dead(false);
        } 
            
      }
    }

    public void HaveDamage(int damage)
    {
      hp -= damage;
      playerController.enabled = false;
     

      StartCoroutine("EfectoVisual");
     
     
      if (hp <= 0)
      {
          hp = 0;
          dead = true;
      }
    }

    private IEnumerator EfectoVisual()
    {
        _renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _renderer.color = Color.white;
    }

    public void Dead(bool isDead)
    {
      gameOver.SetActive(true);
      transform.position = respawnPoint.position;
      transform.localScale = new Vector3(1, 1, 1);
      waitTime = maxWaitTime;
      GetComponent<BoxCollider2D>().enabled = !isDead;
      GetComponent<SpriteRenderer>().enabled = !isDead;
      
    } 

    public void Revival() 
    {
        dead = false;
        hp = totalhp;
        playerController.enabled = true;
    }
}
