using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour {

	// VARIABLES

	public int collumns;
	public int rows;
	public int numberOfBombs;

	public GameObject tilePrefab;

	public Tile[,] tiles;

	// EXECUTION FUNCTIONS

	// Use this for initialization
	void Awake () 
	{
		GenerateMap ();
	}
	
	// METHODS

	private void GenerateMap()
	{
		tiles = new Tile[collumns, rows];

		for (int i = 0; i < collumns; i++)
		{
			for (int j = 0; j < rows; j++)
			{
				float tileX = i - collumns / 2 + 0.5f;
				float tileY = j - rows / 2;
				GameObject go = Instantiate (tilePrefab, new Vector3 (tileX, tileY, 0f), Quaternion.identity, this.transform) as GameObject;
				tiles [i, j] = go.GetComponent<Tile> ();
				tiles [i, j].x = i; tiles [i, j].y = j;
			}
		}

		if (numberOfBombs > (rows * collumns))
		{
			numberOfBombs = (rows * collumns) - ((rows * collumns) / 2);
			Debug.LogError ("Too many bombs! Set to: " + numberOfBombs);
		}

		for (int k = 0; k < numberOfBombs; k++)
		{
			tiles [Random.Range (0, collumns), Random.Range(0, rows)].bombTile = true;
		}

		foreach (Tile tile in tiles)
		{
			tile.Setup ();
		}
	}
}
