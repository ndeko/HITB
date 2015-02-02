using UnityEngine;
using System.Collections;

public class EnemyControler : MonoBehaviour {

	// Use this for initialization
	private int health;
	void Start () 
	{
		health = 3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void GetHit()
	{
		print ("I got hit!");
		health--;
		if(health == 0)
		{
			Destroy(this.gameObject);
		}
	}

}
