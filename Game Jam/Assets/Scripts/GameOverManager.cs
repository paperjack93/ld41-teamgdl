using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance = null;  
    Animator anim;
    public PrincessScript princess;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        anim = GetComponent<Animator>();
    }

    public void PrincessDeath(){
        anim.SetBool("hasEnded",true);
        GetComponent<AudioSource>().Play();
        LevelManager.instance.GetComponent<AudioSource>().Stop();
    }

    public void RestartGame()
    {
        LevelManager.instance.ReloadLevel();
    }

}