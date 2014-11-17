using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class PlayerMooseFSM : BaseObjectFSM {

		//static int CHARGE_RADIUS = 40;
		//static int CHARGE_RADIUS_SQ = CHARGE_RADIUS * CHARGE_RADIUS;

		public delegate void SlowMotionAction(bool Enable);
		public static event SlowMotionAction Slowmotion;


		public float SlowMotionSpeed = 0.2f;

		//private Vector3 touchPosition = Vector3.zero;
		private Vector2 StartPosition = Vector3.zero;
		private Vector2 EndPosition = Vector3.zero;

		public Slider Arrow;


		bool touchActive = false;
		bool arrowActive = false;
		//	private Vector3 moveToDirection = Vector3.zero;
		//private float moveSqrMag = 0;
		//private float lastMoveSqrMag = 0;

		public bool isDead = false;

		public float distAS;
		//private bool allowAttack = true;
		/*
		void OnTap(TapGesture gesture)
		{
				StartPosition = Camera.main.ScreenPointToRay(Input.mousePosition).GetPoint(0);

		if(currentState != MooseState.None) return;
		touchPosition = Camera.main.ScreenToWorldPoint(gesture.Position);
		touchPosition = new Vector3(touchPosition.x, touchPosition.y, myTransform.position.z);
		lastMoveSqrMag = Mathf.Infinity;
		moveToDirection = (touchPosition - myTransform.position).normalized;

		}
				*/

		void OnDrag(DragGesture gesture) 
		{

				if(state == MooseState.Dying || state == MooseState.Dead || state == MooseState.Respawn) return;



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
		}

		void FixedUpdate()
		{
		}

		void Update()
		{
				//do not draw anything.
				if(state == MooseState.Dying || state == MooseState.Dead || state == MooseState.Respawn) return;

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

				if(state == MooseState.Ball)
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
						Arrow.gameObject.SetActive(true);
						arrowActive = true;
				}

				if(state != MooseState.Ball)
				{
						if(state == MooseState.Stand)
						{
								//should be going into charging state;
								//Debug.Log ("going to charging mode");
								state = MooseState.Charging;
								//ChangeGUITextMooseState(state.ToString());
						}
						//direction = (EndPosition - StartPosition).normalized;
						Vector2 directions = (EndPosition - StartPosition).normalized;
						angle = Mathf.Atan2 (directions.y, directions.x);// * Mathf.Rad2Deg;

						mooseAnimator.rotateToAngle (angle, true);
				}

				//Debug.Log ("moved");
		}

		void TouchEnded(Vector2 touchPosition)
		{

				touchActive = false;

				if(arrowActive)
				{
						Arrow.gameObject.SetActive(false);
						arrowActive = false;
				}

				slowMotion = false;
				if(Slowmotion != null)
				{
						Slowmotion (slowMotion);
				}
				Time.timeScale = 1f;
				mooseAnimator.rotateSpeed = rotationSpeed;


				state = MooseState.ShootOut;
				//ChangeGUITextMooseState(currentState.ToString());
				rigidbody2D.velocity = Vector2.zero;
				Vector2 directions = (EndPosition - StartPosition).normalized;
				rigidbody2D.AddForce(directions * attackForce);

				//Debug.Log ("ended");

		}

		void drawArrows()
		{
				Vector2 pointA =transform.position;

				Vector2 pointABvector = (EndPosition - StartPosition).normalized;

				Vector2 pointS = pointA + (pointABvector * distAS);

				Vector2 directions = EndPosition - StartPosition;
				float angleInDeg = Mathf.Atan2 (directions.y, directions.x) * Mathf.Rad2Deg;


				//TODO: FOR SOME UNKNOWN REASON THE FUCKING DEGREES ARE REVERSED
				Arrow.transform.position = Camera.main.WorldToScreenPoint(pointS);
				Arrow.transform.rotation =  Quaternion.Euler(0 ,0 , angleInDeg);

		}

		void updateArrow()
		{
				Vector2 pointA =transform.position;

				Vector2 pointABvector = (EndPosition - StartPosition).normalized;

				Vector2 pointS = pointA + (pointABvector * distAS); 

				Arrow.transform.position = Camera.main.WorldToScreenPoint(pointS);

		}
}
