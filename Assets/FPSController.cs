using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour {
	public float ForwardSpeed = 8.0f;   // Speed when walking forward
	public float BackwardSpeed = 4.0f;  // Speed when walking backwards
	public float JumpForce = 30f;
	private Rigidbody m_RigidBody;
	private CapsuleCollider m_Capsule;
	// Use this for initialization
	void Start () {
		m_RigidBody = GetComponent<Rigidbody>();
		m_Capsule = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Horizontal") !=0 || Input.GetAxis ("Vertical") != 0) {
			float x = Input.GetAxis ("Horizontal");
			float y = Input.GetAxis ("Vertical");
			transform.forward = Camera.main.transform.forward;
		}
	}

}
