using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script_2PBot : Photon.MonoBehaviour {
	
	// Use this for initialization
	public int currentState;
	public MineBotAI botAI;
	
	public List<string> commandQueue;
	
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	public GameObject moveDistanceObj;
	public GameObject localMoveDistanceObj;
	
	public float remainingDistance;
	
	public script_Bot thisScript;
	public script_Player playerScript;
	public GameObject player;
	
	public bool selected;
	public bool distanceDrawn;
	
	public TargetMover targetMover;
	public Vector3 targetSpot;
	public float targetDistance;
	public Vector3 spotPos;
	
	public bool player1Unit;
	public bool player2Unit;

	public GameObject ourPlayer;
	public GameObject selectedBy;

	public script_2PGameManager gameManager;

	public bool canAttack;
	public bool canMove;
	public bool disabled;
	
	void Start () {
		thisScript = this.gameObject.GetComponent<script_Bot>();
		botAI = (MineBotAI)this.gameObject.GetComponent("MineBotAI");
		gameManager = GameObject.Find("prefab_2PGameManager(Clone)").gameObject.GetComponent<script_2PGameManager>();

		canAttack = true;
		canMove = true;

		if (player1Unit)
		{
			player = GameObject.Find("Player 1");
			ourPlayer = player;
		}
		else if (player2Unit)
		{
			player = GameObject.Find("Player 2");
			ourPlayer = player;
		}


		playerScript = player.gameObject.GetComponent<script_Player>();
		targetMover = player.gameObject.GetComponent<TargetMover>();
		distanceDrawn = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Disable movement for Enemies
		if (gameManager.player1Turn)
		{
			if (player1Unit == true)
			{
				switch (currentState)
				{
					case 0:
					{
						botAI.canMove = false;
						botAI.canSearch = false;
					}
						break;
						
					case 1:
					{
						botAI.canMove = true;
						botAI.canSearch = true;
					}
					break;
				}
			}
		}
		else if (gameManager.player2Turn)
		{
			if (player2Unit == true)
			{
				switch (currentState)
				{
				case 0:
				{
					botAI.canMove = false;
					botAI.canSearch = false;
				}
					break;
					
				case 1:
				{
					botAI.canMove = true;
					botAI.canSearch = true;
				}
					break;
				}
			}
		}
		
		//If this bot is selected
		if (selected)
		{
			if (selectedBy == ourPlayer)
			{
				botAI.canMove = true;
				
				//Draw our Distance Object once
				if (!distanceDrawn)
				{
						targetMover.target.position = this.gameObject.transform.position;
						localMoveDistanceObj = (GameObject)Instantiate(moveDistanceObj, this.gameObject.transform.position, this.gameObject.transform.rotation);
						localMoveDistanceObj.gameObject.renderer.enabled = true;
						localMoveDistanceObj.gameObject.transform.localScale = new Vector3(remainingDistance / 5f, 1f, remainingDistance / 5f);
						spotPos = this.gameObject.transform.position;
						
						distanceDrawn = true;

				}
			
				//Set a target and Calculate Distance
				targetSpot = targetMover.target.transform.position;
				targetDistance = Vector3.Distance(targetSpot, spotPos);
			
				//If the distance to the object is less than our available distance, Allow Movement
				if (Vector3.Distance(targetSpot, spotPos) < remainingDistance)
				{
					currentState = 1;	
				}
				else
				{
					currentState = 0;	
				}
			}
		}
		else //This unit is not selected
		{
			//So disable movement
			botAI.canMove = false;
			
			//Remove our Distance Object
			if (localMoveDistanceObj != null)
			{
				distanceDrawn = false;
				Destroy(localMoveDistanceObj);	
			}
			
			currentState = 0;
		}
	}
}
