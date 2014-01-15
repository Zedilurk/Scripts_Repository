using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script_2PSetup : MonoBehaviour {
	
	// Use this for initialization
	private GameObject player1Obj;
	private GameObject player;
	private Camera mainCam;
	
	public GameObject playerPrefab;
	public GameObject gameManagerPrefab;
	public GameObject botPrefab;
	
	private GameObject player1Inst;
	private GameObject player2Inst;
	
	private RtsCameraKeys camKeys;
	private RtsCameraMouse camMouse;
	private RtsCamera cam;
	private script_2PPlayer playerScript;
	private RtsEffectsUpdater effectsUpdater;
	private TargetMover targetScript;
	private Camera playerView;
	private AudioListener playerAudio;
	public Camera playerCam;
	public Camera unitCam;
	
	private List<GameObject> activeUnitList = new List<GameObject>();
	private GameObject[] unitFabs;
	
	public bool allowInput { get; set; }
	public int currPlayerTurn  { get; set; }
	
	public bool useTurns = true;				
	public bool combatOn = true;
	
	public int count;
	
	public static GameObject unit;
	
	public enum PlayerType {Nerd, Jock};
	public int playerNumber;
	public PlayerType playerType;
	
	private GameObject myBot;
	public List<GameObject> unitList;
	
	public Vector3 player1SpawnCoords;
	public Vector3 player2SpawnCoords;
	
	public Vector3[] unitSpawnCoords = new Vector3[5];
	public Vector3[] unit2SpawnCoords = new Vector3[5];
	
	public Vector3 playerMinCoords;
	public Vector3 playerMaxCoords;
	
	public GameObject gameManager;
	public script_2PGameManager gmScript;

	public GameObject childCam;

	public GameObject[] jockSpawns;
	public GameObject[] nerdSpawns;
	
	void Awake () {
		
		#region Create Player Objects
		//Create Player
		player1Inst = (GameObject)Instantiate(playerPrefab, player1SpawnCoords, Quaternion.identity);
		player2Inst = (GameObject)Instantiate(playerPrefab, player2SpawnCoords, new Quaternion(45, 180, 0, 1));
		//Create Game Manager
		gameManager = (GameObject)Instantiate(gameManagerPrefab, new Vector3(0,0,0), Quaternion.identity);
		gmScript = gameManager.GetComponent<script_2PGameManager>();

		player1Inst.gameObject.name = "Player 1";
		player2Inst.gameObject.name = "Player 2";

		EnablePlayer1();

		int randSelect = Random.Range(0,2);
		{
			if (randSelect == 0)
			{
				//Assign Player 1 to Nerd
				Debug.Log("Player should be a Nerd.");
				playerScript.playerType = script_2PPlayer.PlayerType.Nerd;

				//Assign Player 2 to Jock
				playerScript = player2Inst.GetComponent<script_2PPlayer>();
				playerScript.playerType = script_2PPlayer.PlayerType.Jock;
				playerScript.enabled = false;

				//Give Player 1 his script back
				playerScript = player1Inst.GetComponent<script_2PPlayer>();

			}
			else if (randSelect == 1)
			{
				//Assign Player 1 to Jock
				Debug.Log("Player should be a Jock.");
				playerScript.playerType = script_2PPlayer.PlayerType.Jock;

				//Assign Player 2 to Nerd
				playerScript = player2Inst.GetComponent<script_2PPlayer>();
				playerScript.playerType = script_2PPlayer.PlayerType.Nerd;
				playerScript.enabled = false;
				
				//Give Player 1 his script back
				playerScript = player1Inst.GetComponent<script_2PPlayer>();

			}
		}

		#endregion
	}
	
	void Start () {
		SpawnUnit(count);
	}
	
	void Update () {
		
	}
	
	void OnGUI () {
		
	}
	
	public void SpawnUnit(int networkCount)
	{
		Debug.Log("Running Player Unit Spawn Code");
		
		//If we are Nerds, spawn nerds in front of us
		if (playerScript.playerType == script_2PPlayer.PlayerType.Nerd)
		{
			//For the number of units we are supposed to have, spawn allies
			for (int i = 0; i < networkCount; i++)
			{
				Vector3 currentSpawnCoords;
				currentSpawnCoords.x = unitSpawnCoords[i].x;
				currentSpawnCoords.y = unitSpawnCoords[i].y;
				currentSpawnCoords.z = unitSpawnCoords[i].z;
				myBot = (GameObject)Instantiate(nerdSpawns[i], currentSpawnCoords,Quaternion.identity);
				myBot.gameObject.name = "Nerd Unit " + i;
				
				script_2PBot botScript = myBot.transform.gameObject.GetComponent<script_2PBot>();
				botScript.player1Unit = true;
				
				unitList.Add(myBot.gameObject);
			}
			
			//For the number of units we are supposed to have, spawn enemies
			for (int i = 0; i < networkCount; i++)
			{
				Vector3 currentSpawnCoords;
				currentSpawnCoords.x = unit2SpawnCoords[i].x;
				currentSpawnCoords.y = unit2SpawnCoords[i].y;
				currentSpawnCoords.z = unit2SpawnCoords[i].z;
				myBot = (GameObject)Instantiate(jockSpawns[i], currentSpawnCoords,Quaternion.identity);
				myBot.gameObject.name = "Jock Unit " + i;
				
				script_2PBot botScript = myBot.transform.gameObject.GetComponent<script_2PBot>();
				botScript.player2Unit = true;
				
				unitList.Add(myBot.gameObject);
			}
		}
		else if (playerScript.playerType == script_2PPlayer.PlayerType.Jock) //If we are Jocks, spawn jocks in front of us
		{
			//For the number of units we are supposed to have, spawn allies
			for (int i = 0; i < networkCount; i++)
			{
				Vector3 currentSpawnCoords;
				currentSpawnCoords.x = unitSpawnCoords[i].x;
				currentSpawnCoords.y = unitSpawnCoords[i].y;
				currentSpawnCoords.z = unitSpawnCoords[i].z;
				myBot = (GameObject)Instantiate(jockSpawns[i], currentSpawnCoords,Quaternion.identity);
				myBot.gameObject.name = "Jock Unit " + i;
				
				script_2PBot botScript = myBot.transform.gameObject.GetComponent<script_2PBot>();
				botScript.player1Unit = true;
				
				unitList.Add(myBot.gameObject);
			}
			
			//For the number of units we are supposed to have, spawn allies
			for (int i = 0; i < networkCount; i++)
			{
				Vector3 currentSpawnCoords;
				currentSpawnCoords.x = unit2SpawnCoords[i].x;
				currentSpawnCoords.y = unit2SpawnCoords[i].y;
				currentSpawnCoords.z = unit2SpawnCoords[i].z;
				myBot = (GameObject)Instantiate(nerdSpawns[i], currentSpawnCoords,Quaternion.identity);
				myBot.gameObject.name = "Nerd Unit " + i;
				
				script_2PBot botScript = myBot.transform.gameObject.GetComponent<script_2PBot>();
				botScript.player2Unit = true;
				
				unitList.Add(myBot.gameObject);
			}
		}
	}
	
	public void SpawnGameManager()
	{
		gameManager = (GameObject)Instantiate(gameManagerPrefab, new Vector3(0,0,0), Quaternion.identity);						
	}

	//This method is used in the turn system to turn on and off all 
	//of the components that the player will need now that it is his turn
	//First disables all of the other player's components
	public void EnablePlayer1 () {

		#region Disable Player 2
		if (gmScript.turnNumber > 1)
		{
			childCam = player2Inst.transform.FindChild("Camera").gameObject;
			playerCam = childCam.gameObject.GetComponent<Camera>();
			unitCam = player2Inst.transform.FindChild("UnitCam").gameObject.GetComponent<Camera>();
			playerAudio = player2Inst.gameObject.GetComponent<AudioListener>();
			playerScript = player2Inst.gameObject.GetComponent<script_2PPlayer>();
			cam = player2Inst.gameObject.GetComponent<RtsCamera>();
			camKeys = player2Inst.gameObject.GetComponent<RtsCameraKeys>();
			camMouse = player2Inst.gameObject.GetComponent<RtsCameraMouse>();
			effectsUpdater = player2Inst.gameObject.GetComponent<RtsEffectsUpdater>();
			targetScript = player2Inst.gameObject.GetComponent<TargetMover>();
			playerView = player2Inst.gameObject.GetComponent<Camera>();

			playerScript.isMyTurn = false;
			camKeys.enabled = false;
			camMouse.enabled = false;
			cam.enabled = false;
			effectsUpdater.enabled = false;
			targetScript.enabled = false;
			playerCam.enabled = false;
			unitCam.enabled = false;
			playerAudio.enabled = false;
			playerView.enabled = false;

			playerScript.enabled = false;
		}
		#endregion

		#region Enabled Player 1
		if (mainCam != null)
		{
			mainCam = Camera.main;
			GameObject.Destroy(mainCam.gameObject);
		}
		childCam = player1Inst.transform.FindChild("Camera").gameObject;
		playerCam = childCam.gameObject.GetComponent<Camera>();
		unitCam = player1Inst.transform.FindChild("UnitCam").gameObject.GetComponent<Camera>();
		camKeys = player1Inst.gameObject.GetComponent<RtsCameraKeys>();
		camMouse = player1Inst.gameObject.GetComponent<RtsCameraMouse>();
		cam = player1Inst.gameObject.GetComponent<RtsCamera>();
		playerScript = player1Inst.gameObject.GetComponent<script_2PPlayer>();
		effectsUpdater = player1Inst.gameObject.GetComponent<RtsEffectsUpdater>();
		targetScript = player1Inst.gameObject.GetComponent<TargetMover>();
		playerAudio = player1Inst.gameObject.GetComponent<AudioListener>();
		playerView = player1Inst.gameObject.GetComponent<Camera>();

		playerAudio.enabled = true;
		camKeys.enabled = true;
		camMouse.enabled = true;
		cam.enabled = true;
		playerScript.enabled = true;
		effectsUpdater.enabled = true;
		targetScript.enabled = true;
		playerView.enabled = true;

		if (gmScript.turnNumber <= 2)
		{
			cam.MinBounds = playerMinCoords;
			cam.MaxBounds = playerMaxCoords;
			cam.LookAt = player1SpawnCoords;
		}
		
		playerCam.enabled = true;
		unitCam.enabled = true;
		playerScript.isMyTurn = true;
		playerScript.movesRemaining = 5;
		#endregion
	}

	//This method is used in the turn system to turn on and off all 
	//of the components that the player will need now that it is his turn
	//First disables all of the other player's components
	public void EnablePlayer2 () {

		#region Disable Player 1	
		if (gmScript.turnNumber > 1)
		{
			childCam = player1Inst.transform.FindChild("Camera").gameObject;
			playerCam = childCam.gameObject.GetComponent<Camera>();
			unitCam = player1Inst.transform.FindChild("UnitCam").gameObject.GetComponent<Camera>();
			playerAudio = player1Inst.gameObject.GetComponent<AudioListener>();
			playerScript = player1Inst.gameObject.GetComponent<script_2PPlayer>();
			cam = player1Inst.gameObject.GetComponent<RtsCamera>();
			camKeys = player1Inst.gameObject.GetComponent<RtsCameraKeys>();
			camMouse = player1Inst.gameObject.GetComponent<RtsCameraMouse>();
			effectsUpdater = player1Inst.gameObject.GetComponent<RtsEffectsUpdater>();
			targetScript = player1Inst.gameObject.GetComponent<TargetMover>();
			playerView = player1Inst.gameObject.GetComponent<Camera>();
			
			playerScript.isMyTurn = false;
			camKeys.enabled = false;
			camMouse.enabled = false;
			cam.enabled = false;
			effectsUpdater.enabled = false;
			targetScript.enabled = false;
			playerCam.enabled = false;
			unitCam.enabled = false;
			playerAudio.enabled = false;
			playerView.enabled = false;
			
			playerScript.enabled = false;
		}

		#endregion

		#region Enable Player 2
		if (mainCam != null)
		{
			mainCam = Camera.main;
			GameObject.Destroy(mainCam.gameObject);
		}

		childCam = player2Inst.transform.FindChild("Camera").gameObject;
		playerCam = childCam.gameObject.GetComponent<Camera>();
		unitCam = player2Inst.transform.FindChild("UnitCam").gameObject.GetComponent<Camera>();
		playerView = player2Inst.gameObject.GetComponent<Camera>();

		camKeys = player2Inst.gameObject.GetComponent<RtsCameraKeys>();
		camMouse = player2Inst.gameObject.GetComponent<RtsCameraMouse>();
		cam = player2Inst.gameObject.GetComponent<RtsCamera>();
		playerScript = player2Inst.gameObject.GetComponent<script_2PPlayer>();
		effectsUpdater = player2Inst.gameObject.GetComponent<RtsEffectsUpdater>();
		targetScript = player2Inst.gameObject.GetComponent<TargetMover>();
		playerAudio = player2Inst.gameObject.GetComponent<AudioListener>();

		playerView.enabled = true;
		camKeys.enabled = true;
		camMouse.enabled = true;
		cam.enabled = true;
		playerScript.enabled = true;
		effectsUpdater.enabled = true;
		targetScript.enabled = true;
		playerAudio.enabled = true;
		
		if (gmScript.turnNumber <= 2)
		{
			cam.MinBounds = playerMinCoords;
			cam.MaxBounds = playerMaxCoords;
			cam.LookAt = player2SpawnCoords;
		}
	
		if (gmScript.turnNumber == 2)
		{
			cam.Rotation = 180;
		}

		playerCam.enabled = true;
		unitCam.enabled = true;
		playerScript.isMyTurn = true;
		playerScript.movesRemaining = 5;
		#endregion
	}
	
}




