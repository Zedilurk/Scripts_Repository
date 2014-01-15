using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script_2PPlayer : MonoBehaviour {
	
	// Use this for initialization
	
	public float camSpeed;
	public Camera mainCam;
	
	private Ray leftClickRay;
	private RaycastHit leftClickHit;
	private Vector2 mousePosDown;
	private float mouseXDown;
	private float mouseYDown;
	private Vector3 mouse3DHitDown;
	
	private Ray leftUpRay;
	private RaycastHit leftUpHit;
	private Vector2 mousePosUp;
	private float mouseXUp;
	private float mouseYUp;
	private Vector3 mouse3DHitUp;
	public Vector3 publicLeftHit;
	
	private float xDif;
	private float yDif;
	
	private Vector2 screenPointClick;
	private Vector2 screenPointUp;
	
	private float screenXDif;
	private float screenYDif;
	
	public GameObject[] units;
	private Vector2 positionCheck;
	public GameObject selectedUnit;
	private script_2PBot tempUnitControl;
	public UnitStats unitStats;
	
	public string adultOrChild;
	
	public GameObject player1Obj;
	
	public Shader hiLightShader;
	public Shader defaultShader;
	public Shader enemyShader;
	private Renderer tempRend;
	
	public enum PlayerType {Nerd, Jock};
	public PlayerType playerType;
	
	public bool isMyTurn;
	
	//Turn Variables
	public Vector3 startPos;
	public bool movingUnit;
	public int movesRemaining;
	
	//Attacking Variables
	public bool showAttack;
	
	public GameObject targetEnemy;
	public bool attackMode;

	public script_2PGameManager gameManager;

	public GameObject attackEffect;

	public HighlightableObject tempHL;
	public Color highlightColor;

	public GUISkin confirm;
	public GUISkin deny;

	public LayerMask nonTileMask;

	public TargetMover targetScript;

	void Start () {
		mainCam = this.GetComponent<Camera>();
		gameManager = GameObject.Find("prefab_2PGameManager(Clone)").gameObject.GetComponent<script_2PGameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
		//If it's my turn
		if (isMyTurn)
		{
			//And we LEFT click
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{		
				mousePosUp = Input.mousePosition;
				mouseXUp = mousePosUp.x;
				mouseYUp = mousePosUp.y;
				leftUpRay = mainCam.ScreenPointToRay(new Vector3(mouseXUp, mouseYUp, 0));
				Debug.DrawRay (leftUpRay.origin, leftUpRay.direction * 800, Color.yellow);
				
				//And that click is on a 
				if (Physics.Raycast(leftUpRay, out leftUpHit, 800, nonTileMask))
				{
					mouse3DHitUp = leftUpHit.point;
					publicLeftHit = leftUpHit.point;
					Debug.Log(leftUpHit.collider.gameObject.name);
					
					GameObject tempHit = leftUpHit.transform.gameObject;
					script_2PBot tempBot = tempHit.transform.gameObject.GetComponent<script_2PBot>();
					
					//Game object on the "Child" layer
					if (tempHit.gameObject.tag == "Child")
					{
						if (selectedUnit != null)
						{
							if (tempUnitControl.targetMover.target != null)
							{
								//Deselect All Units
								selectedUnit.transform.position = startPos;
								
								//Disable the Unit's Movement
								tempUnitControl = (script_2PBot)selectedUnit.GetComponent("script_2PBot");
								tempUnitControl.selected = false;
								tempUnitControl.selectedBy = null;
								tempUnitControl.currentState = 0;
								tempUnitControl.targetMover.target.transform.position = selectedUnit.transform.position;
								
								//Reset our variables
								movingUnit = false;
							}

							ClearUnits();
						}

						
						//If this is OUR unit and not the enemy's
						if (gameManager.player1Turn)
						{
							if (tempBot.player1Unit == true)
							{
								//Select Unit
								selectedUnit = tempHit;
								targetScript.selectedUnit = selectedUnit;
								unitStats = selectedUnit.GetComponent<UnitStats>();
								tempUnitControl = (script_2PBot)tempHit.GetComponent("script_2PBot");
								tempUnitControl.targetMover.target.transform.position = selectedUnit.transform.position;
								tempUnitControl.selected = true;
								tempUnitControl.selectedBy = this.gameObject;
								//tempUnitControl.currentState = 1;
								
								//Hi-light the unit
								tempRend = tempHit.gameObject.GetComponentInChildren<Renderer>();
								//tempRend.material.shader = hiLightShader;	

								tempHL = selectedUnit.gameObject.GetComponent<HighlightableObject>();
								tempHL.ConstantOn(highlightColor);

								//Get the unit's Starting Position
								startPos = tempHit.gameObject.transform.position;
								if (tempUnitControl.canMove)
								{
									movingUnit = true;
								}
								showAttack = false;
							}
							else //This is the enemy's unit
							{
								Debug.Log("Enemy Unit hit.");

//								if (movingUnit)
//								{
//									showAttack = true;
//									tempUnitControl.botAI.target.position = tempHit.transform.position;
//								}
								//else
								//{
									selectedUnit = tempHit;
									targetScript.selectedUnit = selectedUnit;
									unitStats = selectedUnit.GetComponent<UnitStats>();
									tempUnitControl = (script_2PBot)tempHit.GetComponent("script_2PBot");
									Debug.Log("Selecting unit.");
									tempUnitControl.selected = true;
									tempUnitControl.currentState = 0;

									tempUnitControl.selectedBy = this.gameObject;
									
									tempRend = tempHit.gameObject.GetComponentInChildren<Renderer>();
									//tempRend.material.shader = enemyShader;

									tempHL = selectedUnit.gameObject.GetComponent<HighlightableObject>();
									tempHL.ConstantOn(Color.red);
								//}
							}
						}
						else if (gameManager.player2Turn)
						{
							if (tempBot.player2Unit == true)
							{
								//Select Unit
								selectedUnit = tempHit;
								targetScript.selectedUnit = selectedUnit;
								unitStats = selectedUnit.GetComponent<UnitStats>();
								tempUnitControl = (script_2PBot)tempHit.GetComponent("script_2PBot");
								tempUnitControl.targetMover.target.transform.position = selectedUnit.transform.position;
								tempUnitControl.selected = true;
								tempUnitControl.selectedBy = this.gameObject;
								//tempUnitControl.currentState = 1;
								
								//Hi-light the unit
								tempRend = tempHit.gameObject.GetComponentInChildren<Renderer>();
								//tempRend.material.shader = hiLightShader;	

								tempHL = selectedUnit.gameObject.GetComponent<HighlightableObject>();
								tempHL.ConstantOn(highlightColor);

								//Get the unit's Starting Position
								startPos = tempHit.gameObject.transform.position;
								if (tempUnitControl.canMove)
								{
									movingUnit = true;
								}
								showAttack = false;
							}
							else //This is the enemy's unit
							{
								Debug.Log("Enemy Unit hit.");
//								if (movingUnit)
//								{
//									showAttack = true;
//									tempUnitControl.botAI.target.position = tempHit.transform.position;
//								}
//								else
//								{
									selectedUnit = tempHit;
									targetScript.selectedUnit = selectedUnit;
									unitStats = selectedUnit.GetComponent<UnitStats>();
									tempUnitControl = (script_2PBot)tempHit.GetComponent("script_2PBot");
									tempUnitControl.selected = true;
									tempUnitControl.selectedBy = this.gameObject;
									tempUnitControl.currentState = 0;
									
									tempRend = tempHit.gameObject.GetComponentInChildren<Renderer>();
									//tempRend.material.shader = enemyShader;

									Debug.Log("Highlighting");
									tempHL = selectedUnit.gameObject.GetComponent<HighlightableObject>();
									tempHL.ConstantOn(Color.red);
								//}
							}
						}
					}
					else // This is not a unit, so clear everything
					{
						Debug.Log("Clearing All");
						if (GUIUtility.hotControl == 0)
						{
							//Send the unit back to the starting spot
							if (selectedUnit != null)
							{
								if (tempUnitControl.selectedBy == tempUnitControl.ourPlayer)
								{
									selectedUnit.transform.position = startPos;
								}

								//Disable the Unit's Movement
								tempUnitControl = (script_2PBot)selectedUnit.GetComponent("script_2PBot");
								tempUnitControl.selected = true;
								tempUnitControl.currentState = 0;
									
								//Reset our variables
								movingUnit = false;
								showAttack = false;
								ClearUnits();
							}
						}
					}
				}	
			}
			
			//If we RIGHT click
			if (Input.GetKeyDown(KeyCode.Mouse1))
			{		
				mousePosUp = Input.mousePosition;
				mouseXUp = mousePosUp.x;
				mouseYUp = mousePosUp.y;
				leftUpRay = mainCam.ScreenPointToRay(new Vector3(mouseXUp, mouseYUp, 0));
				Debug.DrawRay (leftUpRay.origin, leftUpRay.direction * 800, Color.yellow);
				
				//And that click is on a 
				if (Physics.Raycast(leftUpRay, out leftUpHit, 800))
				{
					mouse3DHitUp = leftUpHit.point;
					publicLeftHit = leftUpHit.point;
					Debug.Log(leftUpHit.collider.gameObject.name);
					
					GameObject tempHit = leftUpHit.transform.gameObject;
					script_2PBot tempBot = tempHit.transform.gameObject.GetComponent<script_2PBot>();

					//we hit a unit
					if (tempHit.gameObject.tag == "Child")
					{
						//we have a unit selected
						if (selectedUnit != null)
						{
							//we are a jock
							if (playerType == PlayerType.Jock)
							{
								//we selected a nerd
								if (tempHit.gameObject.name.Contains("Nerd"))
								{
									//our selected unit is a jock
									if (selectedUnit.gameObject.name.Contains("Jock"))
									{
										Debug.Log("Nerd Hit");
										targetEnemy = tempHit.gameObject;

										if (Vector3.Distance(selectedUnit.gameObject.transform.position, targetEnemy.transform.position) < tempUnitControl.remainingDistance)
										{
											if (tempUnitControl.canAttack)
											{
												tempUnitControl.selected = true;
												tempUnitControl.currentState = 1;
												
												//Get the unit's Starting Position
												startPos = tempHit.gameObject.transform.position;
												
												tempUnitControl.targetMover.target.transform.position = targetEnemy.transform.position;
												attackMode = true;
												AkSoundEngine.PostEvent("Play_AttackSelect", this.gameObject);
												AkSoundEngine.PostEvent("Set_State_Attack", this.gameObject);
												Destroy(tempUnitControl.localMoveDistanceObj);
											}
										}
									}
								}
							}
							else if (playerType == PlayerType.Nerd) 
							{
								if (tempHit.gameObject.name.Contains("Jock"))
								{
									if (selectedUnit.gameObject.name.Contains("Nerd"))
									{
										Debug.Log("Jock Hit");
										targetEnemy = tempHit.gameObject;

										if (Vector3.Distance(selectedUnit.gameObject.transform.position, targetEnemy.transform.position) < tempUnitControl.remainingDistance)
										{
											if (tempUnitControl.canAttack)
											{
												tempUnitControl.selected = true;
												tempUnitControl.currentState = 1;
												
												//Get the unit's Starting Position
												startPos = tempHit.gameObject.transform.position;
												
												tempUnitControl.targetMover.target.transform.position = targetEnemy.transform.position;
												attackMode = true;
												AkSoundEngine.PostEvent("Play_AttackSelect", this.gameObject);
												AkSoundEngine.PostEvent("Set_State_Attack", this.gameObject);
												Destroy(tempUnitControl.localMoveDistanceObj);
											}
										}
									}
								}
							}
						}
						
					}
				}
				
			} //End of Right Click

			if (attackMode)
			{
				if (selectedUnit != null)
				{
					float distance = Vector3.Distance(selectedUnit.gameObject.transform.position, targetEnemy.gameObject.transform.position);

					if (distance < unitStats.AtkRng)
					{
						script_2PBot botScript = selectedUnit.gameObject.GetComponent<script_2PBot>();

						if (botScript.canAttack)
						{
							Vector3 spawnSpot = new Vector3(targetEnemy.transform.position.x, targetEnemy.transform.position.y + 3, targetEnemy.transform.position.z);
							Instantiate(attackEffect, spawnSpot, Quaternion.identity);
							tempUnitControl.targetMover.target.transform.position = selectedUnit.transform.position;
							tempUnitControl.currentState = 0;
							Debug.Log("Attacking!!!!");
							UnitStats enemyScript = targetEnemy.gameObject.GetComponent<UnitStats>();
							enemyScript.CurrentHealth -= unitStats.AtkDmg;
							attackMode = false;
							movingUnit = false;
							botScript.currentState = 0;
							botScript.canAttack = false;
							ClearUnits();
						}

					}
				}
			}

			if (movingUnit)
			{
				if (movesRemaining > 0)
				{
					if (Input.GetKeyDown(KeyCode.Space))
					{
						if (Vector3.Distance(selectedUnit.transform.position, startPos) > 1)
						{
							movesRemaining--;
						}
						
						//Disable the Unit's Movement
						tempUnitControl = (script_2PBot)selectedUnit.GetComponent("script_2PBot");
						tempUnitControl.selected = true;
						tempUnitControl.currentState = 0;
						
						if (Vector3.Distance(tempUnitControl.targetSpot, tempUnitControl.spotPos) < tempUnitControl.remainingDistance)
						{
							Vector3 newPos = new Vector3(tempUnitControl.targetMover.target.transform.position.x, tempUnitControl.gameObject.transform.position.y, tempUnitControl.targetMover.target.transform.position.z);
							tempUnitControl.gameObject.transform.position = newPos;
						}
						
						//Reset our variables
						movingUnit = false;
						tempUnitControl.canMove = false;
						ClearUnits();
						AkSoundEngine.PostEvent("Play_Confirm", this.gameObject);
					}
				}
			}


			//Constant Update for Highlighting
			mousePosUp = Input.mousePosition;
			mouseXUp = mousePosUp.x;
			mouseYUp = mousePosUp.y;
			leftUpRay = mainCam.ScreenPointToRay(new Vector3(mouseXUp, mouseYUp, 0));
			Debug.DrawRay (leftUpRay.origin, leftUpRay.direction * 800, Color.yellow);
			
			//And that click is on a 
			if (Physics.Raycast(leftUpRay, out leftUpHit, 800))
			{
				mouse3DHitUp = leftUpHit.point;
				publicLeftHit = leftUpHit.point;
				//Debug.Log(leftUpHit.collider.gameObject.name);
				
				GameObject tempHit = leftUpHit.transform.gameObject;
				script_2PBot tempBot = tempHit.transform.gameObject.GetComponent<script_2PBot>();
				
				//Game object on the "Child" layer
				if (tempHit.gameObject.tag == "Child")
				{
					if (playerType == PlayerType.Nerd) 
					{
						if (tempHit.gameObject.name.Contains("Nerd"))
						{
							tempHL = tempHit.gameObject.GetComponent<HighlightableObject>();
							tempHL.On(highlightColor);
						}

						if (tempHit.gameObject.name.Contains("Jock"))
						{
							tempHL = tempHit.gameObject.GetComponent<HighlightableObject>();
							tempHL.On(Color.red);
						}
					}

					if (playerType == PlayerType.Jock) 
					{
						if (tempHit.gameObject.name.Contains("Jock"))
						{
							tempHL = tempHit.gameObject.GetComponent<HighlightableObject>();
							tempHL.On(highlightColor);
						}
						
						if (tempHit.gameObject.name.Contains("Nerd"))
						{
							tempHL = tempHit.gameObject.GetComponent<HighlightableObject>();
							tempHL.On(Color.red);
						}
					}
				}
			}
		}
	}
	
	void OnGUI () {
		//GUI.Label(new Rect(Screen.width / 2 - ((Screen.width / 4) / 2), 100, Screen.width / 4, 50), "Turns Remaining: " + movesRemaining);
		
		//Turn Based Movement
		if (movingUnit)
		{
			//showAttack = false;
			
			if (movesRemaining > 0)
			{
				//Confirm Button
				GUI.skin = confirm;
				if (GUI.Button(new Rect(Screen.width / 2 + Screen.width / 7, Screen.height/1.25f, Screen.width / 9, Screen.height / 10), ""))
				{
					if (Vector3.Distance(selectedUnit.transform.position, startPos) > 2)
					{
						movesRemaining--;
					}
					
					//Disable the Unit's Movement
					tempUnitControl = (script_2PBot)selectedUnit.GetComponent("script_2PBot");
					tempUnitControl.selected = true;
					tempUnitControl.currentState = 0;
					
					if (Vector3.Distance(tempUnitControl.targetSpot, tempUnitControl.spotPos) < tempUnitControl.remainingDistance)
					{
						Vector3 newPos = new Vector3(tempUnitControl.targetMover.target.transform.position.x, tempUnitControl.gameObject.transform.position.y, tempUnitControl.targetMover.target.transform.position.z);
						tempUnitControl.gameObject.transform.position = newPos;
					}
					AkSoundEngine.PostEvent("Play_Confirm", this.gameObject);
					//Reset our variables
					movingUnit = false;
					tempUnitControl.canMove = false;
					ClearUnits();				
				}
				GUI.skin = null;

				//Deny Button
				GUI.skin = deny;
				if (GUI.Button(new Rect(Screen.width / 2 + Screen.width / 7, Screen.height/1.13f, Screen.width / 9, Screen.height / 10), ""))
				{
					//Send the unit back to the starting spot
					selectedUnit.transform.position = startPos;
					
					//Disable the Unit's Movement
					tempUnitControl = (script_2PBot)selectedUnit.GetComponent("script_2PBot");
					tempUnitControl.selected = false;
					tempUnitControl.selectedBy = null;
					tempUnitControl.currentState = 0;
					tempUnitControl.targetMover.target.transform.position = selectedUnit.transform.position;
					
					//Reset our variables
					movingUnit = false;
					ClearUnits();
				}
				GUI.skin = null;
			}
			else
			{
				
			}
		}
		
		//Attack
		if (showAttack)
		{
			movingUnit = false;
			
			if (GUI.Button(new Rect(50, 50, 100, 30), "Attack"))
			{
				
			}
			
			if (GUI.Button(new Rect(50, 100, 100, 30), "Cancel"))
			{
				
			}
		}
		
		
	}
	
	void UpdateUnits () {
		units = GameObject.FindGameObjectsWithTag(adultOrChild);	
	}
	
	public void ClearUnits () {
		//Clear all Selected Units
		Debug.Log("Clearing Units");
		if(selectedUnit != null)
		{
			tempRend = selectedUnit.gameObject.GetComponentInChildren<Renderer>();
			//tempRend.material.shader = Shader.Find("Self-Illumin/Bumped Diffuse");

			tempHL = selectedUnit.gameObject.GetComponent<HighlightableObject>();
			tempHL.Off();
			
			tempUnitControl = (script_2PBot)selectedUnit.GetComponent("script_2PBot");
			tempUnitControl.selected = false;
			tempUnitControl.selectedBy = null;
			tempUnitControl.currentState = 0;
			tempUnitControl.distanceDrawn = false;
			Destroy (tempUnitControl.localMoveDistanceObj);
			selectedUnit = null;
			unitStats = null;
		}
	}

	public void ResetUnit () {

		Debug.Log("Resetting Units");

		if (selectedUnit != null)
		{
			//Disable the Unit's Movement
			tempUnitControl = (script_2PBot)selectedUnit.GetComponent("script_2PBot");
			tempUnitControl.selected = false;
			tempUnitControl.selectedBy = null;
			tempUnitControl.currentState = 0;
			tempUnitControl.targetMover.target.transform.position = selectedUnit.transform.position;

			if (tempUnitControl.selectedBy == tempUnitControl.ourPlayer)
			{
				//Send the unit back to the starting spot
				selectedUnit.transform.position = startPos;
			}

			//Reset our variables
			movingUnit = false;
			ClearUnits();
		}
	}

}
