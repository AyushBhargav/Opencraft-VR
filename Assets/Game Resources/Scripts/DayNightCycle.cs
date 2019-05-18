using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {


	public Light sun;
	public float maxIntensity = 1.0f;
	public int dayNightCycleTimeSec = 60;
	private float time = 0.0f;
	private float I;
	private float thetaPerUnitTime;
	// Use this for initialization
	void Start () {
		//sun = GetComponent<Light> ();
		sun.intensity = maxIntensity;
		I = maxIntensity;
		thetaPerUnitTime = Mathf.PI / dayNightCycleTimeSec;
	}
	
	// Update is called once per frame
	void Update () {
		float dt = Time.deltaTime;
		float i = I * Mathf.Abs (Mathf.Cos (thetaPerUnitTime * time));
		time += dt;
		sun.intensity = i;
	}
}
