using UnityEngine;
using System.Collections;

public class IksicaController : MonoBehaviour 
{
	public LevelLoader LevelLoader;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		if (transform.position.x < 0 || transform.position.x > LevelLoader.MaxX) 
		{
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "Player")
		{
			Destroy (other.gameObject);
			Destroy (this.gameObject);
		}
	}
}


