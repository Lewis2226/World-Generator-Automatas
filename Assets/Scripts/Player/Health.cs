using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int totalHealth = 3;
    private int health;
    public float backforce = 1.5f;

    private SpriteRenderer _renderer;
    //public RectTransform heartsUI;
    private float heartsSize = 16.5f;
    public GameObject GamooverScren;


    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        health = totalHealth;
    }

    public void AddDamage(int amount)
    {
        health = health - amount;

        // Visual Feedback
        StartCoroutine("VisualFeedback");

        // Game  Over
        if (health <= 0)
        {
            health = 0;
            Debug.Log("Has muerto");
            GamooverScren.SetActive(true);
            gameObject.SetActive(false);
        }
        //heartsUI.sizeDelta = new Vector2(heartsSize * health, heartsSize);
        Debug.Log("Player got damaged. His current health is " + health);
    }

    
    private IEnumerator VisualFeedback()
    {
        _renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        _renderer.color = Color.white;
    }
}

