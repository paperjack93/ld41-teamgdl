using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordThrowScript : MonoBehaviour {

	public float maxThrowForce = 1f;
	public float maxVelocity = 10f;
	Vector3 _orgMousePos;
	Rigidbody2D _rigidBody;
	ContactFilter2D _filter;
	float _amountInside = 0f;
	bool _canShoot = true;
	bool _isLaunched = false;
	Collider2D[] _colliders = new Collider2D[5];

	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_filter.NoFilter();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1") && _canShoot) {
	 		_orgMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
	    } else if (Input.GetButtonUp("Fire1") && _canShoot) {
	    	_canShoot = false;
	    	_isLaunched = true;
	 		Vector3 delta = _orgMousePos - Camera.main.ScreenToViewportPoint(Input.mousePosition);
	 		transform.position -= transform.up + transform.up * _amountInside;
	 		_rigidBody.simulated = true;
	 		_rigidBody.AddForce(delta*maxThrowForce, ForceMode2D.Impulse);
	    }
	}

	void FixedUpdate(){
		int colliderCount = _rigidBody.OverlapCollider(_filter, _colliders);
		if(colliderCount < 1){
	 		float angle = Vector2.SignedAngle(Vector2.up, _rigidBody.velocity);
			_rigidBody.MoveRotation(angle);
		} else {
			ProcessCollisions();
			ClearColliders();
		}

		VelocityCheck();
	}

	void VelocityCheck(){
		if(_rigidBody.velocity.magnitude < maxVelocity || !_rigidBody.simulated) return;
		_rigidBody.velocity = _rigidBody.velocity.normalized * maxVelocity;
	}

	void ProcessCollisions(){
		foreach (Collider2D collider in _colliders){
			if(collider == null) continue;
		    if(collider.tag == "Enemy") OnHitEnemy(collider);
		    else if(collider.tag == "Ground") OnHitGround();
		}
	}

	void OnHitEnemy(Collider2D enemy){
		Debug.Log("Hit enemy");
	}

	void OnHitGround(){
		if(!_isLaunched) return;
		_amountInside = _rigidBody.velocity.magnitude * 0.1f;
		transform.position += transform.up * _amountInside;
		_rigidBody.simulated = false;
		_rigidBody.velocity = Vector2.zero;
		_canShoot = true;
		_isLaunched = false;
		Debug.Log("Hit ground");
	}

	void ClearColliders(){
		 for ( int i = 0; i < _colliders.Length; i++)
		 {
		    _colliders[i] = null;
		 }
	}
}
