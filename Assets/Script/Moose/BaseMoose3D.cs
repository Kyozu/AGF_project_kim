using UnityEngine;
using System.Collections;

public class BaseMoose3D : MonoBehaviour {

		#region debug
		//public GUIText guiTextMooseState;
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
		public bool isPlayer = false;
		#endregion

		public enum MooseState3D
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

		public MooseState3D state;


		public MooseAC3D mooseAnimator;
		Vector3 rotateAngle;
		public float rotationSpeed;
		public float slowMotionRotationSpeed;

		bool isDead = false;

		public bool slowMotion = false;

		public Sounds soundManager;


		IEnumerator StandState()
		{
				showDebugState("Stand: Enter");

				rigidbody.velocity = Vector3.zero;
				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Stand);
				while (state == MooseState3D.Stand)
				{

						yield return null;
				}
				showDebugState("Stand: Exit");
				NextState();
		}

		IEnumerator WalkState()
		{
				showDebugState("Walk: Enter");
				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Walk);
				while (state == MooseState3D.Walk)
				{
						//animation.Play ("Move");
						yield return null;
				}
				showDebugState("Walk: Exit");
				NextState();
		}

		IEnumerator ChargingState()
		{
				showDebugState("Charging: Enter");

				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Charging);
				soundManager.Inflate.Play ();
				while (state == MooseState3D.Charging)
				{
						//animation.Play ("Move");
						float speed = rigidbody.velocity.magnitude;
						if (Mathf.Abs(speed) > 0)
						{
								state = MooseState3D.Ball;
								//rigidbody.velocity = Vector3.zero;
						}
						yield return null;
				}
				showDebugState("Charging: Exit");
				if(state == MooseState3D.Ball)
						soundManager.HitCharged.Play ();
				NextState();
		}

		IEnumerator ChargedState()
		{
				showDebugState("Charged: Enter");
				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Charged);
				while (state == MooseState3D.Charged)
				{
						//animation.Play ("Move");

						float speed = rigidbody.velocity.magnitude;
						//Debug.Log ("speed : " + speed);

						//[self bodyAnimation:LARGE_BODY];
						//TODO: change this
						//[self rotateToAngle:0 radians:NO];

						if (Mathf.Abs(speed) > 0)
						{
								state = MooseState3D.Ball;
								rigidbody.velocity = Vector3.zero;
						}
						yield return null;
				}
				showDebugState("Charged: Exit");
				if(state == MooseState3D.Ball)
						soundManager.HitCharged.Play ();
				else if(state == MooseState3D.ShootOut)
						soundManager.Launch.Play ();

				NextState();
		}

		IEnumerator ShootOutState()
		{
				showDebugState("ShootOut: Enter");


				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Ball);
				while (state == MooseState3D.ShootOut)
				{
						float speed = rigidbody.velocity.magnitude;
						//Debug.Log ("speed : " + speed);

						if (speed > shootOutSpeed || speed <= previousShoutOutSpeed)
						{
								state = MooseState3D.Ball;
						}
						else if(speed == previousShoutOutSpeed)
						{
								state = MooseState3D.Stand;
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
				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Ball);
				while (state == MooseState3D.Ball)
				{


						float speed = rigidbody.velocity.magnitude;

						if (Mathf.Abs(speed) < stopBallingSpeed)
						{
								state = MooseState3D.Stand;
								rigidbody.velocity = Vector2.zero;
						}

						yield return null;
				}

				showDebugState("Ball: Exit");
				NextState();
		}

		IEnumerator RespawnState()
		{
				showDebugState("Respawn: Enter");
				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Stand);
				while (state == MooseState3D.Respawn)
				{
						yield return null;
				}
				showDebugState("Respawn: Exit");
				NextState();
		}

		IEnumerator DyingState()
		{
				showDebugState("Dying: Enter");
				rigidbody.Sleep ();
				rigidbody.velocity = Vector3.zero;
				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Dying);
				soundManager.Death.Play ();
				while (state == MooseState3D.Dying)
				{
						if(transform.position != deathPosition)
						{
								float step = 1 * Time.deltaTime;
								transform.position = Vector3.MoveTowards(transform.position, deathPosition, step);
						}
						else
						{
								state = MooseState3D.Dead;
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
				rigidbody.velocity = Vector2.zero;
				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Dead);
				rigidbody.WakeUp ();
				state = MooseState3D.Stand;
				//animation.Play ("Death");

				while (state == MooseState3D.Dead)
				{
						//need to work out a spawner
						//gameObject.SetActive (false);

						yield return null;
				}
				showDebugState("Dead: Exit");
				Debug.Log ("Dead: Exit");
				if(isPlayer)
				{
						GameManager.instance.spawnPlayer ();
				}
				else
						PoolingSystem.DestroyAPS (this.gameObject);

				NextState();
		}

		public virtual void Start () 
		{
				Transform mychildtransform = transform.FindChild("SoundManager");
				soundManager = mychildtransform.gameObject.GetComponent<Sounds>();
				//Debug.Log ("activated");

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
				//guiTextMooseState.text = s;
		}

		public void setCharged()
		{
				mooseAnimator.changeState (MooseAC3D.MooseBodyState.Charged);// = MooseAnimator.BodyState.Charged;
				state = MooseState3D.Charged;
		}


		void showDebugState(string debugString)
		{
				//Debug.Log (this.gameObject.tag);
				//if(gameObject.tag == "EnemyMoose")
						//Debug.Log (debugString);
		}



		public virtual void OnCollisionEnter(Collision coll)
		{
				//Debug.Log ("enemy hit");

				if (coll.gameObject.tag == "EnemyMoose") 
				{
						//state = MooseState.Ball;
						BaseMoose3D other = coll.gameObject.GetComponent<BaseMoose3D> ();


						if (state == MooseState3D.Walk && other.state == MooseState3D.Walk)
								return;
						else
								collisionStuff (coll);

				} 
				else if (coll.gameObject.tag == "Player")
						collisionStuff (coll);

				if (coll.gameObject.tag == "Wall")
						soundManager.WallHit.Play ();

		}


		void collisionStuff(Collision coll)
		{
				state = MooseState3D.ShootOut;
				GameManager.instance.AddSplosion ((Vector3)coll.contacts[0].point);
				soundManager.MooseHitMoose.Play ();
				rigidbody.isKinematic = false;
				coll.gameObject.rigidbody.isKinematic = false;


		}

		void OnTriggerEnter(Collider coll)
		{
				if (coll.name == "Pit")
				{

						//Debug.Log ("entered hole");
						deathPosition = coll.transform.position;
						state = MooseState3D.Dying;
						//
				}
		}


		void OnEnable() 
		{
				//Debug.Log ("activated");
				//state = MooseState3D.Stand;
				//NextState();
		}
}
