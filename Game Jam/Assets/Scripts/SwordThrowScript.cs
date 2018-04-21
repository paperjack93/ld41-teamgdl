using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordThrowScript : MonoBehaviour {

	public float maxThrowForce = 1f;
	Vector3 _orgMousePos;
	Rigidbody2D _rigidBody;

	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
	 		_orgMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
	    } else if (Input.GetButtonUp("Fire1")) {
	 		Vector3 delta = _orgMousePos - Camera.main.ScreenToViewportPoint(Input.mousePosition);
	 		_rigidBody.AddForce(delta*maxThrowForce, ForceMode2D.Impulse);
	 		//_rigidBody.MoveRotation();
	    }
	}
}
