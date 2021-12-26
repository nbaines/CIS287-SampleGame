using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// todo:
// changed to right click to move
// add zoom in and zoom out
// smooth camera movement, remove camera as child of player -- maybe not necessary
// camera clamping to stop camera 360s
		
public class CameraMovementScript : MonoBehaviour {
	public float speed = 3f;
	public Vector3 move = Vector3.zero;
	
	public float speedLeftRight = 2f;
	public float speedUpDown = 2f;
	private float leftRight = 0f;
	private float upDown = 0f;
	GameObject character;
	bool rightClickDown = false;
	// Use this for initialization
	void Start () 
	{
		character = GameObject.Find("Lumberjack1");
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		rightClicked();
		moveCamera();
	}
	
	private void rightClicked()
	{
		if (Input.GetButtonDown("Fire2")) //Fire2 is right click
			rightClickDown = true;
		
		if (Input.GetButtonUp("Fire2"))
		{
			rightClickDown = false;
			Cursor.lockState = CursorLockMode.None;
		}
	}
	private void moveCamera()
	{
		if (rightClickDown)// && transform.rotation.y < 56 && transform.rotation.y > 70)	
		{
			Cursor.lockState = CursorLockMode.Locked;
			leftRight += speedLeftRight * Input.GetAxis("Mouse X");
			upDown += speedUpDown * Input.GetAxis("Mouse Y");
			transform.eulerAngles = new Vector3(upDown * -1f, leftRight, 0f);
			character.transform.eulerAngles = new Vector3(0f, leftRight, 0f);
		}
		else
		{
			if (Input.GetKey("a"))
				leftRight -= speedLeftRight * 4f;
			else if (Input.GetKey("d"))
				leftRight += speedLeftRight * 4f;
			
			transform.eulerAngles = new Vector3(upDown * -1f, leftRight, 0f);
			character.transform.eulerAngles = new Vector3(0f, leftRight, 0f);
		}
	}
}
