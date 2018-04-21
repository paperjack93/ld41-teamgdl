using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyScript : MonoBehaviour {

	public GameObject killFx;
	public bool faceRight = false;
	public Vector2 jump = new Vector2(-5f, 5f);
	public float jumpTimer = 3f;
	Rigidbody2D _rigidBody;


	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		InvokeRepeating("DoStep", 1f, jumpTimer);
	}
	
	void Update () {
		
	}

	public void OnHit(){

		if(killFx != null) Instantiate(killFx, transform.position, Quaternion.identity); 
		Destroy(gameObject);
	}

	void DoStep(){
		_rigidBody.AddForce(jump, ForceMode2D.Impulse);
	}	

	Vector2 GetGroundPoint(Vector2 point){
		RaycastHit2D _raycastHit = Physics2D.Raycast(point+Vector2.up*100, Vector2.down, Mathf.Infinity, 1 << 10);
		if(_raycastHit != null){
			return _raycastHit.point;
		} else return transform.position;
	}
}
