using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour {

	Animator anim;
	NavMeshAgent agent;
	GameObject player;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		player = GameObject.Find("Lumberjack1");
	}
	
	// Update is called once per frame
	void Update () {
		
		if  (player.transform.position.y > 19f && 
			(player.transform.position.x < 53f && player.transform.position.x > 8f) && 
			(player.transform.position.z < 58f && player.transform.position.z > 29f) && Vector3.Distance(player.transform.position, transform.position) > 1.5f)
			{
				
				anim.SetBool("isWalking", true);
				agent.SetDestination(player.transform.position);
				
			}
		else
		{
			anim.SetBool("isWalking", false);
			agent.SetDestination(transform.position);
		}
		if (Vector3.Distance(player.transform.position, transform.position) < 1.5f) // if he's in range, skeleton swings his weapon
			anim.SetBool("inRange", true);
		else
			anim.SetBool("inRange", false);
		
	}
}
