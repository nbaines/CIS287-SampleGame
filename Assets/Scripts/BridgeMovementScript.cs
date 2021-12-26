using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeMovementScript : MonoBehaviour {
	
	CharacterController controller;
	public float speed = 3f;
	public Vector3 move = Vector3.zero;
	private float z;
	private float zMove;
	
	// Use this for initialization
	void Start () 
	{
		controller = gameObject.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		moveBridge();
	}
	void moveBridge () 
	{
		z = transform.position.z;
		if(z < 60)
			zMove = 1;
		if (z > 82)
			zMove = -1;
		move = new Vector3(0, 0, zMove);
        move = transform.TransformDirection(move);
        move *= speed;

        controller.Move(move * Time.deltaTime);
	}
}
