using UnityEngine;


using System.Collections;





public class CoinController : MonoBehaviour {





	void OnTriggerEnter2D(Collider2D other)


	{


		if (other.tag == "Player") 


		{


			Destroy (this.gameObject);


			other.GetComponent<PlayerController>().CoinsCount++;


		}


	}


}


