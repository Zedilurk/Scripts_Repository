using UnityEngine;
using System.Collections;

public class script_2PGameManager : Photon.MonoBehaviour {
	
	// Use this for initialization
	public int turnNumber = 1;
	
	public enum Player1Type {Nerd, Jock};
	public enum Player2Type {Nerd, Jock};
	public Player1Type player1Type;
	public Player2Type player2Type;
	
	public enum TurnName {player1Turn, player2Turn};
	public TurnName turnName;
	
	private bool playersAssigned = false;
	private int i = 0;
	
	public GameObject myPlayer;
	
	public script_2PSetup setupScript;
	
	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	
	public GameObject player1;
	public script_2PPlayer player1Script;

	public GameObject player2;
	public script_2PPlayer player2Script;

	public bool player1Turn;
	public bool player2Turn;

	public GUISkin nextTurn;

	public GameObject[] unitList;
	
	public GameObject player1Banner;
	public GameObject player2Banner;

	public Vector3 bannerStart;
	public Vector3 bannerMid;
	public Vector3 bannerEnd;

	public bool player1In;
	public bool player1Out;

	public bool player2In;
	public bool player2Out;

	public float speed = 1.0F;
	public float startTime;
	public float journeyLength;

	public Texture golfAlive;
	public Texture baseballAlive;
	public Texture footballAlive;
	public Texture soccerAlive;
	public Texture golfDead;
	public Texture baseballDead;
	public Texture footballDead;
	public Texture soccerDead;

	public Texture techAlive;
	public Texture ddAlive;
	public Texture calcAlive;
	public Texture skateAlive;
	public Texture techDead;
	public Texture ddDead;
	public Texture calcDead;
	public Texture skateDead;

	public bool[] jockAlive;
	public bool[] nerdAlive;
	
	public void Awake () {
		uint bankID;
		AkSoundEngine.LoadBank("Music", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
	}
	
	void Start () {
		turnNumber = 1;
		setupScript = GameObject.Find("prefab_2PSetup").GetComponent<script_2PSetup>();
		player1 = GameObject.Find("Player 1");
		player1Script = player1.GetComponent<script_2PPlayer>();

		player2 = GameObject.Find("Player 2");
		player2Script = player2.GetComponent<script_2PPlayer>();
		
		AssignPlayers();
		
		if (turnNumber == 1)
		{
			Debug.Log("Setting Player 1's Turn");
			player1Script.movesRemaining = 5;
			player2Turn = false;

			setupScript.EnablePlayer1();
			player1Script.isMyTurn = true;
			player1Turn = true;

			StartCoroutine("Player1Banner");

			if (player1Type == Player1Type.Jock)
			{
				AkSoundEngine.PostEvent("Set_Team_Jocks", gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Set_Team_Nerds", gameObject);
			}

			NewTurnSound();
		}
		
		if(turnNumber%2==0)
		{
			//Even Turn
			Debug.Log("Setting Player 2's Turn.");
			turnName = TurnName.player2Turn;
			player1Turn = false;

			player1Script.isMyTurn = false;
			setupScript.EnablePlayer2();
			player2Script.isMyTurn = true;
			player2Script.movesRemaining = 5;
			player2Turn = true;

			StartCoroutine("Player2Banner");

			if (player2Type == Player2Type.Jock)
			{
				AkSoundEngine.PostEvent("Set_Team_Jocks", gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Set_Team_Nerds", gameObject);
			}

		}
		else if (turnNumber%2==1)
		{
			//Odd Turn
			Debug.Log("Setting Player 1's Turn.");
			turnName = TurnName.player1Turn;	
			player2Script.isMyTurn = false;
			player2Turn = false;

			setupScript.EnablePlayer1();
			player1Turn = true;
			player1Script.isMyTurn = true;
			player1Script.movesRemaining = 5;

			StartCoroutine("Player1Banner");

			if (player1Type == Player1Type.Jock)
			{
				AkSoundEngine.PostEvent("Set_Team_Jocks", gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Set_Team_Nerds", gameObject);
			}
		}
		
		PlayMusic();
		
	}
	
	void OnGUI () {
		
		//GUI.Label(new Rect(Screen.width / 2,20,20,20), "" + turnNumber);

		//Next Turn Button
		if (player1Script.isMyTurn == true)
		{
			if (!player1Script.attackMode && !player1Script.movingUnit)
			{
				GUI.skin = nextTurn;
				if (GUI.Button(new Rect(Screen.width / 140,Screen.height*3/4.5f, Screen.width / 10, Screen.height / 12), ""))
				{
					if (player1Turn == true)
					{
						if (player1Script.selectedUnit != null)
						{
							player1Script.selectedUnit.transform.position = player1Script.startPos;
							script_2PBot tempUnitControl = (script_2PBot)player1Script.selectedUnit.GetComponent("script_2PBot");
							tempUnitControl.selected = false;
							tempUnitControl.currentState = 0;
							
							//Reset our variables
							player1Script.movingUnit = false;
						}

						NewTurnSound();
						player1Script.ResetUnit();
						turnNumber++;
						UpdateTurns();
					}
				}
				GUI.skin = null;
			}

		}

		//Next Turn Button
		if (player2Script.isMyTurn == true)
		{
			if (!player2Script.attackMode && !player2Script.movingUnit)
			{
				GUI.skin = nextTurn;
				if (GUI.Button(new Rect(Screen.width / 140,Screen.height*3/4.5f, Screen.width / 10, Screen.height / 12), ""))
				{
					if (player2Turn == true)
					{
						if (player2Script.selectedUnit != null)
						{
							player2Script.selectedUnit.transform.position = player2Script.startPos;
							script_2PBot tempUnitControl = (script_2PBot)player2Script.selectedUnit.GetComponent("script_2PBot");
							tempUnitControl.selected = false;
							tempUnitControl.currentState = 0;
							
							//Reset our variables
							player2Script.movingUnit = false;
						}

						NewTurnSound();
						player2Script.ResetUnit();
						turnNumber++;
						UpdateTurns();
					}

				}

				GUI.skin = null;
			}
		}

		//Portraits

		if (jockAlive[0])
		{
			GUI.DrawTexture(new Rect(Screen.width/3.48f,Screen.height / 20, Screen.width/30, Screen.width/30), baseballAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width/3.48f,Screen.height / 20, Screen.width/30, Screen.width/30), baseballDead);
		}

		if (jockAlive[1])
		{
			GUI.DrawTexture(new Rect(Screen.width/3.13f,Screen.height / 20, Screen.width/30, Screen.width/30), soccerAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width/3.13f,Screen.height / 20, Screen.width/30, Screen.width/30), soccerDead);
		}

		if (jockAlive[2])
		{
			GUI.DrawTexture(new Rect(Screen.width/2.84f,Screen.height / 20, Screen.width/30, Screen.width/30), footballAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width/2.84f,Screen.height / 20, Screen.width/30, Screen.width/30), footballDead);
		}

		if (jockAlive[3])
		{
			GUI.DrawTexture(new Rect(Screen.width/2.6f,Screen.height / 20, Screen.width/30, Screen.width/30), golfAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width/2.6f,Screen.height / 20, Screen.width/30, Screen.width/30), golfDead);
		}

		if (jockAlive[4])
		{
			GUI.DrawTexture(new Rect(Screen.width/2.4f,Screen.height / 20, Screen.width/30, Screen.width/30), baseballAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width/2.4f,Screen.height / 20, Screen.width/30, Screen.width/30), baseballDead);
		}

		if (nerdAlive[0])
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/3.12f,Screen.height / 20, Screen.width/30, Screen.width/30), calcAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/3.12f,Screen.height / 20, Screen.width/30, Screen.width/30), calcDead);
		}

		if (nerdAlive[1])
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2.83f,Screen.height / 20, Screen.width/30, Screen.width/30), skateAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2.83f,Screen.height / 20, Screen.width/30, Screen.width/30), skateDead);
		}

		if (nerdAlive[2])
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2.59f,Screen.height / 20, Screen.width/30, Screen.width/30), ddAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2.59f,Screen.height / 20, Screen.width/30, Screen.width/30), ddDead);
		}

		if (nerdAlive[3])
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2.38f,Screen.height / 20, Screen.width/30, Screen.width/30), techAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2.38f,Screen.height / 20, Screen.width/30, Screen.width/30), techDead);
		}

		if (nerdAlive[4])
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2.2f,Screen.height / 20, Screen.width/30, Screen.width/30), calcAlive);
		}
		else
		{
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2.2f,Screen.height / 20, Screen.width/30, Screen.width/30), calcDead);
		}
		//GUI.Label(new Rect(100, 10, 20, 20), "" + turnNumber);
	}
	
	void AssignPlayers () {
		#region Assign Player Types
		if (player1Script.playerType == script_2PPlayer.PlayerType.Nerd)
		{
			player1Type = Player1Type.Nerd;
			player2Type = Player2Type.Jock;
		}
		else
		{
			player1Type = Player1Type.Jock;
			player2Type = Player2Type.Nerd;
		}
		#endregion
	}
	
	public void UpdateTurns () {
		if (turnNumber == 1)
		{
			Debug.Log("Setting Player 1's Turn");
			turnName = TurnName.player1Turn;
			player2Turn = false;
			player2Script.isMyTurn = false;

			setupScript.EnablePlayer1();

			StartCoroutine("EnablePlayer1");
			StartCoroutine("Player1Banner");

		}
		
		if(turnNumber%2==0)
		{
			Debug.Log("Setting Player 2's Turn.");
			turnName = TurnName.player2Turn;
			player1Script.isMyTurn = false;
			player1Turn = false;

			setupScript.EnablePlayer2();

			StartCoroutine("EnablePlayer2");
			StartCoroutine("Player2Banner");

			if (player2Type == Player2Type.Jock)
			{
				AkSoundEngine.PostEvent("Set_Team_Jocks", gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Set_Team_Nerds", gameObject);
			}
		}
		else if (turnNumber%2==1)
		{
			//Nerd Turn
			//Odd Turn
			Debug.Log("Setting Player 1's Turn.");
			turnName = TurnName.player1Turn;	
			player2Script.isMyTurn = false;
			player2Turn = false;

			setupScript.EnablePlayer1();

			StartCoroutine("EnablePlayer1");
			StartCoroutine("Player1Banner");

			if (player1Type == Player1Type.Jock)
			{
				AkSoundEngine.PostEvent("Set_Team_Jocks", gameObject);
			}
			else
			{
				AkSoundEngine.PostEvent("Set_Team_Nerds", gameObject);
			}
		}

		unitList = GameObject.FindGameObjectsWithTag("Child");

		foreach (GameObject unit in unitList)
		{
			script_2PBot botScript = unit.gameObject.GetComponent<script_2PBot>();

			if (botScript.disabled == false)
			{
				botScript.canAttack = true;
				botScript.canMove = true;
			}
		}
	}

	IEnumerator EnablePlayer1()
	{
		yield return new WaitForSeconds(1);
		player1Turn = true;
		player1Script.isMyTurn = true;
		player1Script.movesRemaining = 5;

		StopCoroutine("EnablePlayer1");
	}

	IEnumerator EnablePlayer2()
	{
		yield return new WaitForSeconds(1);
		player2Turn = true;
		player2Script.movesRemaining = 5;
		player2Script.isMyTurn = true;

		StopCoroutine("EnablePlayer2");
	}
	
	public void PlayMusic () {
		// Must call "Play Music" to begin any music playback
		Debug.Log("Playing Music");
		AkSoundEngine.PostEvent("Play_Music", gameObject);
		//AkSoundEngine.PostEvent("Set_State_Attack", gameObject);
		AkSoundEngine.SetRTPCValue("Main_Volume",50);    // 50 is normal volume, below 5 is off, 100 might be very loud
	}

	public void NewTurnSound () {
		AkSoundEngine.PostEvent("Play_NewTurn_Player", gameObject);
	}

	public IEnumerator Player1Banner () {
		startTime = Time.time;
		journeyLength = Vector3.Distance(bannerStart, bannerMid);
		player1In = true;

		yield return new WaitForSeconds(3);

		player1In = false;
		startTime = Time.time;
		journeyLength = Vector3.Distance(bannerMid, bannerEnd);
		player1Out = true;

		yield return new WaitForSeconds(3);
		player1Out = false;
		StopCoroutine("Player1Banner");
	}

	public IEnumerator Player2Banner () {
		startTime = Time.time;
		journeyLength = Vector3.Distance(bannerStart, bannerMid);
		player2In = true;
		
		yield return new WaitForSeconds(4);
		
		player2In = false;
		startTime = Time.time;
		journeyLength = Vector3.Distance(bannerMid, bannerEnd);
		player2Out = true;

		yield return new WaitForSeconds(3);
		player2Out = false;
		StopCoroutine("Player1Banner");
	}

	void Update () {

		if (player1In)
		{
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			player1Banner.transform.position = Vector3.Lerp(bannerStart, bannerMid, fracJourney);
		}

		if (player1Out)
		{
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			player1Banner.transform.position = Vector3.Lerp(bannerMid, bannerEnd, fracJourney);
		}

		if (player2In)
		{
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			player2Banner.transform.position = Vector3.Lerp(bannerStart, bannerMid, fracJourney);
		}
		
		if (player2Out)
		{
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			player2Banner.transform.position = Vector3.Lerp(bannerMid, bannerEnd, fracJourney);
		}
	}
}
