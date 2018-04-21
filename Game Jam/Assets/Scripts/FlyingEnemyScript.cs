using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyingEnemyScript : MonoBehaviour {

	public float moveDistance = 10f;
	public float moveHeight = 1f;
	public float moveDuration = 1f;
	public float diveDistance = 10f;
	public float diveTime = 3f;
	public float moveTimer = 3f;
	public float diveHeight = 10f;

	Transform _target;
	Rigidbody2D _rigidBody;
	bool _isDiving = false;


	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_target = GameObject.FindWithTag("Princess").transform;

		InvokeRepeating("DoStep", 1f, moveTimer);
	}

	void DoStep(){
		if(_isDiving) return;

		_rigidBody.DOJump(transform.position + new Vector3(moveDistance,0f,0f),moveHeight, 0, moveDuration).OnComplete(()=>{
			if(Mathf.Abs(_target.position.x - transform.position.x) > diveDistance) return;
			StartDive();
		});
	}

	void StartDive(){
		_rigidBody.DOJump(_target.position, diveHeight, 0, diveTime);
		_isDiving = true;
	}
}
