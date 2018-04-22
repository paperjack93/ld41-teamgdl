using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{

    Animator anim;
    public PrincessScript princess;

    void Awake()
    {
        
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if (princess._isDead) { 
            anim.SetBool("hasEnded",true);
        }
    }
    public void RestartGame()
    {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

}