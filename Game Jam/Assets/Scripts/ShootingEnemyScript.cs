using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyScript : MonoBehaviour {

	public Vector2 jump = new Vector2(-5f, 5f);
	public float jumpTimer = 3f;
	public float shootDistance = 6f;
	public GameObject projectile;
	public float shootTimer = 4f;

	Transform _target;
	Rigidbody2D _rigidBody;
	bool _isShooting;

	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_target = GameObject.FindWithTag("Princess").transform;

		InvokeRepeating("DoStep", 1f, jumpTimer);
	}

	void DoStep(){
		if(_isShooting) return;
		_rigidBody.AddForce(jump, ForceMode2D.Impulse);
		
		if(Mathf.Abs(_target.position.x - transform.position.x) < shootDistance) StartShooting();
		Debug.Log(Mathf.Abs(_target.position.x - transform.position.x)+" "+(Mathf.Abs(_target.position.x - transform.position.x) < shootDistance));
		
	}

	void StartShooting(){
		_isShooting = true;
		InvokeRepeating("Shoot", shootTimer, shootTimer);
	}

	void Shoot(){
		Instantiate(projectile, transform.position, Quaternion.identity);
	}
}
