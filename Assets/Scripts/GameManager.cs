using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public static GameManager gm;

	[Tooltip("If not set, the player will default to the gameObject tagged as Player.")]
	public GameObject player;

	public enum gameStates {Playing, Death, GameOver, BeatLevel};
	public gameStates gameState = gameStates.Playing;

	public int score=0;
	public bool canBeatLevel = false;
	public int beatLevelScore=0;

	public GameObject mainCanvas;
	public Text mainScoreDisplay;
	public Text mainHealthDisplay;
	public GameObject gameOverCanvas;
	public Text gameOverScoreDisplay;

	[Tooltip("Only need to set if canBeatLevel is set to true.")]
	public GameObject beatLevelCanvas;

	public AudioSource backgroundMusic;
	public AudioClip gameOverSFX;

	[Tooltip("Only need to set if canBeatLevel is set to true.")]
	public AudioClip beatLevelSFX;

	public AudioClip[] musics;

	public bool Use_death = false;

	private Health playerHealth;
	private int dead_enemy_count;

	void Start () {
		if (gm == null) 
			gm = gameObject.GetComponent<GameManager>();

		if (player == null) {
			player = GameObject.FindWithTag("Player");
		}
		dead_enemy_count = 0;
		playerHealth = player.GetComponent<Health>();

		// setup score display
		Collect (0);

		// make other UI inactive
		gameOverCanvas.SetActive (false);
		if (canBeatLevel)
			beatLevelCanvas.SetActive (false);
	}

	void Update () {
		switch (gameState)
		{
		case gameStates.Playing:
			if (playerHealth.isAlive == false) {
				// update gameState
				gameState = gameStates.Death;

				// set the end game score
				gameOverScoreDisplay.text = mainScoreDisplay.text;

				// switch which GUI is showing		
				mainCanvas.SetActive (false);
				gameOverCanvas.SetActive (true);
			} else if (canBeatLevel && score >= beatLevelScore) {
				// update gameState
				gameState = gameStates.BeatLevel;

				// hide the player so game doesn't continue playing
				player.SetActive (false);

				// switch which GUI is showing			
				mainCanvas.SetActive (false);
				beatLevelCanvas.SetActive (true);
			} else {
				mainHealthDisplay.text = playerHealth.healthPoints.ToString ();
			}
				break;
			case gameStates.Death:
				backgroundMusic.volume -= 0.01f;
				if (backgroundMusic.volume<=0.0f) {
					GameObject mainCamera = GameObject.Find ("Main Camera");
					AudioSource.PlayClipAtPoint (gameOverSFX,mainCamera.transform.position);
					gameState = gameStates.GameOver;
				}
				break;
			case gameStates.BeatLevel:
				backgroundMusic.volume -= 0.01f;
				if (backgroundMusic.volume<=0.0f) {
					GameObject mainCamera = GameObject.Find ("Main Camera");
					AudioSource.PlayClipAtPoint (beatLevelSFX,mainCamera.transform.position);
					gameState = gameStates.GameOver;
				}
				break;
			case gameStates.GameOver:
				// nothing
				break;
		}

	}


	public void Collect(int amount) {
		score += amount;
		if (canBeatLevel) {
			mainScoreDisplay.text = score.ToString () + " of "+beatLevelScore.ToString ();
		} else {
			mainScoreDisplay.text = score.ToString ();
			// if (score <= musics.GetLength(0) && score > 0) {
			// 	AudioSource.PlayClipAtPoint (musics[score-1],gameObject.transform.position);
			// }
		}

	}

	public void Enemy_dead() {
		if (Use_death) {
			score++;
			mainScoreDisplay.text = score.ToString () + " of "+beatLevelScore.ToString ();
		}
		if (dead_enemy_count >= musics.GetLength (0)) {
			return;
		}
		GameObject mainCamera = GameObject.Find ("Main Camera");
		AudioSource.PlayClipAtPoint (musics[dead_enemy_count++], mainCamera.transform.position);
	}
}
