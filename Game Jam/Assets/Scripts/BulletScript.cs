using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletScript : MonoBehaviour {

	public float distance;
	public float variation;
	public float height;
	public float duration;

	void Start () {
		Vector2 point = (Vector2)transform.position + (Vector2)transform.TransformDirection(Vector3.left) * (distance + Random.Range(-variation, variation));
		Vector2 targetPoint = GetGroundPoint(point);

		GetComponent<Rigidbody2D>().DOJump(targetPoint, height, 1, duration).SetEase(Ease.Linear).OnComplete(()=>{
			Destroy(gameObject);
		});
	}

	Vector2 GetGroundPoint(Vector2 point){
		RaycastHit2D _raycastHit = Physics2D.Raycast(point+Vector2.up*100, Vector2.down, Mathf.Infinity, 1 << 10);
		if(_raycastHit != null){
			return _raycastHit.point;
		} else return transform.position;
	}

    void OnCollisionEnter(Collision collision) {
    	Destroy(gameObject);
    }

}
