	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessScript : MonoBehaviour {

    public GameObject blood;

	public bool _isDead = false;
   
    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "sword") KillPrincess("sword");
        else KillPrincess("enemy");
    }

    public void KillPrincess(string source){
        if(_isDead) return;
        Debug.Log("The princess is dead!");
        _isDead = true;

        GameObject sword = GameObject.FindWithTag("Sword");
        if(sword) sword.GetComponent<SwordThrowScript>().isEnabled = false;

        GameOverManager.instance.PrincessDeath();

        if(source == "sword"){
            GameObject _blood = Instantiate(blood, sword.transform.position+sword.transform.up, Quaternion.identity); 
            _blood.transform.rotation = sword.transform.rotation;
        }


        if(LevelManager.instance != null) LevelManager.instance.EndGame();
    }
}

