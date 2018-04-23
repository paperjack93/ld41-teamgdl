using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    public bool isInGame = false;
    public float enemyCheckTimer = 5f;
    public GameObject slideshow;
    public int slideshowTime;
    public float timeBeforeNextLevel = 5f;

    public AudioClip MainMenuTheme;
    public AudioClip GameTheme;
    public AudioClip DeathTheme;
    public AudioClip WinTheme;

    bool _hasEnded = false;
    bool _isIncutscene = false; //For skip
    public int enemyCount;
    public float _timer = 5f;
    AudioSource _audio;
    Animator _anim;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        Screen.SetResolution(1280,768, false);
    }

    void Update(){
        if(enemyCount < 1 && isInGame) _timer -= Time.deltaTime;
        if(_timer < 0f) EndLevel();
    }

    public void PlayGame() {
        slideshow.SetActive(true);
        _isIncutscene = true;
        Invoke("StartGame", slideshowTime);
    }

    public void EndLevel(){
        _audio.clip = WinTheme;
        _audio.Play();

        Invoke("NextLevel", 5f);
    }

    public void NextLevel() {
        if(_audio.clip != GameTheme) {
            _audio.clip = GameTheme;
            _audio.Play();
        }
        _timer = timeBeforeNextLevel;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnSpawnedEnemy(){
        _timer = timeBeforeNextLevel;
        enemyCount++;
    }

    public void OnKilledEnemy(){
        enemyCount--;
        if(enemyCount < 1) Invoke("NextLevel", 1f);
    }

    public void StartGame(){
        NextLevel();
        isInGame = true;
    }

    public void EndGame()
    {
        if (!_hasEnded)
        {
            _hasEnded = true;
            if(_anim != null) _anim.SetBool("hasEnded", _hasEnded);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
