using UnityEngine;
using System;
/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
	//player speeds
	public float speed = 1F;
	public float rotationSpeed = 0.25F;

	//player health
	public int hp = 10;
	public Vector2 pos;
	public Vector2 size;
	public float barDisplay; //current health
	public Texture2D emptyTex;
	public Texture2D fullTex;

	void Start() {
		pos = new Vector2(60,20);
		size = new Vector2(100,20);
	}

	void OnGUI() {
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,0, size.x * barDisplay, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
		GUI.EndGroup();
		GUI.EndGroup();
		if (GUI.Button(new Rect(1250, 20, 60, 40), "Menu"))
		{
			print("You clicked the menu");
		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
		if (shot != null)
		{
			// Avoid friendly fire
			//true for enemy, false for player
			if (shot.isEnemyShot != false)
			{	
				print ("hit player with enemy bullet");
				hp--;
				// Destroy the shot
				Destroy(shot.gameObject); // Remember to always target the game object, otherwise you will just remove the script
			}
		}
	}

	void Update()
	{
		barDisplay = hp*10;
		if(hp > 0)
		{
			// movement and rotation
			float translation = Input.GetAxis("Vertical") * speed;
			float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
			translation *= Time.deltaTime/5;
			transform.Translate(0, translation, 0);
			transform.Rotate(0, 0, -rotation);

			// shooting
			bool shoot = Input.GetButtonDown("Fire1");
			shoot |= Input.GetButtonDown("Fire2");
			// Careful: For Mac users, ctrl + arrow is a bad idea
		
			if (shoot)
			{
				WeaponScript weapon = GetComponent<WeaponScript>();
				if (weapon != null)
				{
					// false because the player is not an enemy
					weapon.Attack(false);
					SoundEffectsHelper.Instance.MakePlayerShotSound();
				}
			}
		}
		else if(hp <= 0){
			print ("you died!");
			if(gameObject.name == "Player"){
				print ("game over!");

				// display the game over screen
				gameObject.AddComponent<GameOverScript>();
			}
		}
	}
}

