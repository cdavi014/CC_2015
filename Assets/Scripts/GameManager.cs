using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum CurrentLevel {
	START,
	HAMMER,
	GRAVITY,
	BOMB_DISARM,
	CUTSCENE_0,
	CUTSCENE_1,
	CUTSCENE_2,
	END
}

public class GameManager : MonoBehaviour {
	private static GameManager instance = null;
	public static GameManager Instance {
		get { return instance; }
	}

	public Image blackScreen;
	public float blackoutTime = 1.0f;
	//170x, 20y
	private Vector3 checkPointLocation = Vector3.zero;
	GameObject player = null;

	public AudioManager audio;
	//public LevelBGM levelBGM;

	private int level = 0;

	private CurrentLevel[] levelList = new CurrentLevel [5] {
		CurrentLevel.CUTSCENE_0,
		CurrentLevel.BOMB_DISARM,
		CurrentLevel.GRAVITY,
		CurrentLevel.HAMMER,
		CurrentLevel.END
	};

	//private Dictionary <string, AudioClip> levelBGMDict = new Dictionary<string, AudioClip> ();


	void Awake (){
		if (instance != null) {
			GameObject.Destroy (this.gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad (this.gameObject);
		}
	}

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		if(checkPointLocation == Vector3.zero) checkPointLocation = player.transform.position;
	}

	public void setCheckPoint(Vector3 position)
	{
		checkPointLocation = position;
		Debug.Log("Set new checkpoint to: " + position.ToString());
	}

	public Vector3 getCheckPointPosition()
	{
		return checkPointLocation;
	}

	public void GoToNextLevel (){
		if (instance.level < this.levelList.Length - 1) {
			instance.level++;
			string newLevel = instance.levelList [instance.level].ToString ();

			StartCoroutine (instance.GoToNextLevelRoutine (newLevel));
		}
	}


	public void GoToPrevLevel (){
		if (instance.level > 0) {
			instance.level--;
			string newLevel = instance.levelList [instance.level].ToString ();

			/*if (this.levelBGMDict.ContainsKey (newLevel)) {
				if (this.levelBGMDict[newLevel] != null)
					this.audio.PlayBackgroundMusic (this.levelBGMDict [newLevel], true);
			}*/

			Application.LoadLevel (newLevel);
		}
	}



	IEnumerator GoToNextLevelRoutine (string newLevel){
		float timer = 0.0f;
		float spinTime = 1.0f;

		//GameObject player = GameObject.FindGameObjectWithTag ("Player");

		if (player.GetComponent <PlayerControl> () != null)
			player.GetComponent <PlayerControl> ().enabled = false;

		if (player.GetComponent <PlayerControlGravity> () != null)
			player.GetComponent <PlayerControlGravity> ().enabled = false;

		if (player.GetComponent <Animator> () != null)
			player.GetComponent <Animator> ().enabled = false;

		if (player.GetComponent <Collider2D> () != null)
			player.collider2D.enabled = false;

		if (player.GetComponent <Rigidbody2D> () != null)
			player.rigidbody2D.isKinematic = true;

		this.FadeOut ();

		if (player != null) {
			while (timer < spinTime) {
				yield return new WaitForEndOfFrame ();
				timer += Time.deltaTime;

				player.transform.Rotate (new Vector3 (0, 50 * timer, 0));
			}
		}

		Application.LoadLevel (newLevel);

		this.FadeIn ();
	}




	public void FadeOut (){
		StartCoroutine ("FadeOutRoutine");
	}

	IEnumerator FadeOutRoutine() {
		float timer = 0.0f;

		while (timer < this.blackoutTime) {
			yield return new WaitForEndOfFrame ();
			timer += Time.deltaTime;

			this.blackScreen.color = new Color (this.blackScreen.color.r, this.blackScreen.color.g, this.blackScreen.color.b, timer / this.blackoutTime);
		}
	}

	public void FadeIn (){
		StartCoroutine ("FadeInRoutine");
	}

	IEnumerator FadeInRoutine() {
		float timer = 0.0f;

		while (timer < this.blackoutTime) {
			yield return new WaitForEndOfFrame ();
			timer += Time.deltaTime;

			this.blackScreen.color = new Color (this.blackScreen.color.r, this.blackScreen.color.g, this.blackScreen.color.b, 1.0f - timer / this.blackoutTime);
		}
	}

	public void PlayBackgroundMusic (AudioClip clip, bool loop = false){
		this.audio.PlayBackgroundMusic (clip, loop);
	}

	public void PlayClipAtLocation (AudioClip clip, Vector3 position){
		if (clip != null)
			this.audio.PlayClipAtLocation (clip, position);
	}
}
