	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessScript : MonoBehaviour {

	bool _isDead = false;
   
    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D collision) {
    	if(_isDead) return;
    	Debug.Log("The princess is dead!");
    	_isDead = true;
        LevelManager.instance.EndGame();
        transform.localScale = Vector3.Scale(transform.localScale,new Vector3(1,-1,1));
    } 
}

