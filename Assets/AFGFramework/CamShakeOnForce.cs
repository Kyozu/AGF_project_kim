using UnityEngine;
using System.Collections;

[AddComponentMenu("WG_Engine/Cam Shake Force")]
public class CamShakeOnForce : MonoBehaviour 
{

		Vector3 originalCameraPosition;

		float shakeAmt = 0;

		public Camera mainCamera;

		void Start()
		{
				originalCameraPosition = new Vector3(0,0,-40);
				if (mainCamera == null)
						mainCamera = Camera.main;
		}

		void OnCollisionEnter2D(Collision2D coll) 
		{

				shakeAmt = coll.relativeVelocity.magnitude * .0025f;
				InvokeRepeating("CameraShake", 0, .01f);
				Invoke("StopShaking", 0.3f);

		}

		void CameraShake()
		{
				if(shakeAmt>0) 
				{
						float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
						Vector3 pp = mainCamera.transform.position;
						pp.y+= quakeAmt;
						pp.x += quakeAmt;// can also add to x and/or z
						mainCamera.transform.position = pp;
				}
		}

		void StopShaking()
		{
				CancelInvoke("CameraShake");
				mainCamera.transform.position = originalCameraPosition;
		}

}