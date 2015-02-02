using UnityEngine;
using System.Collections;

public class Jumping : MonoBehaviour {

	public float OriginalY;
	public bool jumping;
	public bool falling;
	float force;
	Animator anim;
	public GameObject Player;
	GameObject PlayerShadow;
	float jumpHeight;
	// Use this for initialization
	void Start () 
	{
		jumpHeight = 3.7f;
		Player = GameObject.FindGameObjectWithTag("Player");
		PlayerShadow = GameObject.FindGameObjectWithTag("Shadow");
		anim = GetComponent<Animator>();
		force = 7;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(jumping)
		{
			if(!falling)
			{
				OriginalY = PlayerShadow.transform.position.y + .7f;
				anim.SetBool("Jumping", true);
				//rigidbody2D.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
				transform.position = Vector2.MoveTowards(this.transform.position, 
				                                         new Vector2(this.transform.position.x, PlayerShadow.transform.position.y + jumpHeight)
				                                         ,force * Time.deltaTime);
				if((PlayerShadow.transform.position.y + jumpHeight) - this.transform.position.y <= .001f)
				{
					jumping = false;
					anim.SetBool("Jumping", false);
					falling = true;
				}
			}
		}
		if(falling)
		{
			transform.position = Vector2.MoveTowards(this.transform.position, 
			                                         new Vector2(PlayerShadow.transform.position.x, PlayerShadow.transform.position.y + .7f) 
			                                         ,force * Time.deltaTime);
			if((this.transform.position.y - .8f) <= PlayerShadow.transform.position.y)
			{
				falling = false;
			}

		}

	}
}
