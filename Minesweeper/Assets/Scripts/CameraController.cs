using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// VARIABLES

	public float zoomSpeed = 4.0f;
	public float moveSpeed = 4.0f;

	private float verMoveSpeed = 0.0f;
	private float horMoveSpeed = 0.0f;
	private float zoom = -10.0f;
	private float zoomMin = -10.0f;
	private float zoomMax = -40.0f;

	private float xMin = 0.0f;
	private float xMax = 0.0f;
	private float yMin = 0.0f;
	private float yMax = 0.0f;

	private BoardGenerator map;

	// EXECUTION METHODS

	private void Start()
	{
		map = FindObjectOfType<BoardGenerator> ();

		xMin = -(map.collumns / 2 - 5);
		xMax = map.collumns / 2 - 5;
		yMin = -(map.rows / 2 - 5);
		yMax = map.rows / 2 - 5;

		this.transform.position = map.tiles [map.collumns / 2, map.rows / 2].transform.position;
	}

	private void Update()
	{
		ZoomHandler ();
		PositionHandler ();
	}

	// METHODS

	private void ZoomHandler()
	{
		// Zoom in based on mouse wheel
		zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSpeed;

		// Clamp the zoom
		zoom = Mathf.Clamp (zoom, zoomMax, zoomMin);

		// Update the camera location based on the zoom
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.transform.localPosition.y, zoom);
	}

	private void PositionHandler()
	{
		horMoveSpeed += Input.GetAxisRaw ("Horizontal") * Time.deltaTime * moveSpeed;
		verMoveSpeed += Input.GetAxisRaw ("Vertical") * Time.deltaTime * moveSpeed;

		horMoveSpeed = Mathf.Clamp (horMoveSpeed, xMin, xMax);
		verMoveSpeed = Mathf.Clamp (verMoveSpeed, yMin, yMax);

		this.transform.position = new Vector3 (horMoveSpeed, verMoveSpeed, this.transform.position.z);
	}
}
