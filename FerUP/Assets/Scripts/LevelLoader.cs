using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class LevelLoader : MonoBehaviour 
{
	public GameObject horizontalBlock;
	public GameObject verticalBlock;
	public GameObject coin;
	public GameObject player;
	public GameObject background;
	public GameObject door;
	public float MaxX;
	public float MaxY;
	public string startMessage = "";
	public bool displayMessage = true;
	public GameObject ilko;
	public bool hasIlko = false;

	private List<GameObject> levelObjects = new List<GameObject>();

	void Start ()
	{
		LoadLevel (player.GetComponent<PlayerController>().LevelCount);
		player.GetComponent<PlayerController> ().LevelsTotal = Directory.GetFiles (@"Assets\Resources\Levels\", "*.txt").Length;
	}

	public void ClearLevel()
	{
		startMessage = "";
		for (int i = 0; i < this.transform.childCount; i++)
		{
			Destroy(this.transform.GetChild(i).gameObject);
		}
		door.SetActive(false);
	}

	public void LoadLevel(int levelCount)
	{
		Time.timeScale = 0;
		ClearLevel ();
		player.GetComponent<PlayerController> ().ResetPlayer ();

		string[] levelMatrix = (Resources.Load ("Levels/"+levelCount, typeof(TextAsset)) as TextAsset).text.Split ('\n');
		Array.Reverse(levelMatrix);

		player.GetComponent<PlayerController> ().CoinsCount = 0;

		float maxX = 0;
		float maxY = 0;

		float y = 0;

		player.GetComponent<PlayerController> ().TotalCoins = 0;
		var p = player.GetComponent<PlayerController>();
		p.hasAntiGrav = p.hasIksica = p.hasUnzoom = p.hasJet = hasIlko = false;
		foreach (string line in levelMatrix) 
		{
			if (line == "") continue;
			if (line.StartsWith("set")) 
			{
				string[] words = line.Split(' ');
				string variable = words[1];
				bool value = bool.Parse (words[2]);
				switch (variable)
				{
					case "hasAntiGrav": p.hasAntiGrav = value; break;
					case "hasIksica": p.hasIksica = value; break;
					case "hasUnzoom": p.hasUnzoom = value; break;
					case "hasJet": p.hasJet = value; break;
					case "hasBombs": hasIlko = value; break;
				}
				continue;
			}
			if (line.StartsWith("message"))
			{
				startMessage = line.Substring("message".Length).Replace('|', '\n');
				continue;
			}
			float x = 0;
			foreach (char ch in line)
			{
				if (ch == '-' || ch == '|')
				{
					var block = Instantiate(horizontalBlock, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
					block.transform.parent = this.transform;
					levelObjects.Add(block);
				}
				else if (ch == 'P')
				{
					player.GetComponent<Transform>().position = new Vector3(x, y, 0);
				}
				else if (ch == 'C')
				{
					var block = Instantiate(coin, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
					block.transform.parent = this.transform;
					player.GetComponent<PlayerController>().TotalCoins++;
				}
				else if (ch == 'D')
				{
					door.transform.position = new Vector3(x, y, 0);
				}
				x += 2;
			}

			y += 2;
			maxX = Math.Max(x, maxX);
			maxY = Math.Max(y, maxY);
		}
		MaxX = (float)maxX;
		MaxY = (float)maxY;

		background.transform.localScale = new Vector3 (maxX + 50, maxY + 50, 1);
		background.transform.position = new Vector3 (maxX / 2, maxY / 2, 2);
		
		displayMessage = true;
		Time.timeScale = 0;
	}

	public void OnGUI()
	{
		if (displayMessage)
		{
			GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
			centeredStyle.alignment = TextAnchor.UpperCenter;
			string instructions = "\nPress ENTER to start";
			GUI.Label (new Rect (0, Screen.height/2-25, 500, 500), startMessage+"\n"+instructions, centeredStyle);
		}
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			Time.timeScale = 1;
			displayMessage = false;
		}

	}

	public void Update()
	{
		if (hasIlko && Time.frameCount % (int)(100 * 60 / MaxX) == 0)
		{
			float x = UnityEngine.Random.Range(0, MaxX);
			float y = MaxY;
			var bomb = Instantiate(ilko, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
			bomb.rigidbody2D.angularVelocity = 200;
			bomb.rigidbody2D.velocity = new Vector3(UnityEngine.Random.Range(-3, 3), -3, 0);
		}
	}
}


