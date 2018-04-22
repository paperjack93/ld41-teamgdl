using UnityEngine;

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
       if(princess._isDead)
            anim.SetBool("hasEnded",true);
        }
    }

