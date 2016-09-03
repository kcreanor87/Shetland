using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour {

	public ParticleSystem[] _partSystems;
	public GameObject _muzzleFlash;

	public bool _active;
	public bool _shooting;

	// Use this for initialization
	void Start () {
		_partSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
	}

	// Update is called once per frame
	void Update () {
		if (!_active || _shooting) return;
		StartCoroutine(Shoot());
	}

	IEnumerator Shoot (){
		_shooting = true;
		yield return new WaitForSeconds(0.7f);
		foreach (ParticleSystem _part in _partSystems){
			_part.Play();
		}
		yield return new WaitForSeconds(1.0f);
		_active = false;
		_shooting = false;
	}
}
