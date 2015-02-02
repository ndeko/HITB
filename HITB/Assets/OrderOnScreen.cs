using UnityEngine;
using System.Collections;

public class OrderOnScreen : MonoBehaviour {

	int newLayer;
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		newLayer =  Mathf.Abs((int)(100*this.transform.position.y));
		this.renderer.sortingOrder = newLayer;
	}
}
