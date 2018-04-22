using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public bool isInGame = false;
    public float enemyCheckTimer = 5f;
    bool hasEnded = false;
    public Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);


        Screen.SetResolution(1280,768, false);

    }


    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CheckForEnemies(){
        if(!isInGame) return;
        GameObject enemy = GameObject.FindWithTag("Enemy");
        if(enemy != null) return;

        Debug.Log("LEVEL OVER");
    }

    public void StartGame(){
        NextLevel();
        isInGame = true;
        InvokeRepeating("CheckForEnemies", enemyCheckTimer, enemyCheckTimer);
    }

    public void EndGame()
    {
        if (!hasEnded)
        {
           
            hasEnded = true;
            anim.SetBool("hasEnded", hasEnded);
            Debug.Log("GAME OVER");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
