using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
	public int type;
	public bool vis;

	public Block(int x, bool y) {
		type = x;
		vis = y;
	}
}
public class GenerateLandscape : MonoBehaviour {

	public static int width = 64;
	public static int depth = 64;
	public static int height = 128;
	public static int undergroundHeight = 15;

	public int heightOffset = 100;
	public float heightScale = 15.0f;
	public float detailScale = 10.0f;
	public int numberOfClouds = 30;
	public int cloudOffset;
	public int cloudSize = 10;
	public float treeProbability = 20;

	public GameObject grassBlock;
	public GameObject snowBlock;
	public GameObject sandBlock;
	public GameObject cloudBlock;
	public GameObject dirtBlock;
	public GameObject treeBlock;
	public GameObject undergroundBlock;

	private static int baseHeight = undergroundHeight;
	public static Block[,,] worldBlocks = new Block[width,height + baseHeight,depth];

	void Start () {
		int seed = (int)Network.time * 10;
		for (int i = 0; i < depth; i++) {
			for (int j = 0; j < width; j++) {
				int k = baseHeight + (int)(Mathf.PerlinNoise((j + seed) / detailScale, (i + seed) / detailScale) * heightScale) + heightOffset;
				Vector3 blockPos = new Vector3 (j, k, i);
				createBlock (k, blockPos,true);
				createTree (blockPos);
				while (k > 0) {
					k--;
					blockPos = new Vector3 (j, k, i);
					createBlock (k, blockPos, false);
				}
			}
		}
		createClouds (numberOfClouds, cloudSize);
	}
	public void generateCaves(Vector3[,] caveVoidPoints, int radius) {
		int numberOfCaves = caveVoidPoints.GetLength (0);
		int numberOfVoidPoints = caveVoidPoints.GetLength (1);
		for (int i = 0; i < numberOfCaves; i++) {
			for (int j = 0; j < numberOfVoidPoints; j++) {
				Vector3 voidPoint = caveVoidPoints [i, j];
				for (int x = -1 * radius; x <= radius; x++) {
					for (int y = -1 * radius; y <= radius; y++) {
						for (int z = -1 * radius; z <= radius; z++) {
							Vector3 temp = new Vector3 (voidPoint.x + x, voidPoint.y + y, voidPoint.z + z);
							Instantiate (undergroundBlock, temp, Quaternion.identity);
							Debug.Log (temp.x + " " + temp.y + " " + temp.z + " ");
							worldBlocks [(int)voidPoint.x+x, (int)voidPoint.y+y, (int)voidPoint.z+z] = null;
						}
					}
				}
			}
		}
	}

	void createTree(Vector3 blockPos) {
		int diceRoll = (int)Random.Range (0.0f, 100.0f);
		if (diceRoll > treeProbability || blockPos.y > 10 + baseHeight|| blockPos.y <= 5 + baseHeight)
			return;

		Vector3 treePos = new Vector3 (blockPos.x, blockPos.y + 1.5f, blockPos.z);
		Instantiate (treeBlock, treePos, Quaternion.identity);
	}

	void createBlock(int y, Vector3 blockPos,bool create) {
		if (y > 15 + baseHeight) {
			if (create)
				Instantiate (snowBlock, blockPos, Quaternion.identity);
			worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block (BlockType.SNOW, create);
		} else if (y > 10 + baseHeight) {
			if (create)
				Instantiate (dirtBlock, blockPos, Quaternion.identity);
			worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block (BlockType.DIRT, create);
		} else if (y > 5 + baseHeight) {
			if (create)
				Instantiate (grassBlock, blockPos, Quaternion.identity);
			worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block (BlockType.GRASS, create);
		} else if (y >= 0 + baseHeight) {
			if (create)
				Instantiate (sandBlock, blockPos, Quaternion.identity);
			worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block (BlockType.SAND, create);
		} else if(y < baseHeight){
			if (create)
				Instantiate (undergroundBlock, blockPos, Quaternion.identity);
			worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = new Block (BlockType.UNDERGROUND_SOIL, create);
		}
	}

	public void drawBlock(Vector3 blockPos) {
		if (blockPos.x < 0 || blockPos.x >= width || blockPos.y < 0 || blockPos.y >= height || blockPos.z < 0 || blockPos.z >= depth)
			return;
		if (worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] == null)
			return;
		if (!worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis) {
			worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].vis = true;
			switch (worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type) {
			case BlockType.SNOW:
				Instantiate (snowBlock, blockPos, Quaternion.identity);
				break;
			case BlockType.GRASS:
				Instantiate (grassBlock, blockPos, Quaternion.identity);
				break;
			case BlockType.SAND:
				Instantiate (sandBlock, blockPos, Quaternion.identity);
				break;
			case BlockType.DIRT:
				Instantiate (dirtBlock, blockPos, Quaternion.identity);
				break;
			case BlockType.UNDERGROUND_SOIL:
				Instantiate (undergroundBlock, blockPos, Quaternion.identity);
				break;
			}
		}
	}

	void createClouds(int number, int size) {
		for (int i = 0; i < number; i++) {
			int x = Random.Range (0,3 * width);
			int z = Random.Range (0,3 * depth);
			for (int j = 0; j < size; j++) {
				int y = height - cloudOffset;
				Vector3 blockPos = new Vector3 (x, y, z);
				Instantiate (cloudBlock, blockPos, Quaternion.identity);
				x += Random.Range (-1, 2);
				z += Random.Range (-1, 2);
				if (x < 0 || x >= width)
					x = width / 2;
				if (z < 0 || z >= width)
					z = depth / 2;
			}

		}
	}



	// Update is called once per frame
	void Update () {
		/*if (Input.GetButtonDown ("Fire1")) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2.0f, Screen.height / 2.0f, 0));
			if (Physics.Raycast (ray, out hit, 2.0f)) {
				Vector3 blockPos = hit.transform.position;

				if ((int)blockPos.y == 0)
					return;
				worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;
				Destroy (hit.transform.gameObject);

				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						for (int z = -1; z <= 1; z++) {
							if (!(x == 0 && y == 0 && z == 0)) {
								Vector3 neighbor = new Vector3 (blockPos.x + x, blockPos.y + y, blockPos.z + z);
								drawBlock (neighbor);
							}
						}
					}
				}
			}
		}*/
	}
}
