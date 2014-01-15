using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script_Tutorial : MonoBehaviour {

	// Use this for initialization
	public string[] text;
	public int textNum;
	public GameObject spaceText;

	public GameObject gameManager;
	private script_GameManager managerScript;
	public GameObject player;
	private script_TerrainControls terrainScript;
	private script_BuildingPlacement buildingScript;

	void Start () {
		this.gameObject.GetComponent<GUIText>().text = text[textNum];
		terrainScript = player.GetComponent<script_TerrainControls>();
		buildingScript = player.GetComponent<script_BuildingPlacement>();
		managerScript = gameManager.GetComponent<script_GameManager>();
	}
	
	// Update is called once per frame
	void Update () {

		if (textNum == 2)
		{
			if (spaceText.guiText.enabled == true)
				spaceText.guiText.enabled = false;

			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
			{
				UpdateText();
			}
		}
		else if (textNum == 3)
		{
			if (spaceText.guiText.enabled == true)
				spaceText.guiText.enabled = false;
			
			if (Input.GetKeyDown(KeyCode.Mouse2))
			{
				UpdateText();
			}
		}
		else if (textNum == 6)
		{
			if (spaceText.guiText.enabled == true)
				spaceText.guiText.enabled = false;

			if (buildingScript.showBuildMenu)
				UpdateText();
		}
		else if (textNum == 7)
		{
			if (buildingScript.building != null)
			{
				if (buildingScript.building.name == "prefab_Palace")
				{
					UpdateText();
				}
				else
				{	
					buildingScript.showBuildMenu = true;
						buildingScript.building = null;
				}
			}
		}
		else if (textNum == 8)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				UpdateText();
			}
		}
		else if (textNum == 10)
		{
			if (spaceText.guiText.enabled == true)
				spaceText.guiText.enabled = false;

			if (buildingScript.manageCity)
			{
				UpdateText();
			}
		}
		else if(textNum == 11)
		{
			if (spaceText.guiText.enabled == true)
				spaceText.guiText.enabled = false;

			if (managerScript.taxRate < 0.2f)
			{
				UpdateText();
			}
		}
		else if (textNum == 12)
		{
			if (spaceText.guiText.enabled == false)
				spaceText.guiText.enabled = true;

			if (Input.GetKeyDown(KeyCode.Space))
			{
				buildingScript.manageCity = false;
				UpdateText();
			}
		}
		else if (textNum == 13)
		{
			if (spaceText.guiText.enabled == true)
				spaceText.guiText.enabled = false;

			if (terrainScript.canChangeTerrainHeight)
			{
				UpdateText();
			}
		}
		else if (textNum == 14)
		{
			if (spaceText.guiText.enabled == false)
				spaceText.guiText.enabled = true;
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				terrainScript.canChangeTerrainHeight = false;
				UpdateText();
			}
		}
		else if (textNum == 15)
		{
			if (spaceText.guiText.enabled == true)
				spaceText.guiText.enabled = false;
			
			if (terrainScript.canChangeTerrainTexture)
			{
				UpdateText();
			}
		}
		else if (textNum == 16)
		{
			if (spaceText.guiText.enabled == false)
				spaceText.guiText.enabled = true;
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				terrainScript.canChangeTerrainTexture = false;
				UpdateText();
			}
		}
		else
		{
			if (spaceText.guiText.enabled == false)
				spaceText.guiText.enabled = true;

			if (Input.GetKeyDown(KeyCode.Space))
			{
				UpdateText();
			}
		}
	}

	void UpdateText()
	{
		if (textNum < text.Length - 1)
			textNum++;
		else
		{
			Destroy(this.gameObject);
		}

		this.gameObject.GetComponent<GUIText>().text = text[textNum];
	}
}
