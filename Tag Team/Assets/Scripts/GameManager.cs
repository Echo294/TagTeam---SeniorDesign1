using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	// make game manager public static so can access this from other scripts
	public static GameManager gm;

	// public variables
	[Header("=======Basic Attributes=======")]
	public int score=0;
	public bool canBeatLevel = false;
	public int beatLevelScore=0;
	public float startTime=10.0f;
	public AudioSource musicAudioSource;
	public bool gameIsOver = false;

	[Header("=======Main Canvas=======")]
	public GameObject mainCanvas;
	public Text mainScoreDisplay;
	public Text mainTimerDisplay;

	[Header("=======Game Over Canvas=======")]
	public GameObject gameOverCanvas;
	public Button restartButton;
	public string restartLevelToLoad;
	public GameObject gameOverScoreOutline;

	[Header("=======Others=======")]
	public Button playAgainButton;
	public string playAgainLevelToLoad;
	public Button nextLevelButton;
	public string nextLevelToLoad;

	private float currentTime;

	// setup the game
	void Start () {

		// set the current time to the startTime specified
		currentTime = startTime;

		// get a reference to the GameManager component for use by other scripts
		if (gm == null) 
			gm = this.gameObject.GetComponent<GameManager>();

		// init scoreboard to 0
		mainScoreDisplay.text = "0";

		// inactivate the gameOverScoreOutline gameObject, if it is set
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (false);
		//inactivate the game over canvas
		if(gameOverCanvas)
			gameOverCanvas.SetActive (false);

		// add button click listener to the button
		if (restartButton)
		{
			restartButton.onClick.AddListener(RestartGame);
		}	
	}

	// this is the main game event loop
	void Update () {
		if (!gameIsOver) {
			if (canBeatLevel && (score >= beatLevelScore)) {  // check to see if beat game
				BeatLevel ();
			} else if (currentTime < 0) { // check to see if timer has run out
				EndGame ();
			} else { // game playing state, so update the timer
				currentTime -= Time.deltaTime;
				mainTimerDisplay.text = currentTime.ToString ("0.00");				
			}
		}
	}

	void EndGame() {
		// game is over
		gameIsOver = true;

		// repurpose the timer to display a message to the player
		mainTimerDisplay.text = "GAME OVER";

		// activate the gameOverScoreOutline gameObject, if it is set 
		if (gameOverScoreOutline)
			gameOverScoreOutline.SetActive (true);
	
		// inactivate the main canvas 
		if (mainCanvas)
			mainCanvas.SetActive (false);
		// activate the game over canvas 
		if (gameOverCanvas)
			gameOverCanvas.SetActive(true);
		// reduce the pitch of the background music, if it is set 
		if (musicAudioSource)
			musicAudioSource.pitch = 0.5f; // slow down the music
	}
	
	void BeatLevel() {
		// game is over
		gameIsOver = true;

		// repurpose the timer to display a message to the player
		mainTimerDisplay.text = "LEVEL COMPLETE";

		//Switch the canvases ...
	}

	// public function that can be called to update the score or time
	public void targetHit (int scoreAmount, float timeAmount)
	{
		// increase the score by the scoreAmount and update the text UI
		score += scoreAmount;
		mainScoreDisplay.text = score.ToString ();
		
		// increase the time by the timeAmount
		currentTime += timeAmount;
		
		// don't let it go negative
		if (currentTime < 0)
			currentTime = 0.0f;

		// update the text UI
		mainTimerDisplay.text = currentTime.ToString ("0.00");
	}

	// public function that can be called to restart the game
	public void RestartGame ()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the game from a basic level
		Debug.Log("Restarting!");
        SceneManager.LoadScene(restartLevelToLoad);
	}
	// public function that can be called to restart the game
	public void PlayCurrentLevelAgain()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the level
		SceneManager.LoadScene(playAgainLevelToLoad);
	}
	// public function that can be called to go to the next level of the game
	public void NextLevel ()
	{
		// we are just loading the specified next level (scene)
        SceneManager.LoadScene(nextLevelToLoad);
	}
	

}
