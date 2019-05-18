using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

	public Transform player;
	private float y;
	public float height = 10.0f;
	// Use this for initialization
	void Start () {
		y = player.position.y - height;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.position.x, y, player.position.z);
	}
}
