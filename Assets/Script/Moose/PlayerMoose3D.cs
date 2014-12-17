using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMoose3D : BaseMoose3D {

		public delegate void SlowMotionAction(bool Enable);
		public static event SlowMotionAction Slowmotion;
		public float SlowMotionSpeed = 0.2f;

		//private Vector3 touchPosition = Vector3.zero;
		private Vector2 StartPosition = Vector3.zero;
		private Vector2 EndPosition = Vector3.zero;

		public GameObject Arrow;


		bool touchActive = false;
		bool arrowActive = false;
		//	private Vector3 moveToDirection = Vector3.zero;
		//private float moveSqrMag = 0;
		//private float lastMoveSqrMag = 0;
		public float distAS;

		//public float attackForce;

		//bool slowMotion = false;

		void OnDrag(DragGesture gesture) 
		{

				if(state == MooseState3D.Dying || state == MooseState3D.Dead || state == MooseState3D.Respawn) return;



				if (gesture.Phase == ContinuousGesturePhase.Started) 
				{
						Vector2 touchPosition = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0);
						TouchBegan (touchPosition);
				}
				else if (gesture.Phase == ContinuousGesturePhase.Updated)
				{
						Vector2 touchPosition = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0);
						TouchMoved (touchPosition);
				}
				else if (gesture.Phase == ContinuousGesturePhase.Ended)
				{
						Vector2 touchPosition = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0);
						TouchEnded (touchPosition);
				}
		}

		void Awake()
		{
				//Init();
				Time.timeScale = 1f;
				isPlayer = true;
		}


		void Update()
		{
				//do not draw anything.
				if(state == MooseState3D.Dying || state == MooseState3D.Dead || state == MooseState3D.Respawn) return;

				if(touchActive)
				{
						updateArrow ();
				}

		}

		void TouchBegan(Vector2 touchPosition)
		{
				StartPosition = touchPosition;
				touchActive = true;
				//Debug.Log ("Start position" + StartPosition);

				//Debug.Log ("moose pos:" + transform.position);

				if(state == MooseState3D.Ball)
				{
						//slowMotion
						slowMotion = true;
						if(Slowmotion != null)
						{
								Slowmotion (slowMotion);
						}

						Time.timeScale = SlowMotionSpeed;
						mooseAnimator.rotateSpeed = slowMotionRotationSpeed;
				}


		}

		void TouchMoved(Vector2 touchPosition)
		{

				EndPosition = touchPosition;

				drawArrows ();

				if(!arrowActive)
				{
						Arrow.SetActive(true);
						arrowActive = true;
				}

				if(state != MooseState3D.Ball)
				{
						if(state == MooseState3D.Stand)
						{
								//should be going into charging state;
								//Debug.Log ("going to charging mode");
								state = MooseState3D.Charging;
								//ChangeGUITextMooseState(state.ToString());
						}
						//direction = (EndPosition - StartPosition).normalized;
						Vector2 directions = (EndPosition - StartPosition).normalized;
						angle = Mathf.Atan2 (directions.y, directions.x) * Mathf.Rad2Deg;

						mooseAnimator.rotateToAngle (angle);
				}

				//Debug.Log ("moved");
		}

		void TouchEnded(Vector2 touchPosition)
		{

				touchActive = false;

				if(arrowActive)
				{
						Arrow.SetActive(false);
						arrowActive = false;
				}

				slowMotion = false;
				if(Slowmotion != null)
				{
						Slowmotion (slowMotion);
				}
				Time.timeScale = 1f;
				//mooseAnimator.rotateSpeed = rotationSpeed;


				state = MooseState3D.ShootOut;
				//ChangeGUITextMooseState(currentState.ToString());
				rigidbody.velocity = Vector3.zero;
				Vector2 directions = (EndPosition - StartPosition).normalized;
				Vector3 direction3D = new Vector3 (directions.x, 0, directions.y);
				rigidbody.AddForce(direction3D * attackForce);

				//Debug.Log ("ended");

		}

		void drawArrows()
		{
				Vector2 directions = (EndPosition - StartPosition).normalized;
				angle = Mathf.Atan2 (directions.y, directions.x) * Mathf.Rad2Deg;

				float angle3d = angle - 90;
				if (angle3d < 0)
						angle3d += 360;

				//Debug.Log ("angle: " + angle3d);
				Arrow.transform.rotation = Quaternion.AngleAxis( -angle3d , transform.up);

		}

		void updateArrow()
		{
				Vector2 directions = (EndPosition - StartPosition).normalized;
				angle = Mathf.Atan2 (directions.y, directions.x) * Mathf.Rad2Deg;

				float angle3d = angle - 90;
				if (angle3d < 0)
						angle3d += 360;

				//Debug.Log ("angle: " + angle3d);
				Arrow.transform.rotation = Quaternion.AngleAxis( -angle3d , transform.up);

		}
}
