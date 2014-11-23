using UnityEngine;
using System.Collections;

public class MooseAC3D : MonoBehaviour {

		public float rotationDirection = 0.0f; //don't know
		public float rotateSpeed = 30f;

		public enum MooseBodyState
		{
				Stand = 0, //no movement
				Walk, //walk to point
				Charging, //start charging
				Charged, //ready to ball
				Ball, //balling around the room
				Dying, //play the dying animation
				Dead // set to inactive and stuff like that

		};

		public MooseBodyState state;

		Vector3 rotateAngle;
		public float rotationSpeed;
		public float slowMotionRotationSpeed;


		IEnumerator StandState()
		{
				showDebugState("Stand: Enter");
				while (state == MooseBodyState.Stand)
				{
						animation.Play ("Wait");
						yield return null;
				}
				showDebugState("Stand: Exit");
				NextState();
		}

		IEnumerator WalkState()
		{
				showDebugState("Walk: Enter");
				while (state == MooseBodyState.Walk)
				{
						animation.Play ("Walk");
						yield return null;
				}
				showDebugState("Walk: Exit");
				NextState();
		}

		IEnumerator ChargingState()
		{
				showDebugState("Charging: Enter");
				while (state == MooseBodyState.Charging)
				{
						animation.Play ("Attack");
						yield return null;
				}
				showDebugState("Charging: Exit");
				NextState();
		}

		IEnumerator ChargedState()
		{
				showDebugState("Charged: Enter");
				while (state == MooseBodyState.Charged)
				{
						animation.Play ("Attack");
						yield return null;
				}
				showDebugState("Charged: Exit");
				NextState();
		}

		IEnumerator BallState()
		{

				showDebugState("Ball: Enter");
				while (state == MooseBodyState.Ball)
				{
						animation.Play ("Damage");
						transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y + rotateSpeed, 0);
						yield return null;
				}

				showDebugState("Ball: Exit");
				NextState();
		}

		IEnumerator DyingState()
		{
				showDebugState("Dying: Enter");
				animation.Play ("Dead");
				while (state == MooseBodyState.Dying)
				{

						yield return null;
				}
				showDebugState("Dying: Exit");

				NextState();
		}

		IEnumerator DeadState()
		{
				showDebugState("Dead: Enter");
				animation.Play ("Dead");
				while (state == MooseBodyState.Dead)
				{
						yield return null;
				}
				showDebugState("Dead: Exit");
				NextState();
		}

		public virtual void Start () 
		{

				NextState();
		}


		public void NextState () 
		{
				string methodName = state.ToString() + "State";
				StartCoroutine (methodName);
		}


		void showDebugState(string debugString)
		{
				//Debug.Log (this.gameObject.tag);
				//if(gameObject.tag == "EnemyMoose")
				//		Debug.Log (debugString);
		}

		public void changeState(MooseBodyState MooseState)
		{
				state = MooseState;
		}


		public void rotateToAngle(float angle)
		{
				float angle3d = angle - 90;
				if (angle3d < 0)
						angle3d += 360;

				Debug.Log ("angle: " + angle3d);
				transform.rotation = Quaternion.Euler(0, -angle3d , 0);

		}
}
