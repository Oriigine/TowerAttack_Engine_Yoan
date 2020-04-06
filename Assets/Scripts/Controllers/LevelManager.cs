using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    public float timer = 60f;
    public int intTimer;
    public Action<Alignment> OnTimeGameEnd;

    public void Start()
    {
        EntityManager entityManager = FindObjectOfType<EntityManager>();
        if(entityManager != null)
        {
            entityManager.OnTowerDestroy += EndGame;
        }
    }
    private void Update()
    {
        intTimer = Mathf.RoundToInt(timer);
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            
            
            EndGame(Alignment.Player);
        }
    }

    private void EndGame(Alignment alignment)
    {
        switch(alignment)
        {
            case Alignment.Player:
                Debug.Log("You Loose !");
                break;
            case Alignment.IA:
                Debug.Log("Victory !");
                break;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
