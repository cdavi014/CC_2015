﻿using UnityEngine;
using System.Collections;

public class Hammer : MonoBehaviour
{
	public Animator hammerAnim;
	private bool isHammering = false;
	public bool IsHammering {
		get { return this.isHammering; }
	}

	public Transform hammerCollider;

	public GameObject hammerParticle;

	public AudioClip hammerClip;



	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space) && !this.isHammering) {
			StartCoroutine ("HammerDown");
		}
	}

	IEnumerator HammerDown () {
		float timer = 0.0f;
		float maxTime = 0.2f;

		bool hasHitDown = false;

		this.isHammering = true;
		this.hammerAnim.SetBool ("isHammering", true);

		while (timer < maxTime) {
			yield return new WaitForEndOfFrame ();
			timer += Time.deltaTime;

			if ((timer > 0.1f) && !hasHitDown) {
				hasHitDown = true;
				GameManager.Instance.PlayClipAtLocation (this.hammerClip, this.transform.position);
			}
		}

		this.isHammering = false;
		this.hammerAnim.SetBool ("isHammering", false);
	}

	public void HammerConnected () {
		if (this.hammerParticle != null) {
			GameObject particle = GameObject.Instantiate (this.hammerParticle, this.hammerCollider.position, Quaternion.identity) as GameObject;
			GameObject.Destroy (particle, 1.0f);
		}
	}
}

