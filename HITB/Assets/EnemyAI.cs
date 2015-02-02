using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour 
{
	//cooldown for enemy moving again
	const float mCoolDown = 1.0f;
	//the randomly generated x and y for the enemy to stand "somewhere" near the player
	public float relX;
	public float relY;

	//Cooldowns for attacking and moving so neither is constantly happening
	public float moveCoolDown;
	public float AttackCoolDown;

	//players position
	Vector2 Playerpos;
	//where this enemy should stand reletive to the player
	Vector2 MoveTo;
	//max move speed
	int maxSpeed;
	//attack range
	int hitRange;
	float hitRangeV;
	//if this enemy is facing right or not.
	bool facingRight;
	//the animator for the enemy
	Animator anim;


	void Start () 
	{
		moveCoolDown = 1.5f;
		AttackCoolDown = Random.Range(1,3);
		maxSpeed = 4;
		hitRange = 1;
		hitRangeV = .5f;
		facingRight = true;
		anim = this.GetComponent<Animator>();
		Playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;
		relX = (Random.Range(50,150)) / 100f;
		relY = (Random.Range(-50,50)) / 100f;
	}


	void Update ()
	{
		//set which way the enemy should face and which side of the player it should aim for
		SetMoveDirection();
		//if they're not at the predetermined location they should move
		if(ShouldMove())
		{
			//make him move his legs
			anim.SetBool("Moving", true);
			//Move ONLY moves the enemy. all checking and calculation is done before this
			Move();
		}
		else if (ShouldAttack() && moveCoolDown == mCoolDown)
		{
			anim.SetBool("Punch",true);
			Attack();
		}
		else
		{
			//puts the enemy back into his idle state
			anim.SetBool("Moving", false);
		}

	}
	#region /********************** MOVEMENT **************************/
	void SetMoveDirection()
	{
		//(re)sets the players current location
		Playerpos = GameObject.FindGameObjectWithTag("Player").transform.position;
		//if the player is to the right of the enemy
		if (this.transform.position.x < Playerpos.x)
		{
			//the enemy will aim to stand on the left of the player
			MoveTo = new Vector2(Playerpos.x - relX, Playerpos.y + relY);
			//if they're not facing right (which they should be)
			if(!facingRight)
			{
				//flip the sprite to face right
				Flip();
			}
		}
		else
		{
			//enemy will aim for the right side of the player
			MoveTo = new Vector2(Playerpos.x + relX, Playerpos.y + relY);
			//if they're not facing left which they should be
			if (facingRight)
			{
				//flip the sprite to face left
				Flip();
			}
		}
	}
	void Move()
	{
		//move to the determined location
		transform.position = Vector2.MoveTowards(transform.position, MoveTo, 3 * Time.deltaTime);
	}
	void Flip()
	{
		facingRight= !facingRight;
		Vector3 theScale =  transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	bool ShouldMove()
	{
		//checks if the enemy is where they should be
		if (transform.position.x != MoveTo.x || transform.position.y != MoveTo.y)
		{
			//if the move cooldown has run its course
			if(moveCoolDown <= 0)
			{
				return true;
			}
			else
			{
				//they need to rest more so reduce the cooldown
				moveCoolDown -= Time.deltaTime;
				return false;
			}
		}
		else
		{
			//if we're here then the enemy is standing where it should so reset the cooldown
			moveCoolDown = mCoolDown;
		}
		return false;
	}
#endregion
#region /********************** COMBAT **************************/
	bool ShouldAttack()
	{
		if(AttackCoolDown <= 0)
		{
			AttackCoolDown = Random.Range(1,3);
			return true;
		}
		else if(AttackCoolDown > 0 && moveCoolDown == mCoolDown)
		{
			AttackCoolDown -= Time.deltaTime;

		}
		return false;
	}
	void Attack()
	{
		Vector2 hitSize;
		hitSize.y = transform.position.y + hitRangeV;
		if(facingRight)
		{
			hitSize.x = transform.position.x + hitRange;
		}
		else
		{
			hitSize.x = transform.position.x - hitRange;
		}
		Collider2D[] PlayersHit = new Collider2D[10];
		Physics2D.OverlapAreaNonAlloc(this.transform.position, hitSize, PlayersHit);
		Vector3 draw;
		draw.x = hitSize.x;
		draw.y = hitSize.y;
		draw.z = this.transform.position.z;
		Debug.DrawLine(this.transform.position, draw, Color.red, 120);

	}
	public void GetHit()
	{
		print("I Got Hit");
		anim.SetTrigger("Hit");
	}
#endregion
}
