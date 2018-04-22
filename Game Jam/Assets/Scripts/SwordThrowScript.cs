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
	public GameObject help;
	public AudioClip shootSFX;
	public AudioClip chargeSFX;
	public float timeToShowHelp = 3f;

	Vector3 _orgMousePos;
	Rigidbody2D _rigidBody;
	Collider2D _collider;
	ContactFilter2D _filter;
	float _amountInside = 0f;
	bool _isInGround = true;
	bool _canShoot = true;
	bool _isLaunched = false;
	bool _isAiming = false;
	bool _isMovingOutOfGround = false;
	Collider2D[] _colliders = new Collider2D[4];
	RaycastHit2D[] _raycastHits = new RaycastHit2D[1];
	Collider2D _groundCollider;
	AudioSource _audio;
	float _timeSinceLastClick;

	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();
		_audio = GetComponent<AudioSource>();
		_filter.NoFilter();
	}

	void Update () {
		if(_isAiming) UpdateReticles();

		if (Input.GetButtonDown("Fire1") && _canShoot) {
			if(_isInGround && _groundCollider != null && !_isMovingOutOfGround) {
				_isMovingOutOfGround = true;
				transform.DOMoveY(transform.position.y - _collider.Distance(_groundCollider).distance + 0.5f, 0.2f).OnComplete(()=>{
					_isInGround = false;
					_isMovingOutOfGround = false;
					if(Input.GetButton("Fire1") && _canShoot) StartAiming();
				});
			} else StartAiming();

			_timeSinceLastClick = 0f;
			help.SetActive(false);
	    } else if (Input.GetButtonUp("Fire1") && _isAiming) Shoot();

	    if(!_isLaunched) return;

		int colliderCount = _rigidBody.OverlapCollider(_filter, _colliders);
		int rayCastCount = _collider.Cast((Vector2)_rigidBody.velocity.normalized, _raycastHits, _rigidBody.velocity.magnitude * Time.deltaTime);

		if(rayCastCount>0){
			foreach (RaycastHit2D hit in _raycastHits){
				if(hit == null) continue;
				_colliders[colliderCount] = hit.collider;
				colliderCount++;
			}
		}
		if(colliderCount > 0){
			ProcessCollisions();
			ClearColliders();
		} else {
			float angle = Vector2.SignedAngle(Vector2.up, _rigidBody.velocity);
			_rigidBody.MoveRotation(angle);
		}
	}

	void FixedUpdate(){
		VelocityCheck();
		HelpCheck();
	}

	void Shoot(){
		_audio.clip = shootSFX;
		_audio.Play();

		_canShoot = false;
	    _isLaunched = true;
	    _isAiming = false;
	 	Vector3 delta = _orgMousePos - Camera.main.ScreenToViewportPoint(Input.mousePosition);
	 	delta = Vector3.ClampMagnitude(delta, maxMagnitude);
	 	delta.y = Mathf.Max(0, delta.y);
	 	_rigidBody.simulated = true;
	 	_rigidBody.AddForce(delta*maxThrowForce, ForceMode2D.Impulse);

	 	Camera.main.DOShakePosition(delta.magnitude,delta.magnitude*2);

	 	aimReticle.SetActive(false);
	 	aimPointer.SetActive(false);
		help.SetActive(false);

		int colliderCount = _rigidBody.OverlapCollider(_filter, _colliders);
		if(colliderCount > 1){
			ProcessCollisions();
			ClearColliders();
		}
	}

	void StartAiming(){
		_audio.clip = chargeSFX;
		_audio.Play();
		_orgMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		_isAiming = true;
		aimReticle.transform.rotation = Quaternion.identity;
		UpdateReticles();
		aimReticle.SetActive(true);
		aimPointer.SetActive(true);
		help.SetActive(false);
	}

	void UpdateReticles(){
		Vector3 delta = _orgMousePos - Camera.main.ScreenToViewportPoint(Input.mousePosition);
	    delta = Vector3.ClampMagnitude(delta, maxMagnitude);
	    delta.y = Mathf.Max(0, delta.y);
        aimPointer.transform.up = delta;
        aimPointer.transform.localScale = new Vector3(1f,1+delta.magnitude*6f,1f);
        _timeSinceLastClick = 0f;

        //Camera.main.DOShakePosition(0.05f,delta.magnitude/10);
	}

	void VelocityCheck(){
		if(_rigidBody.velocity.magnitude < maxVelocity || !_rigidBody.simulated) return;
		_rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, maxVelocity);
	}

	void HelpCheck(){
		if(_isInGround && !_isAiming) _timeSinceLastClick += Time.fixedDeltaTime;
		if(_timeSinceLastClick < timeToShowHelp || help.activeSelf) return;
		help.transform.rotation = Quaternion.identity;
		help.SetActive(true);
	}

    void OnCollisionEnter2D(Collision2D collision) {
    	ProcessCollider(collision.otherCollider);
    } 

	void ProcessCollisions(){
		foreach (Collider2D collider in _colliders){
			if(collider == null) continue;
			ProcessCollider(collider);
		}
	}

	void ProcessCollider(Collider2D collider){
		if(collider.tag == "Enemy") OnHitEnemy(collider.GetComponent<EnemyScript>());
		else if(collider.tag == "Armor") OnHitArmor(collider, collider.GetComponent<ArmorScript>());
		else if(collider.tag == "Ground") OnHitGround(collider);
	}

	void OnHitEnemy(EnemyScript enemy){
		if(!_isLaunched) return;
		enemy.OnHit();
		Debug.Log("Hit enemy");
	}

	void OnHitArmor(Collider2D collider, ArmorScript armor){
		if(!_isLaunched) return;
		armor.OnHit(this);

		ContactPoint2D[] contacts = new ContactPoint2D[1];
		if(_collider.GetContacts(contacts) < 1) return;
		ContactPoint2D contact = contacts[0];
        Vector2 direction = Vector2.Reflect(_rigidBody.velocity.normalized, contact.normal);
				Debug.Log(_rigidBody.velocity.normalized +" " +	 contact.normal + " "+ direction);

		_rigidBody.velocity = contact.normal * Mathf.Min(_rigidBody.velocity.magnitude, 15f); 

		Debug.Log("Hit Armor");
	}

	void OnHitGround(Collider2D collider){
		if(!_isLaunched) return;
		_amountInside = Mathf.Min(1f, _rigidBody.velocity.magnitude * 0.1f);
		transform.position += transform.up * _amountInside;
		_rigidBody.simulated = false;
		_rigidBody.velocity = Vector2.zero;
		_canShoot = true;
		_isLaunched = false;
		_isInGround = true;
		_groundCollider = collider;
		Debug.Log("Hit ground");
	}

	void ClearColliders(){
		for ( int i = 0; i < _colliders.Length; i++){
			_colliders[i] = null;
		}
	}
}
