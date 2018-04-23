using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour {

	[System.Serializable] public struct SpawnData{
		public float spawnTime;
		public GameObject spawnObject;
	}

 	public List<SpawnData> spawnItems = new List<SpawnData>();

	void Start () {
		foreach (SpawnData data in spawnItems){
			StartCoroutine(SpawnEnemy(data));
		}
	}
	
	IEnumerator SpawnEnemy(SpawnData data){
		 yield return new WaitForSeconds(data.spawnTime);
		 if(LevelManager.instance.isInGame) Instantiate(data.spawnObject, transform.position, Quaternion.identity);
	}
}
