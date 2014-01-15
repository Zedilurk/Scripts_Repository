using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script_Bot : Photon.MonoBehaviour {

	// Use this for initialization
	public int currentState;
	public MineBotAI botAI;
	
	public List<string> commandQueue;
	
	public PhotonView View;
	
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
	
	void Start () {
		thisScript = this.gameObject.GetComponent<script_Bot>();
		botAI = (MineBotAI)this.gameObject.GetComponent("MineBotAI");
		View = (PhotonView)gameObject.GetComponent<PhotonView>();
		View.observed = thisScript;
		player = GameObject.FindGameObjectWithTag("Player");
		playerScript = player.gameObject.GetComponent<script_Player>();
		targetMover = player.gameObject.GetComponent<TargetMover>();
		distanceDrawn = false;
	}
	
	// Update is called once per frame
	void Update () {
	
		switch (currentState)
		{
			case 0:
			{
				botAI.canSearch = false;
			}
			break;
			
			case 1:
			{
				botAI.canSearch = true;
			}
			break;
		}
		
		if (!View.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
		}
		
		if (selected)
		{
		
			if (!distanceDrawn)
			{
				targetMover.target.position = this.gameObject.transform.position;
				localMoveDistanceObj = (GameObject)Instantiate(moveDistanceObj, this.gameObject.transform.position, this.gameObject.transform.rotation);
				localMoveDistanceObj.gameObject.renderer.enabled = true;
				localMoveDistanceObj.gameObject.transform.localScale = new Vector3(remainingDistance / 5f, 1f, remainingDistance / 5f);
				spotPos = this.gameObject.transform.position;
				
				distanceDrawn = true;
			}
			
			targetSpot = targetMover.target.transform.position;
			
			targetDistance = Vector3.Distance(targetSpot, spotPos);
			
			if (Vector3.Distance(targetSpot, spotPos) < remainingDistance)
			{
				currentState = 1;	
			}
			else
			{
				currentState = 0;	
			}
		}
		else
		{
			if (localMoveDistanceObj != null)
			{
				distanceDrawn = false;
				Destroy(localMoveDistanceObj);	
			}
			
			currentState = 0;
		}

		
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
 
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }
	
}
