using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script_City : MonoBehaviour {

	// Use this for initialization
	public List<GameObject> connections = new List<GameObject>();

	public List<GameObject> cityList = new List<GameObject>();

	private script_City otherCityScript;

	public float myMinZ;
	public float myMinX;
	public float myMaxZ;
	public float myMaxX; 

	public List<float> cityMinZ = new List<float>();
	public List<float> cityMinX = new List<float>();
	public List<float> cityMaxZ = new List<float>();
	public List<float> cityMaxX = new List<float>();

	public GameObject influenceObj;
	public Collider influenceCollider;

	public GameObject[] buildings;

	public int buildingType;

	public script_GameManager gameManager;

	public Texture icon;

	void Awake () {
		if (this.transform.FindChild("prefab_Influence"))
		{
			influenceObj = this.transform.FindChild("prefab_Influence").gameObject;
		}
		else
		{
			Transform parentObj = this.gameObject.transform.parent;
			influenceObj = parentObj.transform.FindChild("prefab_Influence").gameObject;
		}
		influenceCollider = influenceObj.GetComponent<SphereCollider>();
		myMinZ = influenceCollider.bounds.min.z;
		myMinX = influenceCollider.bounds.min.x;
		myMaxZ = influenceCollider.bounds.max.z;
		myMaxX = influenceCollider.bounds.max.x;
		cityMinX.Add(myMinX);
		cityMinZ.Add(myMinZ);
		cityMaxX.Add(myMaxX);
		cityMaxZ.Add(myMaxZ);
		Debug.Log("This Object: " + this.gameObject.name + " MinX = " + myMinX);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FindBuildings () {

		buildings = GameObject.FindGameObjectsWithTag("Building");

		foreach (GameObject building in buildings)
		{
				//Update our Local Connections
				if (!connections.Contains(building.gameObject))
				{
					connections.Add(building.gameObject);
				}

				//Add all buildings to City List
				if (!cityList.Contains(building.gameObject))
				{
					cityList.Add(building.gameObject);
				}

				//Get the other Object's script and add it's city list as well
				if (building.gameObject.GetComponent<script_City>() != null)
				{
					otherCityScript = building.gameObject.GetComponent<script_City>();
				}
				else
				{
					otherCityScript = building.gameObject.GetComponentInChildren<script_City>();
				}

				foreach (GameObject otherObj in otherCityScript.cityList)
				{
					if (!cityList.Contains(otherObj))
					{
						cityList.Add(otherObj);
					}
				
					//Add all of the other building's Min X Values
					foreach (float minX in otherCityScript.cityMinX)
					{
						if (!cityMinX.Contains(minX))
						{
							cityMinX.Add(minX);
							cityMinX.Sort();
							Debug.Log("City X Min Bounds: " + cityMinX[0]);
						}
					}
				
					//Add all of the other building's Max X Values
					foreach (float maxX in otherCityScript.cityMaxX)
					{
						if (!cityMaxX.Contains(maxX))
						{
							cityMaxX.Add(maxX);
						cityMaxX.Sort();
						}
					}
				
					//Add all of the other building's Min Z Values
					foreach (float minZ in otherCityScript.cityMinZ)
					{
						if (!cityMinZ.Contains(minZ))
						{
							cityMinZ.Add(minZ);
						cityMinZ.Sort();
						}
					}
				
					//Add all of the other building's Max Z Values
					foreach (float maxZ in otherCityScript.cityMaxZ)
					{
						if (!cityMaxZ.Contains(maxZ))
						{
							cityMaxZ.Add(maxZ);
							cityMaxZ.Sort();
						}
					}
				}

		}
	}
}
