using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour	
{
	//public script_Unit modelUnit;
	public GameObject modelUnit;
	public List<script_Unit> poolList;
	private int unitCount;
	public GameObject offscreenHoldingArea;

	public int unitsInPool;
	
	void Awake()
	{
		LoadPool();
		offscreenHoldingArea = this.transform.gameObject;
		InvokeRepeating("FillPool", 5.0f, 1.0f);
	}

	void Timer() {
		FillPool();
	}
	
	public void LoadPool()
	{
		//modelUnit = this.transform.FindChild("prefab_Citizen").GetComponent<script_Unit>();
		modelUnit = this.transform.FindChild("prefab_Citizen").transform.gameObject;
		modelUnit.transform.position = offscreenHoldingArea.transform.position;
		
		poolList = new List<script_Unit>();

		//Fill Pool
		for ( int c = 0; c < 25; ++c )
		{
			//Debug.Log("Filling Pool");
			GameObject newUnit = (GameObject)Instantiate(modelUnit.gameObject);
			newUnit.transform.parent = transform;	
			//newy.name = "PooledUnit" + c;
			poolList.Add(newUnit.GetComponent<script_Unit>());
		}

		unitCount = poolList.Count;
		unitsInPool = unitCount;
	}

	public void FillPool()
	{
		//If we don't have 25 Units in our Pool
		if (poolList.Count < 25)
		{		
			//Fill Pool from our Current # to 25

			/* 
			 * Currently Commented out because it is too intense to fill the entire missing pool in 1 Method
			for ( int c = poolList.Count; c < 25; ++c )
			{
				GameObject newUnit = (GameObject)Instantiate(modelUnit.gameObject);
				newUnit.transform.parent = transform;	
				//newy.name = "PooledUnit" + c;
				poolList.Add(newUnit.GetComponent<script_Unit>());
			}
			*/

			//Spawns 1 Every Time this Runs
			//Each Tick of Invoked Method "Timer"
			if (poolList.Count < 25)
			{
				GameObject newUnit = (GameObject)Instantiate(modelUnit.gameObject);
				newUnit.transform.parent = transform;	
				//newy.name = "PooledUnit" + c;
				poolList.Add(newUnit.GetComponent<script_Unit>());
			}
			
			unitCount = poolList.Count;
			unitsInPool = unitCount;
		}
	}

}