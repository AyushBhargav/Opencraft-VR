using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BlockType {
	public const int NULL = 0;
	public const int GRASS = 2;
	public const int SAND = 3;
	public const int DIRT = 5;
	public const int TREE = 4;
	public const int SNOW = 1;
	public const int CLOUD = 6;
	public const int UNDERGROUND_SOIL = 7;
}

public class PlayerInteraction : MonoBehaviour {

	public Transform player;

	public int cubeScale = 1;
	public GameObject grassBlock;
	public GameObject snowBlock;
	public GameObject sandBlock;
	public GameObject cloudBlock;
	public GameObject dirtBlock;
	public GameObject treeBlock;

	public GameObject gameManager;
	public Block[,,] worldBlocks;
	GenerateLandscape generateLandscape;
	public int spawnDistance = 3;

	private int[] blockArray = new int[]{ BlockType.GRASS, BlockType.SNOW, BlockType.DIRT, BlockType.SAND, BlockType.TREE };
	public static int blockType = 0;
	void placeBlock (int blockType) {

		Vector3 playerPos = player.position;
		Vector3 playerDirection = player.forward;
		Vector3 blockPosition = playerPos + playerDirection * spawnDistance;
		Vector3 rayDirection = Vector3.down;
		RaycastHit bottomBlockHit;

		// If some shit happens.
		if (!Physics.Raycast (blockPosition, rayDirection, out bottomBlockHit))
			return;
		//Vector3 targetBlockPosition = new Vector3 (blockPosition.x, blockPosition.y + cubeScale - bottomBlockHit.distance);
		Vector3 targetBlockPosition = new Vector3 (bottomBlockHit.transform.position.x, bottomBlockHit.transform.position.y + 1, bottomBlockHit.transform.position.z);

		switch (blockType) {
		case BlockType.GRASS: 
			Instantiate (grassBlock, targetBlockPosition, Quaternion.identity);
			worldBlocks [(int)targetBlockPosition.x, (int)targetBlockPosition.y, (int)targetBlockPosition.z] = new Block (BlockType.GRASS, true);
			break;
		case BlockType.SNOW: 
			Instantiate (snowBlock, targetBlockPosition, Quaternion.identity);
			break;
		case BlockType.SAND: 
			Instantiate (sandBlock, targetBlockPosition, Quaternion.identity);
			break;
		case BlockType.CLOUD: 
			Instantiate (cloudBlock, targetBlockPosition, Quaternion.identity);
			break;
		case BlockType.TREE: 
			Instantiate (treeBlock, targetBlockPosition, Quaternion.identity);
			break;
		case BlockType.DIRT: 
			Instantiate (dirtBlock, targetBlockPosition, Quaternion.identity);
			break;
		}
	}

	// Use this for initialization
	void Start () {
		//player = transform;
		generateLandscape = gameManager.GetComponent<GenerateLandscape>();
		worldBlocks = GenerateLandscape.worldBlocks;
	}
	
	// Update is called once per frame
	void Update () {

		//Code responsibe for identify and call placeBlock()
		if (Input.GetButtonDown ("Fire2")) {
			placeBlock (blockArray[blockType]);
		}

		if (Input.GetButtonDown ("Fire1")) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2.0f, Screen.height / 2.0f, 0));
			if (Physics.Raycast (ray, out hit, 2.0f)) {
				Vector3 blockPos = hit.transform.position;

				if ((int)blockPos.y == 0)
					return;
				switch (worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z].type) {
				case BlockType.GRASS:
					InventoryDetails.grassBlocks++;
					break;
				case BlockType.DIRT:
					InventoryDetails.dirtBlock++;
					break;
				case BlockType.SAND:
					InventoryDetails.sandblock++;
					break;
				case BlockType.SNOW:
					InventoryDetails.snowBlock++;
					break;
				case BlockType.UNDERGROUND_SOIL:
					InventoryDetails.undergroundBlock++;
					break;
				}
				worldBlocks [(int)blockPos.x, (int)blockPos.y, (int)blockPos.z] = null;
				Destroy (hit.transform.gameObject);

				for (int x = -1; x <= 1; x++) {
					for (int y = -1; y <= 1; y++) {
						for (int z = -1; z <= 1; z++) {
							if (!(x == 0 && y == 0 && z == 0)) {
								Vector3 neighbor = new Vector3 (blockPos.x + x, blockPos.y + y, blockPos.z + z);
								generateLandscape.drawBlock (neighbor);
							}
						}
					}
				}
			}
		}
	}
		

}
