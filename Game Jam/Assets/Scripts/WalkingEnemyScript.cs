using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WalkingEnemyScript : MonoBehaviour {

	public Vector2 jump = new Vector2(-5f, 5f);
	public float jumpTimer = 3f;

	Rigidbody2D _rigidBody;

	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();

		InvokeRepeating("DoStep", 1f, jumpTimer);
	}
	
	void DoStep(){
		_rigidBody.AddForce(jump, ForceMode2D.Impulse);
	}	

}
