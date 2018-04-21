using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordThrowScript : MonoBehaviour {

	public float maxThrowForce = 1f;
	Vector3 _orgMousePos;
	Rigidbody2D _rigidBody;
	ContactFilter2D _filter;
	float _amountInside = 0f;

	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_filter.NoFilter();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
	 		_orgMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
	    } else if (Input.GetButtonUp("Fire1")) {
	 		Vector3 delta = _orgMousePos - Camera.main.ScreenToViewportPoint(Input.mousePosition);
	 		transform.position -= transform.up;
	 		_rigidBody.simulated = true;
	 		_rigidBody.AddForce(delta*maxThrowForce, ForceMode2D.Impulse);
	    }
	}

	void FixedUpdate(){
		Collider2D[] colliders = new Collider2D[5];
		int colliderCount = _rigidBody.OverlapCollider(_filter, colliders);
		if(colliderCount < 1){
	 		float angle = Vector2.SignedAngle(Vector2.up, _rigidBody.velocity);
			_rigidBody.MoveRotation(angle);
		} else {
			if(!_rigidBody.IsSleeping()){
				//_amountInside = _rigidBody.velocity.magnitude * 0.1f;
				transform.position += transform.up * _amountInside;
				_rigidBody.simulated = false;
				_rigidBody.velocity = Vector2.zero;
			}
		}	 	
	}
}
