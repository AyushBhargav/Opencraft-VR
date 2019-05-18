using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleModified : MonoBehaviour {

	public Light sun;
	public int dayTimeSec = 60;
	public float startingIntensity = 1.0f;
	public float timeBetweenMorningAndEvening = 70.0f;
	public Camera leftCam,rightCam;
	public bool found = false;

	private Color color1 = Color.white;
	private Color color2 = Color.magenta;
	private Color color3 = Color.black;
	private Color morningSkyColor;
	private Color nightSkyColor = Color.black;

	private float I;
	private float time = 0.0f;
	private float thetaByT;
	private float timeBetweenEveningAndNight = 30.0f;
	public bool isMorning = true;


	// Use this for initialization
	void Start () {
		sun.intensity = startingIntensity;
		I = startingIntensity;
		thetaByT = (float)(Mathf.PI / (dayTimeSec * 2.0f));
	}
	
	// Update is called once per frame
	void Update () {
		if (!found) {
			leftCam = GameObject.Find ("MainCamera Left").GetComponent<Camera>();
			rightCam = GameObject.Find ("MainCamera Right").GetComponent<Camera>();
			morningSkyColor = leftCam.backgroundColor;
			found = true;
		}
		if (found) {
			float dt = Time.deltaTime;
			float i = I * Mathf.Abs (Mathf.Cos (thetaByT * time));
			time += dt;
			float timeGradient = i / startingIntensity;
			if (((int)time / dayTimeSec) % 2 == 0) {
				sun.color = Color.Lerp (color1, color3, 1 - timeGradient);
				leftCam.backgroundColor = Color.Lerp (morningSkyColor, nightSkyColor, 1 - timeGradient);
				rightCam.backgroundColor = Color.Lerp (morningSkyColor, nightSkyColor, 1 - timeGradient);
			} else {
				sun.color = Color.Lerp (color3, color1, timeGradient);
				leftCam.backgroundColor = Color.Lerp (nightSkyColor, morningSkyColor, timeGradient);
				rightCam.backgroundColor = Color.Lerp (nightSkyColor, morningSkyColor, timeGradient);
			}
			sun.intensity = i;
		}
	}
}
