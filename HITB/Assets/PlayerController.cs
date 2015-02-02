using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	float maxSpeed;
	public float move;
	public float ComboTimer;
	bool facingRight = true;
	public bool Blocking;
	GameObject PlayerShadow;
	Animator anim;
	// Use this for initialization
	void Start ()
	{
		Blocking = false;
		move = 0;
		maxSpeed = 8;
		ComboTimer = 0f;
		PlayerShadow = GameObject.FindGameObjectWithTag("Shadow");
		anim = GetComponent<Animator>();

	}
	void Update()
	{
		if(Input.GetButtonUp("Block"))
		{
			anim.SetBool("B-Input", false);
			Blocking= false;
		}
	}
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (ComboTimer > 0)
		{
			ComboTimer -= Time.deltaTime;
		}
		if(Input.GetButtonDown("Jump") && !Blocking && !(this.GetComponent<Jumping>().jumping) && !(this.GetComponent<Jumping>().falling))
		{
			this.GetComponent<Jumping>().jumping = true;
		}
		else if(Input.GetButton("Block"))
		{
			anim.SetBool("B-Input", true);
			Blocking= true;
		}
		else if(Input.GetButtonDown("Punch") && move < 0.1 && !Blocking)
		{

			if(ComboTimer > 0)
			{
				anim.SetTrigger("Second-X");
				ComboTimer = 0;
			}
			else
			{
				anim.SetTrigger("X-Input");
				ComboTimer = 1.0f;
			}
			Punch();

		}
		if(GetComponent<Jumping>().jumping || GetComponent<Jumping>().falling)
		{
			MoveInAir();
			ComboTimer = 0;
		}
		else if (!Blocking)
		{
			Move();

		}

	}
	void Move()
	{
		if(Input.GetAxis("Vertical") > 0)
		{
			if(this.transform.position.y < 0)
			{
				move = Input.GetAxis("Vertical");
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, (move * maxSpeed) / 2);
				PlayerShadow.GetComponent<Rigidbody2D>().velocity = new Vector2(rigidbody2D.velocity.x, (move * maxSpeed) / 2);
				ComboTimer = 0;
			}

		}
		if(this.transform.position.y > -3.5)
		{
			if(Input.GetAxis("Vertical") < 0)
			{
				move = Input.GetAxis("Vertical");
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, (move * maxSpeed) / 2);
				PlayerShadow.GetComponent<Rigidbody2D>().velocity =
					new Vector2(rigidbody2D.velocity.x, (move * maxSpeed) / 2);
				ComboTimer = 0;
			}
			
		}
		if( Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
		{
			move = Input.GetAxis("Horizontal");
			if(move > 0 && !facingRight)
			{
				Flip();
			}
			else if(move < 0 && facingRight)
			{
				Flip();
			}
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			PlayerShadow.GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			ComboTimer = 0;
			
		}
		anim.SetFloat("Speed",Mathf.Abs(move));
		move = 0;
	}

	void MoveInAir()
	{
		if(Input.GetAxis("Vertical") > 0)
		{
			if(this.transform.position.y < 0)
			{
				move = Input.GetAxis("Vertical");
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, (move * maxSpeed) / 2);
				PlayerShadow.GetComponent<Rigidbody2D>().velocity = new Vector2(rigidbody2D.velocity.x, (move * maxSpeed) / 2);
			}
		}
		if(Input.GetAxis("Vertical") < 0)
		{
			if(this.transform.position.y > -3.5)
			{
				move = Input.GetAxis("Vertical");
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, (move * maxSpeed) / 2);
				PlayerShadow.GetComponent<Rigidbody2D>().velocity = new Vector2(rigidbody2D.velocity.x, (move * maxSpeed) / 2);
			}
		}
		if(Input.GetAxis("Horizontal") > 0)
		{
			move = Input.GetAxis("Horizontal");

			if(facingRight)
			{
				Flip();
			}
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			PlayerShadow.GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		}
		else if(Input.GetAxis("Horizontal") < 0)
		{
			move = Input.GetAxis("Horizontal");
			if(!facingRight)
			{
				Flip();
			}
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			PlayerShadow.GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		}
		anim.SetFloat("Speed",Mathf.Abs(move));

	}
	void Punch()
	{
		Vector2 hitSize;
		hitSize.y = this.gameObject.transform.position.y + .2f;

		if(facingRight)
		{
			hitSize.x = this.gameObject.transform.position.x + 1.3f;
		}
		else
		{
			hitSize.x = this.gameObject.transform.position.x - 1.3f;
		}
		Collider2D[] enemiesHit = new Collider2D[10];
		Physics2D.OverlapAreaNonAlloc(this.transform.position, hitSize, enemiesHit);
		Vector3 draw;
		draw.x = hitSize.x;
		draw.y = hitSize.y;
		draw.z = this.transform.position.z;
		Debug.DrawLine(this.transform.position, draw, Color.red, 120);
		//print(enemiesHit[0].ToString());
		for (int enemies = 0; enemies < enemiesHit.Length - 1; enemies++)
		{
			if(enemiesHit[enemies] != null)
			{
				GameObject HitEnemy = enemiesHit[enemies].collider2D.gameObject;
				if(HitEnemy.tag == "Enemy")
				{
					HitEnemy.GetComponent<EnemyAI>().GetHit();
				}
			}
		}
	}
	void Flip()
	{
		facingRight= !facingRight;
		Vector3 theScale =  transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
