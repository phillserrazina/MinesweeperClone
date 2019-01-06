using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// VARIABLES

	public int x;
	public int y;

	public bool bombTile;
	public bool emptyTile;

	public bool isClicked = false;
	public bool isFlagged = false;

	public Sprite[] tileSprites;

	private BoardGenerator map;
	private SpriteRenderer spriteRenderer;
	private GameManager gameManager;
	private int bombsAround = 0;

	// EXECUTION METHODS

	private void Awake()
	{
		map = FindObjectOfType<BoardGenerator> ();
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		gameManager = FindObjectOfType<GameManager> ();
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0))
			spriteRenderer.color = Color.white;

		if (this.isClicked)
			spriteRenderer.sprite = GetSprite ();
	}

	private void OnMouseOver()
	{
		if (this.isClicked)
			return;
		
		if (Input.GetMouseButtonDown(0))
			spriteRenderer.color = Color.gray;

		if (Input.GetMouseButtonUp(0))
		{
			if (this.bombTile)
				gameManager.gameOver = true;

			if (this.emptyTile)
				RevealEmptyMap (x, y, new bool[map.collumns, map.rows]);

			this.isClicked = true;
		}

		if (Input.GetMouseButtonUp(1))
		{
			if (this.isFlagged)
			{
				spriteRenderer.sprite = tileSprites [11];
				gameManager.remainingFlags++;
				this.isFlagged = false;
			}

			else if (gameManager.remainingFlags > 0 && !this.isFlagged)
			{
				spriteRenderer.sprite = tileSprites [9];
				gameManager.remainingFlags--;
				this.isFlagged = true;
			}
		}
	}

	// METHODS

	/// <summary>
	/// Contructor for the tile
	/// </summary>
	public void Setup()
	{
		if (this.bombTile)
			return;
		
		bombsAround = GetNumberOfBombsAround ();

		if (this.bombsAround == 0)
			this.emptyTile = true;
	}


	#region Utility Methods

	private int GetNumberOfBombsAround()
	{
		int answer = 0;

		answer += CheckForBomb (x - 1, 	y);
		answer += CheckForBomb (x + 1, 	y);
		answer += CheckForBomb (x - 1, 	y + 1);
		answer += CheckForBomb (x, 		y + 1);
		answer += CheckForBomb (x + 1, 	y + 1);
		answer += CheckForBomb (x - 1, 	y - 1);
		answer += CheckForBomb (x, 		y - 1);
		answer += CheckForBomb (x + 1, 	y - 1);

		return answer;
	}

	private int GetNumberOfBombsOnSide()
	{
		int answer = 0;

		answer += CheckForBomb (x - 1, 	y);
		answer += CheckForBomb (x + 1, 	y);
		answer += CheckForBomb (x, 		y + 1);
		answer += CheckForBomb (x, 		y - 1);

		return answer;
	}

	private Sprite GetSprite()
	{
		/*	SPRITE ARRAY REFERENCE
			0: Empty Tile
			1-8: Number Tiles
			9: Flag Tile
			10: Mine Tile
			11: Unclicked Tile
		*/

		if (this.bombTile)
			return tileSprites [10];

		return tileSprites [bombsAround];
	}

	private int CheckForBomb(int x, int y)
	{
		if (x < 0 || x >= map.collumns || y < 0 || y >= map.rows)
			return 0;
		
		if (map.tiles [x, y].bombTile)
			return 1;

		return 0;
	}

	private void RevealEmptyMap(int x, int y, bool[,] visited)
	{
		if (x < 0 || x >= map.collumns || y < 0 || y >= map.rows)
			return;
		
		if (visited [x, y])
			return;

		map.tiles [x, y].spriteRenderer.sprite = map.tiles [x, y].GetSprite ();
		map.tiles [x, y].isClicked = true;

		if (map.tiles [x, y].GetNumberOfBombsOnSide() > 0)
			return;

		visited [x, y] = true;

		RevealEmptyMap (x - 1, y, visited);
		RevealEmptyMap (x + 1, y, visited);
		RevealEmptyMap (x, y - 1, visited);
		RevealEmptyMap (x, y + 1, visited);
	}

	#endregion
}
