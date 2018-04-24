using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyScript : MonoBehaviour {

	public GameObject killFx;
	public bool faceRight = false;
	public float maxRandomSize = 1.2f;

	bool _isDead = false;

	Rigidbody2D _rigidBody;


	void Start () {
		if(faceRight) transform.localScale = Vector3.Scale(transform.localScale,new Vector3(-1f,1f,1f));
		 transform.localScale = new Vector3(
		 	transform.localScale.x * Random.Range(1f,maxRandomSize),
		 	transform.localScale.y * Random.Range(1f,maxRandomSize),
		 	transform.localScale.z * Random.Range(1f,maxRandomSize));
		InvokeRepeating("CheckPos", 1f, 1f);
		LevelManager.instance.OnSpawnedEnemy();

	}
	
	public void OnHit(){
		if(_isDead) return;
		_isDead = true;
		LevelManager.instance.OnKilledEnemy();
		if(!LevelManager.instance.isInGame) return;

		if(killFx != null) Instantiate(killFx, transform.position, Quaternion.identity); 
		Camera.main.DOShakePosition(0.25f,0.25f, 3);
		Destroy(gameObject);
	}

	void CheckPos(){
		if(transform.position.magnitude>60f) OnHit();
	}

	void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Sword" && collision.otherCollider.tag != "Armor") OnHit();
        else if(collision.gameObject.tag == "Princess") OnHit();
    }

}
