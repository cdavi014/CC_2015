using UnityEngine;
using System.Collections;

public class LevelTriggerGravity : MonoBehaviour {

	Vector2 onEnter = Vector2.zero;
	Vector2 onExit = Vector2.zero;
	bool enteredLevel2 = false;
	GameObject gameManager = null;

	// Use this for initialization
	void Start () 
	{
		gameManager = GameObject.FindGameObjectWithTag("GameManager");
		Debug.Log ("GameManager is" + gameManager.ToString());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log ("Velocity: " + other.rigidbody2D.velocity);
		if(other.tag.Equals ("Player"))
		{
			onEnter = other.rigidbody2D.velocity;
			if(transform.name.Contains("leverLevel_"))
			{
				transform.renderer.enabled = false;
				LeverTriggerAction(other);
			}
			//Debug.Log ("Player is: " + player.ToString ());
			//Debug.Log ("Level Entered" + transform.name + " by object --> " + other.name);
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag.Equals ("Player"))
		{
			PlayerControlGravity player = (PlayerControlGravity) other.gameObject.GetComponent ("PlayerControlGravity");

			onExit = other.rigidbody2D.velocity;
			bool sameDirection = CheckSameDirection (onEnter, onExit); //Find if its a true floor level change

			if(sameDirection && !transform.name.Equals("Level4Trigger") )
			{
				//find out if up or down level, we would have to call player control floorSettings()
				GameManager manager = (GameManager) gameManager.GetComponent("GameManager");
				Debug.Log ("Manager is: " + manager.ToString());

				if(transform.name.Equals("Level2Trigger"))
				{
					manager.setCheckPoint(player.transform.position);

					Debug.Log("Set checkpoint for level 2");
					if(onExit.y < 0.0)
						player.floorSettings(1);
					else 
					{
						player.floorSettings(2); // switching between floors 1 and 2
						if(!enteredLevel2)
							Time.timeScale = 0; enteredLevel2 = true;
					}
				}
				else if(transform.name.Equals("Level3Trigger"))
				{	if(onExit.y < 0.0)
						player.floorSettings(2);
					else
						player.floorSettings(3); // switching between floors 1 and 2 // switching between floors 2 and 3

					Debug.Log ("Name before switch! : " + transform.name);
					
				}
			}
			else if(transform.name.Equals("Level4Trigger"))
			{
				//Debug.Log ("Gravity Scale: " + other.rigidbody2D.gravityScale);
				float random = Random.Range(0, 100);

				player.FlipVertical(50, random, 1);

				//Debug.Log ("Random Number: " + random);
				//Debug.Log ("Random Multiplier: " + randomMultiplier);
			}
		}
	}

	void LeverTriggerAction(Collider2D other)
	{
		if(transform.name.Equals("leverLevel_1_1"))
			Destroy(GameObject.Find("Destroy_1"));
		if(transform.name.Equals("leverLevel_1_2"))//TODO: remove if not have time for moving platform for level 1 --> 2
		{
			//MovingPlatform platform = (MovingPlatform) GameObject.Find ("wall(moving)Gravity").GetComponent("MovingPlatform");
			//platform.moveSpeed.y = 25;
		}
	}
	
	bool CheckSameDirection(Vector2 enter, Vector2 exit)
	{
		//TODO: Add corner case when going sideways and going up on exit

		if ((enter.y < 0 && exit.y < 0) || (enter.y > 0 && exit.y > 0))
				return true;
		else
				return false;
	}
	
}
