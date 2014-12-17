using UnityEngine;
using System.Collections;

[AddComponentMenu("WG_Engine/Camera Shake")]
public class CameraShake : MonoBehaviour 
{
		public float duration = 0.5f;
		public float speed = 1.0f;
		public float magnitude = 0.1f;

		public Vector3 originalCameraPosition;

		float shakeAmt = 0;

		public Camera mainCamera;

		void Start()
		{
				if (mainCamera == null)
						mainCamera = Camera.main;

				originalCameraPosition = mainCamera.transform.position;
		}

		void Update() {
				//Press F on the keyboard to simulate the effect
				/*
				if(Input.GetKeyDown(KeyCode.F)) {
						PlayShake();
				}*/
		}

		//This function is used outside (or inside) the script
		public void PlayShake() {
				StopAllCoroutines();
				StartCoroutine("Shake");
		}

		private IEnumerator Shake() {
				float elapsed = 0.0f;

				//Vector3 originalCamPos = transform.position;
				float randomStart = Random.Range(-1000.0f, 1000.0f);

				while (elapsed < duration) {
						elapsed += Time.deltaTime;			

						float percentComplete = elapsed / duration;			

						float damper = 1.0f - Mathf.Clamp(1.5f * percentComplete - 1.0f, 1.0f, 1.0f);
						float alpha = randomStart + speed * percentComplete;

						float x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
						float y = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

						x *= magnitude * damper;
						y *= magnitude * damper;

						mainCamera.transform.position = new Vector3(x, y, originalCameraPosition.z);

						yield return 0;
				}

				mainCamera.transform.position = Vector3.Lerp( mainCamera.transform.position, originalCameraPosition, Time.deltaTime * 5f);
		}





		void OnCollisionEnter(Collision coll) 
		{
				magnitude = coll.relativeVelocity.magnitude * .025f;
				PlayShake ();
				//shakeAmt = coll.relativeVelocity.magnitude * .0025f;
				//InvokeRepeating("CameraShakes", 0, .0S1f);
				//Invoke("StopShaking", 0.3f);

		}

		void CameraShakes()
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