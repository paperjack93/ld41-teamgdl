	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessScript : MonoBehaviour {

    public GameObject blood;

	bool _isDead = false;
   
    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D collision) {
    	if(_isDead) return;
    	Debug.Log("The princess is dead!");
    	_isDead = true;
        transform.localScale = Vector3.Scale(transform.localScale,new Vector3(1,-1,1));

        if(collision.gameObject.tag == "Sword"){
            GameObject _blood = Instantiate(blood, collision.contacts[0].point, Quaternion.identity); 
            _blood.transform.rotation = collision.transform.rotation;
        }

        if(LevelManager.instance != null) LevelManager.instance.EndGame();
    } 
}

