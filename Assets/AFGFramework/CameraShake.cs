using UnityEngine;
using System.Collections;
using DG.Tweening;

[AddComponentMenu("WG_Engine/Camera Shake")]
public class CameraShake : MonoBehaviour 
{
		public float duration = 0.5f;
		public float speed = 1.0f;
		public float magnitude = 0.2f;
		public bool CollisionBasedMagnitude = true;

		public TrailRenderer myTrail;
		public Color startColor;
		public Color bounceColor;
		public float startColourDuration = 0.1f;
		public float endcolorDuration = 0.2f;


		public Vector3 originalCameraPosition;

		float shakeAmt = 0;

		public Camera mainCamera;

		void Start()
		{
				if (mainCamera == null)
						mainCamera = Camera.main;

				originalCameraPosition = mainCamera.transform.position;

				magnitude = 1.0f;
				Debug.Log ("mag: " + magnitude);
		}

		void Update() {
				//Press F on the keyboard to simulate the effect

				if(Input.GetKeyDown(KeyCode.F)) {
						changeColor();
				}
		}

		//This function is used outside (or inside) the script
		public void PlayShake() {
				StopAllCoroutines();
				StartCoroutine("Shake");
		}

		public void changeColor()
		{
				//myTrail.material.SetColor("_TintColor", bounceColor);
				//Debug.Log ("color set");
				//myTrail.material.DOColor (bounceColor, "_TintColor", 0.1f);

				Sequence seq = DOTween.Sequence();
				seq.Append(myTrail.material.DOColor (bounceColor, "_TintColor", startColourDuration));//.SetDelay(0.3f));
				seq.Append(myTrail.material.DOColor (startColor, "_TintColor", endcolorDuration));
				//DOTween.Punch(()=> myTrail.material.GetColor("_TintColor"), x=> myTrail.material.color = x, bounceColor, 1);
		}

		private IEnumerator Shake() {
				float elapsed = 0.0f;

				//Vector3 originalCamPos = transform.position;
				float randomStart = Random.Range(-1000.0f, 1000.0f);

				while (elapsed < duration) {
						elapsed += Time.deltaTime;			
						//Debug.Log("elapsed :" + elapsed);
						float percentComplete = elapsed / duration;			
						//Debug.Log("percentComplete :" + percentComplete);
						float damper = 1.0f - Mathf.Clamp (1.5f * percentComplete - 1.0f, 0.0f, 1.0f);

						//Debug.Log("damper :" + damper);

						float alpha = randomStart + speed * percentComplete;
						//Debug.Log("alpha :" + alpha);
						float x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
						//Debug.Log("x before :" + x);

						float y = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;
						//Debug.Log("y before :" + y);

						x *= magnitude * damper;
						//Debug.Log("x :" + x);
						y *= magnitude * damper;
						//Debug.Log("y :" + y);

						mainCamera.transform.position = new Vector3(originalCameraPosition.x + x, originalCameraPosition.y + y, originalCameraPosition.z);

						yield return 0;
				}

				mainCamera.transform.position = Vector3.Lerp( mainCamera.transform.position, originalCameraPosition, Time.deltaTime * 5f);
				StopShaking ();
		}





		void OnCollisionEnter(Collision coll) 
		{
				if (CollisionBasedMagnitude)
						magnitude = coll.relativeVelocity.magnitude * .025f;
				changeColor ();
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