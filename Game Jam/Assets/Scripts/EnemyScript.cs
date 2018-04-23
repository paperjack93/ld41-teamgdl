using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyScript : MonoBehaviour {

	public GameObject killFx;
	public bool faceRight = false;

	bool _isDead = false;

	Rigidbody2D _rigidBody;


	void Start () {
		if(faceRight) transform.localScale = Vector3.Scale(transform.localScale,new Vector3(-1,1,1));
		InvokeRepeating("CheckPos", 1f, 1f);
		LevelManager.instance.OnSpawnedEnemy();
	}
	
	public void OnHit(){
		if(_isDead) return;
		_isDead = true;

		if(killFx != null) Instantiate(killFx, transform.position, Quaternion.identity); 
		Camera.main.DOShakePosition(0.25f,0.25f, 3);
		LevelManager.instance.OnKilledEnemy();
		Destroy(gameObject);
	}

	void CheckPos(){
		if(transform.position.magnitude>60f) Destroy(gameObject);
	}

	void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "sword") OnHit();
    }

}
