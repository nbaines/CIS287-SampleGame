using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// todo:
// enemy guards the bridge, way to defeat? push him into spike pit?
// enemy tries to push you off the cliff into spike pit? -- enemy takes off time
// learn animation, find a good character, save for last -- animation easy, find character
// restart, save state, load, for extra credit
// crumbling platform, one try to jump on -- not for this project
// allow movement while falling somehow -- done
// fix the hold f to fly to allow movement within it, also fix it for when falling -- fixed


//-----------------------------------------------------------------------------------------------------------
// timer, save high scores
// UI that displays score, powerups, remaining time -- done
// enemy that hits you and subtracts time -- done
// death upon falling in the spike pit -- done
// enemy and player animations -- done
// restart, exit -- no exit, restart done.
// timer that ends the game after some amount of time -- game ends on time running out
//-----------------------------------------------------------------------------------------------------------

public class Player01Movement : MonoBehaviour {
	
	public float speed = 5f;
	public float jumpForce = 10f;
	public float gravity = 25f;
	public Vector3 move = Vector3.zero;
	private bool running = false; //checks if sprint is held down to allow user to run
	private bool rightClicked = false; //checks if right click is held down for camera control
	private bool canDoubleJump = false; //allows user to double jump
	private bool fHeld = false; //for the jetpack style powerup 
	private bool canFly = false; // for the jetpack powerup once they collect the pickup is set to true
	private float jumps = 0; // number of jumps performed in the current isgrounded frame
	private float time = 180f; // game lasts 3 minutes or whatever i change it to later and forget this comment
	private float score = 0f;
	private float min = 0f; //minutes left
	private float sec = 0f; // seconds left
	public float sprintSpeed = 5f; //new variable added to control sprint speed, don't want it to just be 2x run speed
	Camera mainCam;
	Text startText;
	Text gameOver;
	Text timer;
	Text scoreText;
	Text doubleJump; //the object named double jump, not related to double jumping within this script - poorly named
	Text flight;
	public Button restartButton; //one of these doesn't need to be here
	public Button btn1;			 // but idk which one yet
	Animator anim;
	CharacterController controller;
	GameObject bridge;
	GameObject bridgeWalls;
	public GameObject restart;
	Transform tempTrans;
	public bool assembly; // change from public to private for the assembly.dll error
	
	void Start () 
	{
		gameOver = GameObject.Find("gameOver").GetComponent<Text>();
		mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
		scoreText = GameObject.Find("scoreText").GetComponent<Text>();
		flight = GameObject.Find("flyText").GetComponent<Text>();
		doubleJump = GameObject.Find("doubleJump").GetComponent<Text>();
		timer = GameObject.Find("Timer").GetComponent<Text>();
		anim = gameObject.GetComponent<Animator>();
		controller = gameObject.GetComponent<CharacterController>();
		bridge = GameObject.Find("Bridge");
		btn1 = restartButton.GetComponent<Button>();
		tempTrans = transform.parent;
		startText = GameObject.Find("startText").GetComponent<Text>();
		btn1.onClick.AddListener(debugOnClick);
	}
	
	void Update () 
	{
		if(timeRemaining()) //timer returns true while the time is above 0s, when it returns false all controls are stopped
					//will probably bring up a black screen with game over and a score on it i hope
		{
			
			withPlayerController();
			isSprinting();
			displayInstructions();
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			animating(h, v);
			isRightClicked();	
		}
		else //the game ends when time runs out, or if they fall in the pit.
		{
			mainCam.clearFlags = CameraClearFlags.SolidColor;
			mainCam.cullingMask = 0;
			doubleJump.enabled = false;
			flight.enabled = false;
			timer.enabled = false;
			restart.SetActive(true);
			scoreText.color = Color.white;
			gameOver.text = "Game Over! Thanks for Playing!";
		}
	}
	void displayInstructions()
	{
		if (time > 175f)
			startText.enabled = true;
		else
			startText.enabled = false;
	}
	void debugOnClick()
	{
		Debug.Log("Button has been clicked!");
		SceneManager.LoadScene("SampleScene");
	}
	void animating(float h, float v)
	{
		bool walking = false;
		if (h != 0f || v != 0f)
		{
			walking = true;
		}
		if (running)
		{
			anim.SetBool("isWalking", false);
			anim.SetBool("isRunning", walking);
		}
		else
		{
			anim.SetBool("isRunning", false);
			anim.SetBool("isWalking", walking);
		}
	}
	void withPlayerController()
	{
		makeChild();
		
		if (controller.isGrounded)
		{
			//Debug.Log("Grounded");
			jumps = 0;
			if (rightClicked)
				move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			else 
				move = new Vector3(0, 0, Input.GetAxis("Vertical"));
			move = transform.TransformDirection(move);
			if (running)
				move *= sprintSpeed;
			else
				move *= speed;
			if (Input.GetButtonDown("Jump") && jumps < 1)
			{
				move.y = jumpForce;
				jumps++;
			}	
		}
		if(!controller.isGrounded)
		{
			//never touch the y in not grounded or you'll shoot off into space or cancel the jump.
			//if (rightClicked)
			//weird bugs with movement in the air with the if/else chain, removed it.
			if (!running)
			{
				move.x = Input.GetAxis("Horizontal") * speed; 
				move.z = Input.GetAxis("Vertical") * speed;
			}
			else
			{
				move.x = Input.GetAxis("Horizontal") * sprintSpeed; 
				move.z = Input.GetAxis("Vertical") * sprintSpeed;
			}
			//else
				//move.z = Input.GetAxis("Vertical") * speed;
			move = transform.TransformDirection(move);
			if (canDoubleJump)
			{
				if (Input.GetButtonDown("Jump") && jumps < 2) //outside of isgrounded jump is shorter, maybe multiply to fix
				{
					move.y = jumpForce * 1.2f; //second jump didn't feel quite right, made it a little higher to compensate
					jumps++;
				}
			}
		}		
		isFlying();
		if (!fHeld)
			move.y -= gravity * Time.deltaTime;
		controller.Move(move * Time.deltaTime);		
	}
	private void isFlying()
	{
		if (Input.GetKey(KeyCode.F) && canFly)
			{
				fHeld = true;
				move.y = 0f;
				move.y += jumpForce *.5f;
			}
		if (Input.GetKeyUp(KeyCode.F))
			fHeld = false;
	}
	private void isRightClicked()
	{
		if (Input.GetButtonDown("Fire2")) 
			rightClicked = true;
		if (Input.GetButtonUp("Fire2"))
			rightClicked = false;
	}
	private void isSprinting()
	{
		if (Input.GetButtonDown("Sprint")) //Sprint is left shift
			running = true;
		if (Input.GetButtonUp("Sprint"))
			running = false;
	}
	private void makeChild()
	{
		if (Vector3.Distance(transform.position, bridge.transform.position) < 2)
			transform.parent = bridge.transform;
	}
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//use hit to see what we just collided with
		if (hit.gameObject.name == "pickup1")
		{
			//first pickup, gives the player the ability to double jump
			doubleJump.enabled = true;
			canDoubleJump = true;
			hit.gameObject.SetActive(false);
		}
		if (hit.gameObject.name == "pickup2" || hit.gameObject.name == "pickup4")
		{
			// this pickup adds a minute to the timer
			time += 30f;
			hit.gameObject.SetActive(false);
		}
		if (hit.gameObject.name == "pickup3")
		{
			//allows player to fly, hidden off the map
			flight.enabled = true;
			canFly = true;
			hit.gameObject.SetActive(false);
		}
	}
	private void OnTriggerEnter(Collider hit)
	{
		//use hit to see what trigger was entered if needed - might need stay for the enemy collision
		//Debug.Log("Entered");
		transform.parent = tempTrans;
		if (hit.transform.IsChildOf(GameObject.Find("coins").transform))
		{
			
			score += 10;
			scoreText.text = "Score: " + score.ToString();
			//hit.MeshRenderer.enabled = false;
			hit.gameObject.SetActive(false); //destroy removes the collider but does not remove the renderer
		}
		if (hit.gameObject.name == "skeleHitbox")
			time -= 15f; //lose 10 seconds if you get hit by the skeleton or whatever i change it to
		if (hit.gameObject.name == "pitPlane")
			time = 0;
		
	}
	private bool timeRemaining()
	{
		//returns false when time has run out otherwise displays the remaining time to the screen
		time -= Time.deltaTime;
		min = (int)(time/60);
		sec = (int)(time % 60);
		if(time > 0)
			timer.text = "Time: " + min.ToString("0") + ":" + sec.ToString("00");
		else
			return false;
		return true;
	}
}