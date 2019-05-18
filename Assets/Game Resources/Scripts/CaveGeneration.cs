using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction {
	
	public const int UP = 0;
	public const int DOWN = 1;
	public const int LEFT = 2;
	public const int RIGHT = 3;
	public const int MOVE_AWAY = 4;
	public const int MOVE_TOWARDS = 5;
}

public class CaveGeneration : MonoBehaviour {

	public static int numberOfCavesPerWorldChunk = 3;
	public int radius = 0; //Number of spaces per random worms.
	public static int maxCaveLength = 100;
	public int maxZ = 63;
	public int maxY = 15;
	public int maxX = 63;
	public int minZ= 0;
	public int minY = 0;
	public int minX = 0;
	public Vector3[,] caveVoidPoints;
	GenerateLandscape generateLandscape;

	void Awake () {
		numberOfCavesPerWorldChunk = Random.Range (3, 16);
		caveVoidPoints = new Vector3[numberOfCavesPerWorldChunk,maxCaveLength];
		for (int i = 0; i < numberOfCavesPerWorldChunk; i++) {
			generateCave (i);
		}
		generateLandscape = this.gameObject.GetComponent<GenerateLandscape> ();
		generateLandscape.generateCaves (caveVoidPoints, radius);
	}

	void generateCave(int index) {
		Vector3[] localCavePoints = new Vector3[maxCaveLength];
		// +1 to include maxPoints as Random.Range is exclusive of right arguement.
		Vector3 startingPos = new Vector3 (Random.Range (minX, maxX - radius + 1), Random.Range (minY, maxY - radius + 1), Random.Range (minZ, maxZ - radius + 1));
		localCavePoints [0] = startingPos;

		Vector3 prevPos = startingPos;
		for (int i = 1; i < maxCaveLength; i++) {
			localCavePoints [i] = generateLocalCavePoints (prevPos);
			prevPos = localCavePoints[i];
		}

		for (int i = 0; i < maxCaveLength; i++)
			caveVoidPoints [index, i] = localCavePoints [i];
	}

	Vector3 generateLocalCavePoints(Vector3 prevPoint) {
		
		int x = (int)prevPoint.x;
		int y = (int)prevPoint.y;
		int z = (int)prevPoint.z;
		bool isDone = false;
		while (!isDone) {
			int choice = Random.Range (0, 6);
			switch (choice) {
			case Direction.UP:
				if (y < maxY - radius) {
					y++;
					isDone = true;
				}
				break;
			case Direction.DOWN:
				if (y > minY + radius) {
					y--;
					isDone = true;
				}
				break;
			case Direction.LEFT:
				if (x > minX + radius) {
					x--;
					isDone = true;
				}
				break;
			case Direction.RIGHT:
				if (x < maxX - radius) {
					x++;
					isDone = true;
				}
				break;
			case Direction.MOVE_AWAY:
				if (z < maxZ - radius) {
					z++;
					isDone = true;
				}
				break;
			case Direction.MOVE_TOWARDS:
				if (z > minZ + radius) {
					z--;
					isDone = true;
				}
				break;
			}
		}
		return new Vector3 (x, y, z);
	}
	public int getRadius() {
		return radius;
	}
}
