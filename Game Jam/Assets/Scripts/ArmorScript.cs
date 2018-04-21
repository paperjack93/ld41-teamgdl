using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorScript : MonoBehaviour {

	public GameObject hitFx;
	public int hp = 5;
	public float bounceBack = 1f;

	public void OnHit(SwordThrowScript sword){
		if(hitFx != null) Instantiate(hitFx, transform.position, Quaternion.identity);
		hp--;
		if(hp == 0) Destroy(gameObject);
	}
}
