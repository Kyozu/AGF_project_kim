using UnityEngine;
using System.Collections;

public class MooseAnimator : MonoBehaviour {

		public bool animateMoose = false; //don't know
		public float rotationDirection = 0.0f; //don't know
		public float rotateSpeed = 30f; //the speed at which the moose rotates;

		public GameObject head;
		public Animator headAnim;
		public float headRadius;
		public float legRadius;

		public GameObject body;
		public Animator bodyAnimator;

		public GameObject legFrontRight;
		public GameObject legFrontLeft;
		public bool isBiped = false; //if you only want to have two legs
		public GameObject legBackRigth;
		public GameObject legBackLeft;

		public float LegFrontAngle;
		public float legBackAngle;

		public bool Walking;

		public enum BodyState
		{
				Normal = 0, //normal body
				Ball, //charging body
				Charging,
				Charged,
				Other
		};

		public enum HeadState
		{
				headFR = 0,
				headFL = 1,
				headBL = 2,
				headBR = 3
		};

		public BodyState state; // { get, set [{ ;headAnim.SetInteger ("headState", 0); }]};
		public BodyState preState;

	// Use this for initialization
	void Start () 
		{
				state = BodyState.Normal;
				rotateToAngle (0, true);
		}
	
	// Update is called once per frame
	void FixedUpdate () 
		{
				if (state == preState)
				{
						preState = state;
				}
				else
				{
						int stateint = (int)state;


						//if (preState == BodyState.Ball)
								//rotateToAngle (0, true);



						bodyAnimator.SetInteger ("bodyState", stateint);

						preState = state;

				}

				legsAnimate(Walking);

				if(state == BodyState.Ball)
				{
						rotateToAngle (0, false);
				}

		}

		void legsAnimate(bool animate)
		{

				Animator legACFR = legFrontRight.GetComponent<Animator> ();
				legACFR.SetBool ("Walking", animate);
				Animator legACFL = legFrontLeft.GetComponent<Animator> ();
				legACFL.SetBool ("Walking", animate);
				Animator legACBR = legBackRigth.GetComponent<Animator> ();
				legACBR.SetBool ("Walking", animate);
				Animator legACBL = legBackLeft.GetComponent<Animator> ();
				legACBL.SetBool ("Walking", animate);

		}

		public void rotateToAngle(float angle, bool isRadian)
		{
				float angleInRadians = 0f;

				//if not radians convert to radians.
				if (isRadian)
				{
						angleInRadians = angle;
				}
				else
				{
						rotationDirection += rotateSpeed;

						if (rotationDirection >= 180f) rotationDirection -= 360.0f;

						if (rotationDirection <= -180f)   rotationDirection += 360.0f;

						angleInRadians = rotationDirection * Mathf.Deg2Rad; //CC_DEGREES_TO_RADIANS(self.rotationDirection);
				}

				//[self changeHeadAnim:angleInRadians];

				float headX = headRadius* Mathf.Cos(angleInRadians) + body.transform.position.x;
				float headY = headRadius* Mathf.Sin(angleInRadians) + body.transform.position.y;
				head.transform.position = new Vector3 (headX, headY, 0f);// ccp(headX, headY);
				changeHeadAnim (angleInRadians);

				float angleDegrees = angleInRadians * Mathf.Rad2Deg;
				if (state < BodyState.Charging)
				{
						
						body.transform.rotation =  Quaternion.Euler(0 ,0 , angleDegrees); //    CC_RADIANS_TO_DEGREES(-angleInRadians);
						//[self rotateLegs:_body.rotation];
						
				}
				else
				{
						body.transform.rotation =  Quaternion.Euler(0 ,0 , 0);
						//this places the legs so they can't be seen with the charged body
				}

				rotateLegs (-angleDegrees);

		}

		void changeHeadAnim(float angleInRadians)
		{
				float angleDegrees = angleInRadians * Mathf.Rad2Deg;

				//CCLOG(@"Angle: %f", angleDegrees);
				if (angleDegrees > 0f) {
						//_head.zOrder = _body.zOrder - 1;
						if (angleDegrees > 90f) {
								headAnim.SetInteger ("headState", 2); //HeadState.headBL);
						} else {
								headAnim.SetInteger ("headState", 3); //HeadState.headBR);
						}

				} else {
						//_head.zOrder = _body.zOrder + 1;
						if (angleDegrees < -90f) {
								headAnim.SetInteger ("headState", 1); //HeadState.headFL);
						} else {
								headAnim.SetInteger ("headState", 0);//HeadState.headFR);
						}


				}
		}

		void rotateLegs(float angleInDegrees)
		{
				//leg degrees

				//front right
				float flr = -(angleInDegrees + LegFrontAngle);
				//front left
				float fll = -(angleInDegrees - LegFrontAngle);
				//back right
				float blr = -(angleInDegrees + legBackAngle);
				//back left
				float bll = -(angleInDegrees - legBackAngle);

				float angleInRadians = flr * Mathf.Deg2Rad;
				float legX = legRadius* Mathf.Cos(angleInRadians) + body.transform.position.x;
				float legY = legRadius* Mathf.Sin(angleInRadians) + body.transform.position.y;
				legFrontRight.transform.position = new Vector3 (legX, legY, 0f);

				angleInRadians = fll * Mathf.Deg2Rad;
				 legX = legRadius* Mathf.Cos(angleInRadians) + body.transform.position.x;
				 legY = legRadius* Mathf.Sin(angleInRadians) + body.transform.position.y;
				legFrontLeft.transform.position = new Vector3 (legX, legY, 0f);

				angleInRadians = blr * Mathf.Deg2Rad;
				 legX = legRadius* Mathf.Cos(angleInRadians) + body.transform.position.x;
				 legY = legRadius* Mathf.Sin(angleInRadians) + body.transform.position.y;
				legBackRigth.transform.position = new Vector3 (legX, legY, 0f);

				angleInRadians = bll * Mathf.Deg2Rad;
				 legX = legRadius* Mathf.Cos(angleInRadians) + body.transform.position.x;
				 legY = legRadius* Mathf.Sin(angleInRadians) + body.transform.position.y;
				legBackLeft.transform.position = new Vector3 (legX, legY, 0f);

		}


}
