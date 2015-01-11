using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{

	public int CoinsCount;
	public int TotalCoins;
	public int LevelCount;
	public int LevelsTotal = 2;
	public LevelLoader levelLoader;
	public GameObject door;
	public GameObject iksica;
	public Transform camera;
	public GameObject cameraSprite;
	public GameObject spaceShip;

	public bool hasAntiGrav = false;
	public bool hasIksica = false;
	public bool hasUnzoom = false;
	public bool hasJet = false;

	public void OnGUI()
	{
		string coinsMsg = "ECTS: " + CoinsCount + "/" + TotalCoins;
		string levelMsg = "Level: " + LevelCount;
		GUI.Label (new Rect (10, 10, 100, 100), coinsMsg + "\n" + levelMsg);
	}

	public void Die()
	{
		ResetPlayer ();
		levelLoader.LoadLevel(LevelCount);
	}

	public int timesGravityDisabled = 0;
	private void EnableGravity()
	{
		timesGravityDisabled--;
		if (timesGravityDisabled == 0) 
		{
			rigidbody2D.gravityScale = 3;
		}
	}
	private void DisableGravity()
	{
		timesGravityDisabled++;
		rigidbody2D.gravityScale = 0;
	}

	void Update()
	{
		if (hasAntiGrav && Input.GetKeyDown (KeyCode.G)) 
		{
			GameObject forcefield = this.transform.FindChild("Forcefield").gameObject;

			if (forcefield.activeSelf)
			{
				EnableGravity();
			}
			else
			{
				DisableGravity();
			}

			forcefield.SetActive(!forcefield.activeSelf);
		}

		if (CoinsCount == TotalCoins)
		{
			door.SetActive(true);
		}

		if (hasIksica && Input.GetKeyDown(KeyCode.LeftShift))
		{
			var iksicaInst = Instantiate(iksica, this.transform.position, Quaternion.identity) as GameObject;
			iksicaInst.GetComponent<IksicaController>().LevelLoader = levelLoader;
			if (transform.localScale.x < 0)
			{			
				iksicaInst.rigidbody2D.velocity = new Vector2(-20, 0);
				iksicaInst.rigidbody2D.angularVelocity = -600;			
			}
			else
			{
				iksicaInst.rigidbody2D.velocity = new Vector2(20, 0);
				iksicaInst.rigidbody2D.angularVelocity = 600;
			}
		}
		if (transform.position.y < -10)
		{
			Die ();
		}

		if (hasUnzoom) 
		{
			if (Input.GetKeyDown (KeyCode.S)) 
			{
				cameraSprite.SetActive (true);
			}

			if (Input.GetKey (KeyCode.S)) 
			{
				Vector3 p = camera.position;
				camera.position = new Vector3 (p.x, p.y, p.z - 10);
			}

			if (Input.GetKeyUp (KeyCode.S)) 
			{
				cameraSprite.SetActive (false);
			}
		}

		if (hasJet) 
		{
			if (Input.GetKeyDown (KeyCode.LeftAlt)) 
			{
				spaceShip.SetActive (true);
				rigidbody2D.velocity = new Vector3 (Mathf.Sign (transform.localScale.x) * 10, 0, 0);
				DisableGravity();
			}
			if (Input.GetKey (KeyCode.LeftAlt)) 
			{
				rigidbody2D.velocity = new Vector3 (Mathf.Sign (transform.localScale.x) * 10, 0, 0);//rigidbody2D.AddForce (new Vector3 (Mathf.Sign (transform.localScale.x) * 1, 0, 0));
			}
			if (Input.GetKeyUp (KeyCode.LeftAlt)) 
			{
				spaceShip.SetActive (false);
				EnableGravity();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Door")
		{
			if (LevelCount == LevelsTotal)
			{
				levelLoader.LoadLevel(1);
				LevelCount = 1;
			}
			else
			{
				levelLoader.LoadLevel(LevelCount + 1);
				LevelCount++;
			}
		}
	}

	public void ResetPlayer()
	{
		EnableGravity();
		timesGravityDisabled = 0;
		this.transform.FindChild ("Forcefield").gameObject.SetActive (false);
		spaceShip.SetActive (false);
	}
}


