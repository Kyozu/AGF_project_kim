using UnityEngine;
using System.Collections;

public class EnemyMoose : MonoBehaviour 
{
	/*
	public float targetDistanceToPlayer = 4;
	public float distanceToPlayer = 0;
	public PlayerMooseFSM playerMoose;
	public float attackCooldown = 2;
	public float preAttackDelay = 0.5f;
	public bool enableAttack = true;

	//Initiate attack protocol
	public void SetAttack()
	{
		if(!enableAttack) return; //attack is still in cooldown
		myRigidbody.velocity = Vector3.zero;
		ScaleUp();
		currentState = MooseState.Sizing;
		ChangeGUITextMooseState(currentState.ToString());
		StartCoroutine("PreAttackDelay");
	}

	IEnumerator PreAttackDelay()
	{
		yield return new WaitForSeconds(preAttackDelay);
		Charge();
	}

	public void Charge()
	{

		ScaleDown();
		myRigidbody.AddForce(direction * attackForce);
		currentState = MooseState.Charge;
		ChangeGUITextMooseState(currentState.ToString());
		StartCoroutine("SetAttackCooldown");
	}

	IEnumerator SetAttackCooldown() //Don't ever stop this coroutine
	{
		enableAttack = false;
		yield return new WaitForSeconds(attackCooldown);
		enableAttack = true;
	}

	void FixedUpdate()
	{
		//		Debug.Log(myRigidbody.velocity.sqrMagnitude);

	}

	void Update()
	{
		if(isDead) return;

		if(currentState == MooseState.Attacked)
			spriteTransform.Rotate(new Vector3(0,0,rotationSpeed));

		if(currentState == MooseState.None)
		{
			if(playerMoose)
			{
				if(playerMoose.isDead) return;
				direction = (playerMoose.myTransform.position - myTransform.position).normalized;
				angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;
				spriteTransform.rotation = Quaternion.Euler(0,0, angle - 90);

				distanceToPlayer = Vector3.Distance(playerMoose.myTransform.position,myTransform.position);

				if(distanceToPlayer > targetDistanceToPlayer)
				{
					myRigidbody.velocity = direction * walkSpeed;

				}
				else
				{
					myRigidbody.velocity = Vector3.zero;
					SetAttack();
				}
			}
		}

		if(currentState == MooseState.Charge || currentState == MooseState.Attacked) //Not allowed to do anything if attacked
		{
			currentSqrMag = myRigidbody.velocity.sqrMagnitude;
			if(lastSqrMag > currentSqrMag)
			{
//				Debug.Log(lastSqrMag + " VS " + currentSqrMag);
				if(currentSqrMag < 7)
				{
					Debug.Log(name + "STOP");
					myRigidbody.velocity = Vector3.zero;
					currentState = MooseState.None;
					ChangeGUITextMooseState(currentState.ToString());
					lastSqrMag = 0;
					currentSqrMag = 0;
				}
			}
			else if(myRigidbody.IsSleeping())
			{
				Debug.Log(name + "EMERGENCY STOP");
				myRigidbody.velocity = Vector3.zero;
				currentState = MooseState.None;
				ChangeGUITextMooseState(currentState.ToString());
				lastSqrMag = 0;
				currentSqrMag = 0;
			}
			if(currentSqrMag > 0)
				lastSqrMag = currentSqrMag;
		}	
	}

	void Awake()
	{
		Init();
	}
	
	void Start()
	{
				playerMoose = GameObject.Find("Player").GetComponent<PlayerMooseFSM>();
	}
	*/
	
}
