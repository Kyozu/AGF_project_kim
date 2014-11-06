using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour 
{

		PoolingSystem pS;

		// To spawn the prefab as it is
		//pS.InstantiateAPS("prefabName");

		// To spawn the prefab at a position and with a specific rotation
		//pS.InstantiateAPS("prefabName", _position, _rotation);

		// Same as above, also makes the spawned prefab a child of "parent" gameobject
		//pS.InstantiateAPS("prefabName", _position, _rotation, parent);


	public float timeScale = 1;


	public float chargeToChargeForce = 200;
	private static GameManager _instance;
		#region textStuff
		[HideInInspector]
		public static InGameHUD GameCanvas;
		#endregion
	
	public static GameManager instance
	{
		get
		{
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<GameManager>();
			return _instance;
		}
	}

	public void RegisterObject()//BaseObject b)
	{
		//int count = objects.Count;
		//b.id = count;
		//objects.Add(b);
	}

	void Start()
	{
		Time.timeScale = timeScale;

				GameCanvas = InGameHUD.Instance;

				if(pS == null)
				{
						pS = PoolingSystem.Instance;
				}

				Debug.Log (pS);
	}

	public void PauseButton()
		{
				Application.LoadLevel (1);
		}



		public void addScore(int score, Vector3 position)
		{
				Vector3 ScreenPos = Camera.main.WorldToScreenPoint (position);
				GameCanvas.addScore (score, ScreenPos);

		}

		//void 

		void finishTween()
		{

				Debug.Log ("EaseFinished");

		}

		/*public void destroySplosion(GameObject splosion)
		{
				PoolingSystem.DestroyAPS (splosion);
		}*/

		public void AddSplosion(Vector3 position)
		{
				pS.InstantiateAPS ("Splosion", position, Quaternion.Euler (Vector3.zero));
		}
}
