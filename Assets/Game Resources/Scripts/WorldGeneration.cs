using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour {

	public static int width = 128;
	public static int depth = 128;
	public static int height = 128;
	public float verticalScale = 20.0f;
	public float horizontalScale = 25.0f;
	public int treeProbability = 20;


	public GameObject grassBlock;
	public GameObject snowBlock;
	public GameObject sandBlock;
	public GameObject dirtBlock;
	public GameObject stoneBlock;
	public GameObject treeBlock;

	void Start () {
		int seed = (int)Network.time * 10;
		for (int i = 0; i < depth; i++) {
			for (int j = 0; j < width; j++) {
				int k = (int)(Mathf.PerlinNoise((j + seed) / horizontalScale, (i + seed) / horizontalScale) * verticalScale);
				Vector3 blockPos = new Vector3 (j, k, i);
				createBlock (k, blockPos, false);
				Debug.LogWarning ("" + k + "\n");
				while (k > 0) {
					k--;
					Vector3 blockPos2 = new Vector3 (j, k, i);
					createBlock (k, blockPos2, true);
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void createBlock(int y, Vector3 blockPos, bool beneath) {
		if (beneath) {
			if (y >= 15) {
				Instantiate (snowBlock, blockPos, Quaternion.identity);
			} else if (y > 0) {
				Instantiate (dirtBlock, blockPos, Quaternion.identity);
			} else if (y == 0) {
				Instantiate (sandBlock, blockPos, Quaternion.identity);
			}
		} else {
			if (y >= 15) {
				Instantiate (snowBlock, blockPos, Quaternion.identity);
			} else if (y > 0) {
				Instantiate (dirtBlock, blockPos, Quaternion.identity);
			} else if(y == 0) {
				Instantiate (sandBlock, blockPos, Quaternion.identity);
			}
		}
	}
}
