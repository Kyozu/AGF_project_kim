using UnityEngine;
using System.Collections;
[AddComponentMenu("WG_Engine/Input Controller")]
public class InputController : MonoBehaviour {
	

	// Update is called once per frame
		void Update()
		{
				Debug.Log ("update");

		#if UNITY_EDITOR
		Debug.Log("Unity Editor");
		#endif
				if (Input.touchCount > 0)
				{
						Debug.Log ("Touch");
						Vector2 touchPosition = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
						if (Input.GetTouch(0).phase == TouchPhase.Began) 
						{
								// Input.GetTouch(0)
								TouchBegan (touchPosition);
						}
						else if (Input.GetTouch(0).phase == TouchPhase.Moved)
						{
								TouchMoved (touchPosition);
						}
						else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
						{
								TouchEnded (touchPosition);
						}
				}

				/*
				int i = 0;
				while (i < Input.touchCount) {
						Debug.Log ("Touch in while");
						if (Input.GetTouch(i).phase == TouchPhase.Began) {
								Debug.Log ("Touchbegan ");
						}
						++i;
				}*/

		}

		void TouchBegan(Vector2 touchPosition)
		{
				//StartPosition = touchPosition;
				Debug.Log ("Start position" + touchPosition);

				Debug.Log ("moose pos:" + transform.position);

		}

		void TouchMoved(Vector2 touchPosition)
		{
				Debug.Log ("moved");
		}

		void TouchEnded(Vector2 touchPosition)
		{
				Debug.Log ("ended");

		}
}
