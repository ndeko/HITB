using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour {
	public GameObject Player;
	// Use this for initialization
	public float y;
	public float x;
	void Start () 
	{
		this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y - .7f);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
