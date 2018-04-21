using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordThrowScript : MonoBehaviour {

	public float maxThrowForce = 1f;
	public float maxVelocity = 10f;
	public float maxMagnitude = 0.75f;
	public GameObject aimReticle;
	public GameObject aimPointer;

	Vector3 _orgMousePos;
	Rigidbody2D _rigidBody;
	ContactFilter2D _filter;
	float _amountInside = 0f;
	bool _canShoot = true;
	bool _isLaunched = false;
	bool _isAiming = false;
	Collider2D[] _colliders = new Collider2D[5];

	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_filter.NoFilter();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1") && _canShoot) {
	 		_orgMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
	 		_isAiming = true;
	 		aimReticle.SetActive(true);
	 		aimPointer.SetActive(true);
	 		aimReticle.transform.rotation = Quaternion.identity;

	    } else if (Input.GetButtonUp("Fire1") && _isAiming) {	    	
	    	_canShoot = false;
	    	_isLaunched = true;
	    	_isAiming = false;
	 		Vector3 delta = _orgMousePos - Camera.main.ScreenToViewportPoint(Input.mousePosition);
	 		Debug.Log(delta.magnitude);
	 		delta = Vector3.ClampMagnitude(delta, maxMagnitude);
	 		transform.position -= transform.up + transform.up * _amountInside;
	 		_rigidBody.simulated = true;
	 		_rigidBody.AddForce(delta*maxThrowForce, ForceMode2D.Impulse);

	 		aimReticle.SetActive(false);
	 		aimPointer.SetActive(false);
	    }

	    if(_isAiming){
	    	Vector3 delta = _orgMousePos - Camera.main.ScreenToViewportPoint(Input.mousePosition);
	    	delta = Vector3.ClampMagnitude(delta, maxMagnitude);
         	aimPointer.transform.up = delta;
         	aimPointer.transform.localScale = new Vector3(1f,1+delta.magnitude*3f,1f);
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
		_rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, maxVelocity);
	}

	void ProcessCollisions(){
		foreach (Collider2D collider in _colliders){
			if(collider == null) continue;
		    if(collider.tag == "Enemy") OnHitEnemy(collider.GetComponent<EnemyScript>());
		    else if(collider.tag == "Armor") OnHitArmor(collider.GetComponent<ArmorScript>());
		    else if(collider.tag == "Ground") OnHitGround();
		}
	} 

	void OnHitEnemy(EnemyScript enemy){
		if(!_isLaunched) return;
		enemy.OnHit();
		Debug.Log("Hit enemy");
	}

	void OnHitArmor(ArmorScript armor){
		if(!_isLaunched) return;
		armor.OnHit(this);
		_rigidBody.velocity *= -armor.bounceBack;
		Debug.Log("Hit Armor");
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
		for ( int i = 0; i < _colliders.Length; i++){
			_colliders[i] = null;
		}
	}


}
