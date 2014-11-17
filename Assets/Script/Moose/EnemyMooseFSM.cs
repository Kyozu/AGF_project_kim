using UnityEngine;
using System.Collections;

public class EnemyMooseFSM : BaseObjectFSM {

		GameManager gameManeger;

		public float minDistanceToPlayer = 4f;
		public float maxDistanceToPlayer = 5f;
		public float distanceToPlayer = 0;
		public PlayerMooseFSM playerMoose;
		public float attackCooldown = 2f;
		public float AttackDelay = 0.5f;
		float currentAttackDelay = 0f;
		public bool enableAttack = true;

		Steer2D.SteeringAgent myAgent;
		Steer2D.Seek seekController;
		Steer2D.Flee fleeController;

	// Use this for initialization
	public override void Start () 
		{
				base.Start ();

				//GameObject GM = GameObject.FindGameObjectWithTag("GameController");
				gameManeger = GameManager.instance;

				GameObject player = GameObject.FindGameObjectWithTag("Player");
				playerMoose = player.GetComponent<PlayerMooseFSM> ();

				setupSeek ();
		}
	

		void setupSeek()
		{
				myAgent = GetComponent<Steer2D.SteeringAgent> ();
				myAgent.CanSteer = false;

				seekController = GetComponent<Steer2D.Seek> ();
				seekController.SeekTarget = playerMoose.gameObject.transform;
				seekController.enabled = false;
				fleeController = GetComponent<Steer2D.Flee> ();
				fleeController.fleeTargetPoint = playerMoose.gameObject.transform;
				fleeController.FleeRadius = minDistanceToPlayer;
				fleeController.enabled = false;
		}
	// Update is called once per frame
	void FixedUpdate () 
		{
				if(state == MooseState.Dying || state == MooseState.Dead || state == MooseState.Respawn) return;

				if(state == MooseState.Ball || state == MooseState.ShootOut)
				{
						currentAttackDelay = 0f;
						if (playerMoose.slowMotion) 
						{
								mooseAnimator.rotateSpeed = playerMoose.slowMotionRotationSpeed;
						} 
						else
						{
								mooseAnimator.rotateSpeed = playerMoose.rotationSpeed;
						}

				}
				else
				{
						Vector3 directions = (playerMoose.transform.position - transform.position).normalized;
						angle = Mathf.Atan2 (directions.y, directions.x);// * Mathf.Rad2Deg;

						mooseAnimator.rotateToAngle (angle, true);
						findTarget (directions);
				}
	
	}

		void findTarget(Vector3 direction)
		{

				distanceToPlayer = Vector3.Distance(playerMoose.transform.position,transform.position);


				if(state == MooseState.Stand)
				{
						if(distanceToPlayer > maxDistanceToPlayer || distanceToPlayer < minDistanceToPlayer)
								state = MooseState.Walk;
						else
						{
								int shoot = Random.Range (0, 4000);

								if (shoot > 200 && shoot < 250)
										state = MooseState.Charging;
						}


				}
				else if(state == MooseState.Walk)
				{
						//if() distance to player is less than distance to player then avoid
						//else if distance is less than move towards
						//else attack
						myAgent.CanSteer = true;

						if(distanceToPlayer > maxDistanceToPlayer)
						{
								//rigidbody2D.velocity = direction * walkSpeed;
								//gameObject.AddComponent<Steer2D>(Seek)
								seekController.enabled = true;
								fleeController.enabled = false;
						}
						else if (distanceToPlayer < minDistanceToPlayer)
						{
								//rigidbody2D.velocity =  -direction * walkSpeed;
								//SetAttack();
								seekController.enabled = false;
								fleeController.enabled = true;
						}
						else 
						{

								seekController.enabled = false;
								fleeController.enabled = false;
								//rigidbody2D.velocity = Vector3.Lerp(rigidbody2D.velocity, Vector3.zero, Time.fixedTime );
								rigidbody2D.velocity = Vector2.zero;
								state = MooseState.Stand;


						}
				}
				if(state == MooseState.Charged)
				{
						if (AttackDelay > currentAttackDelay)
								currentAttackDelay += Time.fixedDeltaTime;
						else
						{
								state = MooseState.ShootOut;
								//ChangeGUITextMooseState(currentState.ToString());
								rigidbody2D.velocity = Vector2.zero;
								Vector2 directions = (playerMoose.transform.position - transform.position).normalized;
								rigidbody2D.AddForce(directions * attackForce);
						}

				}

		}
				

		public override void OnCollisionEnter2D(Collision2D coll)
		{
				//Debug.Log ("enemy hit");


				if (coll.gameObject.tag == "EnemyMoose") 
				{
						//state = MooseState.Ball;
						if (state != MooseState.Walk)
								disableSteering ();

				} 
				else if (coll.gameObject.tag == "Player")
				{
						disableSteering ();
						gameManeger.addScore (10, (Vector3)coll.contacts[0].point);
				}
						

				base.OnCollisionEnter2D(coll);


				//state = MooseState.ShootOut;

		}

		void disableSteering()
		{
				rigidbody2D.isKinematic = false;
				myAgent.CanSteer = false;
				seekController.enabled = false;
				fleeController.enabled = false;
		}
}
