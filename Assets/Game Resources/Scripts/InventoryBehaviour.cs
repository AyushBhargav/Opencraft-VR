using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBehaviour : MonoBehaviour {

	// Use this for initialization
	public Camera leftCam,rightCam,mainCam;
	public GameObject grass,snow,dirt,sand,tree;
	private bool found = false,vis = false;
	private GameObject clone = null;
	public float rotateScale;
	public Text cubeInfo;
	private float x;
	private int i;
	private GameObject[] cubeArray;
	private string[] stringArray;
	void Start () {
		cubeArray = new GameObject[]{grass, snow, dirt, sand};
		stringArray = new string[]{"Grass Cube", "Snow Cube", "Dirt Cube", "Sand Cube"};
		i = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!found) {
			leftCam = GameObject.Find ("MainCamera Left").GetComponent<Camera>();
			rightCam = GameObject.Find ("MainCamera Right").GetComponent<Camera>();
			mainCam = Camera.main;
			leftCam.cullingMask = ~(1 << LayerMask.NameToLayer ("UI"));
			rightCam.cullingMask = ~(1 << LayerMask.NameToLayer ("UI"));
			vis = false;
			found = true;
		}
		if (Input.GetButtonDown ("Fire3")) {
			if (!vis) {
				leftCam.cullingMask = 1 << LayerMask.NameToLayer ("UI");
				rightCam.cullingMask = 1 << LayerMask.NameToLayer ("UI");
				vis = true;
				clone = Instantiate (grass, transform.position + new Vector3(0,10,0),Quaternion.identity);
				clone.transform.parent = mainCam.transform;
				clone.layer = LayerMask.NameToLayer("UI");
				clone.transform.localScale += new Vector3(10,10,10);
				cubeInfo.text = "Grass cube";
			} else {
				leftCam.cullingMask = ~(1 << LayerMask.NameToLayer ("UI"));
				rightCam.cullingMask = ~(1 << LayerMask.NameToLayer ("UI"));
				vis = false;
				if (clone != null)
					Destroy (clone);
			}
		}
		if (Input.GetButtonDown("Fire2")) {
			if(vis){
				i = (i + 1) % 5;
				Destroy (clone);
				clone = Instantiate (cubeArray[i], transform.position + new Vector3(0,10,0),Quaternion.identity);
				clone.transform.parent = mainCam.transform;
				clone.layer = LayerMask.NameToLayer("UI");
				clone.transform.localScale += new Vector3(10,10,10);
				cubeInfo.text = stringArray[i];
				PlayerInteraction.blockType = (PlayerInteraction.blockType + 1) % 5;
			}
		}
		if (clone != null)
			clone.transform.Rotate (new Vector3 (0, 1, 0));
	}
}
