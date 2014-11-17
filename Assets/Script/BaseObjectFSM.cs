using UnityEngine;
using System.Collections;

public class BaseObjectFSM : MonoBehaviour {

		//debug shit
		#region debug
		public GUIText guiTextMooseState;
		public float attackForce = 800;
		public float walkSpeed = 3;

		public Vector2 direction;


		protected Vector3 moveToDirection = Vector3.zero;
		protected float angle = 0;	

		public float shootOutSpeed = 7.0f; //the speed to change to balling
		public float stopBallingSpeed = 4.0f; //the speed to stop balling

		#endregion

		#region Death

		Vector3 deathPosition;


		#endregion


		#region shoot out Checks
		float previousShoutOutSpeed = 0.0f;

		#endregion

		public enum MooseState
		{
				Stand = 0, //no movement
				Walk, //walk to point
				Charging, //start charging
				Charged, //ready to ball
				ShootOut, //to speed up to normalSpeed
				Ball, //balling around the room
				Respawn, //respawn animation
				Dying, //play the dying animation
				Dead // set to inactive and stuff like that

	};

		public MooseState state;


		public MooseAnimator mooseAnimator;
		Vector3 rotateAngle;
		public float rotationSpeed;
		public float slowMotionRotationSpeed;



		public bool slowMotion = false;

		public Sounds soundManager;


		IEnumerator StandState()
		{
				showDebugState("Stand: Enter");
				rigidbody2D.velocity = Vector2.zero;
				mooseAnimator.state = MooseAnimator.BodyState.Normal;
				while (state == MooseState.Stand)
				{
			 	
						yield return null;
				}
				showDebugState("Stand: Exit");
				NextState();
		}

		IEnumerator WalkState()
		{
				showDebugState("Walk: Enter");
				mooseAnimator.Walking = true;
				mooseAnimator.state = MooseAnimator.BodyState.Normal;
				while (state == MooseState.Walk)
				{
						//animation.Play ("Move");
						yield return null;
				}
				showDebugState("Walk: Exit");
				mooseAnimator.Walking = false;
				NextState();
		}

		IEnumerator ChargingState()
		{
				showDebugState("Charging: Enter");

				mooseAnimator.state = MooseAnimator.BodyState.Charging;
				soundManager.Inflate.Play ();
				while (state == MooseState.Charging)
				{
			//animation.Play ("Move");
						float speed = rigidbody2D.velocity.magnitude;
						if (Mathf.Abs(speed) > 0)
						{
								state = MooseState.Ball;
								//rigidbody2D.velocity = Vector3.zero;
						}
						yield return null;
				}
				showDebugState("Charging: Exit");
				if(state == MooseState.Ball)
						soundManager.HitCharged.Play ();
				NextState();
		}

		IEnumerator ChargedState()
		{
				showDebugState("Charged: Enter");
				mooseAnimator.state = MooseAnimator.BodyState.Charged;
				while (state == MooseState.Charged)
				{
			//animation.Play ("Move");

						float speed = rigidbody2D.velocity.magnitude;
						//Debug.Log ("speed : " + speed);

						//[self bodyAnimation:LARGE_BODY];
						//TODO: change this
						//[self rotateToAngle:0 radians:NO];

						if (Mathf.Abs(speed) > 0)
						{
								state = MooseState.Ball;
								//rigidbody2D.velocity = Vector3.zero;
						}
						yield return null;
				}
				showDebugState("Charged: Exit");
				if(state == MooseState.Ball)
						soundManager.HitCharged.Play ();
				else if(state == MooseState.ShootOut)
						soundManager.Launch.Play ();

				NextState();
		}

		IEnumerator ShootOutState()
		{
				showDebugState("ShootOut: Enter");


				mooseAnimator.state = MooseAnimator.BodyState.Ball;
				while (state == MooseState.ShootOut)
				{
						float speed = rigidbody2D.velocity.magnitude;
						//Debug.Log ("speed : " + speed);

						if (speed > shootOutSpeed || speed <= previousShoutOutSpeed)
						{
								state = MooseState.Ball;
						}
						else if(speed == previousShoutOutSpeed)
						{
								state = MooseState.Stand;
						}

						previousShoutOutSpeed = speed;

						yield return null;
				}

				showDebugState("ShootOut: Exit");
				NextState();
		}

		IEnumerator BallState()
		{

				showDebugState("Ball: Enter");
				mooseAnimator.state = MooseAnimator.BodyState.Ball;
				while (state == MooseState.Ball)
				{


						float speed = rigidbody2D.velocity.magnitude;

						if (Mathf.Abs(speed) < stopBallingSpeed)
						{
								state = MooseState.Stand;
								rigidbody2D.velocity = Vector2.zero;
						}

						yield return null;
				}

				showDebugState("Ball: Exit");
				NextState();
		}

		IEnumerator RespawnState()
		{
				showDebugState("Respawn: Enter");
				//animation.Play ("Idle");
				while (state == MooseState.Respawn)
				{
						yield return null;
				}
				showDebugState("Respawn: Exit");
				NextState();
		}

		IEnumerator DyingState()
		{
				showDebugState("Dying: Enter");
				rigidbody2D.velocity = Vector2.zero;
				//animation.Play ("Idle");
				soundManager.Death.Play ();
				while (state == MooseState.Dying)
				{
						if(transform.position != deathPosition)
						{
								float step = 10 * Time.deltaTime;
								transform.position = Vector3.MoveTowards(transform.position, deathPosition, step);
						}
						else
						{
								state = MooseState.Dead;
						}

						yield return null;
				}
				showDebugState("Dying: Exit");
				NextState();
		}

		IEnumerator DeadState()
		{
				showDebugState("Dead: Enter");
				GameManager.instance.AddGhost (deathPosition, false);
				rigidbody2D.velocity = Vector2.zero;
				//animation.Play ("Death");
				while (state == MooseState.Dead)
				{
						//need to work out a spawner
						PoolingSystem.DestroyAPS (this.gameObject);

						//gameObject.SetActive (false);

						yield return null;
				}
				showDebugState("Dead: Exit");
				NextState();
		}

		public virtual void Start () 
		{
				Transform mychildtransform = transform.FindChild("SoundManager");
				soundManager = mychildtransform.gameObject.GetComponent<Sounds>();


				NextState();
		}
				

		public void NextState () 
		{
				string methodName = state.ToString() + "State";
				ChangeGUITextMooseState(state.ToString());
				StartCoroutine (methodName);
		}

		public void ChangeGUITextMooseState(string s)
		{
				guiTextMooseState.text = s;
		}

		public void setCharged()
		{
				mooseAnimator.state = MooseAnimator.BodyState.Charged;
				state = MooseState.Charged;
		}


		void showDebugState(string debugString)
		{
				//Debug.Log (this.gameObject.tag);
				//if(gameObject.tag == "EnemyMoose")
				//		Debug.Log (debugString);
		}



		public virtual void OnCollisionEnter2D(Collision2D coll)
		{
				//Debug.Log ("enemy hit");

				if (coll.gameObject.tag == "EnemyMoose") 
				{
						//state = MooseState.Ball;
						BaseObjectFSM other = coll.gameObject.GetComponent<BaseObjectFSM> ();


						if (state == MooseState.Walk && other.state == MooseState.Walk)
								return;
						else
								collisionStuff (coll);

				} 
				else if (coll.gameObject.tag == "Player")
						collisionStuff (coll);

				if (coll.gameObject.tag == "Wall")
						soundManager.WallHit.Play ();
						
		}


		void collisionStuff(Collision2D coll)
		{
				state = MooseState.ShootOut;
				GameManager.instance.AddSplosion ((Vector3)coll.contacts[0].point);
				soundManager.MooseHitMoose.Play ();
				rigidbody2D.isKinematic = false;
				coll.gameObject.rigidbody2D.isKinematic = false;


		}

		void OnTriggerEnter2D(Collider2D coll)
		{
				if (coll.name == "Pit")
				{

						Debug.Log ("entered hole");
						deathPosition = coll.transform.position;
						state = MooseState.Dying;
						//
				}
		}
				

		void OnActive()
		{
				state = MooseState.Stand;
				NextState();
		}
}

