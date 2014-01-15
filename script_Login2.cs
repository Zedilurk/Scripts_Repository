using UnityEngine;
using System.Collections;

public class script_Login2 : MonoBehaviour {

	// Use this for initialization
	
	private string formNick = ""; //this is the field where the player will put the name to login
	private string formPassword = ""; //this is his password
	
	string formText = ""; //this field is where the messages sent by PHP script will be in
	string URL = "http://www.benkurdziel.com/login.php"; //change for your URL
	string hash = "9824"; //change your secret code, and remember to change into the PHP file too
	
	private Rect textrect = new Rect (10, 120, 380, 100); //just make a GUI object rectangle
	public GUISkin loginSkin;
	public string successTest;
	private Rect windowRect = new Rect (Screen.width / 2 - 210, Screen.height / 2 - 130, 400, 160);
	public bool showWarning = false;
	
	public GUISkin notNow;
	
	public WorkerMenu2 workerMenu;
	public GameObject nameHolder;
	public script_NameHolder nameScript;
	
	public GameObject UsernameGUI;
	public GameObject PasswordGUI;
	public UILabel usernameLabel;
	public UILabel passwordLabel;
	
	public GameObject loadingText;

//	public void Awake () {
//		uint bankID;
//		AkSoundEngine.LoadBank("Music", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
//	}

	void Start () {
		nameHolder = GameObject.FindGameObjectWithTag("Target");
		nameScript = nameHolder.GetComponent<script_NameHolder>();
		
		usernameLabel = UsernameGUI.gameObject.GetComponent<UILabel>();
		passwordLabel = PasswordGUI.gameObject.GetComponent<UILabel>();

		//PlayMusic();

	}
	
	// Update is called once per frame
	public void OnClick () {
		//if (Input.GetKeyDown(KeyCode.Return))
		//{
		StartCoroutine("Login");
		//}
	}
	
	/*
	void OnGUI() {
		GUI.skin = loginSkin;
		GUI.BeginGroup(new Rect(Screen.width / 2 + (Screen.width / 100), Screen.height / 2 + 10, 400, 260),"");
		GUI.Box(new Rect(0,0, 400,260), "");
	    GUI.Label(new Rect (20, 40, 80, 20), "Username:" ); //text with your nick
	    GUI.Label(new Rect (20, 75, 80, 20), "Password:" );
	
	    formNick = GUI.TextField (new Rect (110, 40, 150, 20), formNick ); //here you will insert the new value to variable formNick
	    formPassword = GUI.PasswordField (new Rect (110, 75, 150, 20), formPassword,"*"[0], 50 ); //same as above, but for password
	
		//Login Button
	    if ( GUI.Button (new Rect (270, 30, 80, 35) , "" ) ){ //just a button
	        StartCoroutine("Login");
	    }
	    
	    //Not Now Button
	    GUI.skin = notNow;
	    if ( GUI.Button (new Rect (270, 65, 80, 35) , "" ) ){ //just a button
	        showWarning = true;
	    }
	    
	    if (showWarning)
	    {
	    	windowRect = GUI.Window (0, windowRect, DoMyWindow, "Skip Login?");
	    }
	    
	    GUI.TextArea( textrect, formText );
	    GUI.EndGroup();
	}
	*/
	
	void DoMyWindow (int windowID) {
	
		if (showWarning)
		{
			GUI.Label(new Rect(10, 25, 380, 60),"Are you sure you want to continue without Logging in? Without logging in, the game cannot store your Personal Statistics.");
		
			if (GUI.Button (new Rect (90,100,100,40), ""))
			{
				Destroy(this.gameObject);
				Application.LoadLevel("ModeSelect");
			}
			
			if (GUI.Button (new Rect (210,100,100,40), ""))
			{
				showWarning = false;
			}
		}
				
	}
	
	IEnumerator Login() {
		Debug.Log("Running Login");
		
		loadingText.gameObject.SetActive(true);
		formNick = usernameLabel.text;
	    formPassword = passwordLabel.text;
		
	    WWWForm form = new WWWForm(); //here you create a new form connection
	    form.AddField( "myform_hash", hash ); //add your hash code to the field myform_hash, check that this variable name is the same as in PHP file
	    form.AddField( "myform_nick", formNick );
	    form.AddField( "myform_pass", formPassword );
	    WWW w = new WWW(URL, form); //here we create a var called 'w' and we sync with our URL and the form
	    yield return w; //we wait for the form to check the PHP file, so our game dont just hang
	    if (w.error != null) {
	    	Debug.Log("There was an error.");
	        print(w.error); //if there is an error, tell us
			loadingText.gameObject.SetActive(false);
	    } else {
	        print("Test ok");
	        formText = w.data; //here we return the data our PHP told us
	        
	        Debug.Log(w.data);
	        
	        if (w.data == successTest)
	        {
	        	PhotonNetwork.player.name = formNick;
				//workerMenu = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<WorkerMenu2>();
				//workerMenu.formNick = formNick;
				nameScript.name = formNick;
	        	Application.LoadLevel("ModeSelect");			
	        }
	        
	        w.Dispose(); //clear our form in game
	    }
	
	    formNick = ""; //just clean our variables
	    formPassword = "";
	}

//	public void PlayMusic () {
//		// Must call "Play Music" to begin any music playback
//		Debug.Log("Playing Music");
//		AkSoundEngine.PostEvent("Play_Music", gameObject);
//		//AkSoundEngine.PostEvent("Set_State_Attack", gameObject);
//		AkSoundEngine.SetRTPCValue("Main_Volume",50);    // 50 is normal volume, below 5 is off, 100 might be very loud
//
//	}
	
}
