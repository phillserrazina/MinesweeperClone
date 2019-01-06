using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// VARIABLES

	public bool gameOver = false;
	public int remainingFlags = 0;

	public Text finalText;
	public Text flagsText;

	private BoardGenerator map;
	private bool playerWon = false;

	// EXECUTION METHODS

	private void Awake()
	{
		map = FindObjectOfType<BoardGenerator> ();
		remainingFlags = map.numberOfBombs;
	}

	private void Update()
	{
		flagsText.text = remainingFlags.ToString ();

		if (gameOver)
		{
			ClickAllTiles ();
			finalText.text = "You lost!";
			finalText.transform.parent.gameObject.SetActive (true);
		}

		if (CheckForWin())
		{
			ClickAllTiles ();
			finalText.text = "You won!";
			finalText.transform.parent.gameObject.SetActive (true);
		}
	}

	// METHODS

	public void ResetScene()
	{
		SceneManager.LoadScene ("GameScene");
	}

	private void ClickAllTiles()
	{
		for (int i = 0; i < map.collumns; i++)
			for (int j = 0; j < map.rows; j++)
				map.tiles [i, j].isClicked = true;
	}

	private bool CheckForWin()
	{
		Tile[,] tileArray = map.tiles;

		foreach (Tile tile in tileArray) 
		{
			if (tile.bombTile && !tile.isFlagged)
				return false;
		}

		return true;
	}
}
