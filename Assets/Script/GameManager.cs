using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour 
{

		PoolingSystem pS;

		public GameObject PitOject;
		public string easyEnemy = "EnemyMoose";

		public Vector3 spawnValues;

		//debug
		public bool spawn = false;
		public Rect currentRect;

		//set this
		public RectTransform[] EnemySpawnRects;
		//used for spawing enemies
		Rect[] EnemySpawnRectsWorld;

		//spawning holes.
		//public RectTransform[] HoleSpawnRects;
		//public string PitName = "Pit";
		public GameObject Player;
		public RectTransform[] PlayerSpawnRects;
		//used for spawing enemies
		Rect[] PlayerSpawnRectsWorld;

		// To spawn the prefab as it is
		//pS.InstantiateAPS("prefabName");

		// To spawn the prefab at a position and with a specific rotation
		//pS.InstantiateAPS("prefabName", _position, _rotation);

		// Same as above, also makes the spawned prefab a child of "parent" gameobject
		//pS.InstantiateAPS("prefabName", _position, _rotation, parent);
		public int hazardCount;
		public float spawnWait;
		public float startWait;
		public float waveWait;

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
				

	void Start()
	{

				EnemySpawnRectsWorld = new Rect[EnemySpawnRects.Length];

				for(int i = 0; i < EnemySpawnRects.Length; i++)
				{
						RectTransform tempRect = EnemySpawnRects [i];
						Vector2 rectposition = tempRect.position;

						Vector3[] corners = new Vector3[4];


						tempRect.GetWorldCorners (corners);

						Rect spawnRect = new Rect(corners[0].x,corners[2].y, corners[2].x - corners[0].x, corners[0].y - corners[2].y);
						EnemySpawnRectsWorld[i] = spawnRect;
				}

				PlayerSpawnRectsWorld = new Rect[PlayerSpawnRects.Length];

				for(int i = 0; i < PlayerSpawnRects.Length; i++)
				{
						RectTransform tempRect = PlayerSpawnRects [i];
						Vector2 rectposition = tempRect.position;

						Vector3[] corners = new Vector3[4];


						tempRect.GetWorldCorners (corners);

						Rect spawnRect = new Rect(corners[0].x,corners[2].y, corners[2].x - corners[0].x, corners[0].y - corners[2].y);
						PlayerSpawnRectsWorld[i] = spawnRect;
				}


				Time.timeScale = timeScale;

				GameCanvas = InGameHUD.Instance;

				if(pS == null)
				{
						pS = PoolingSystem.Instance;
				}
				spawnPlayer ();
				StartCoroutine (SpawnWaves ());
				//Debug.Log (pS);
				//spawnHoles ();
	}
		/* this doesn't work as expected
		void spawnHoles()
		{
				foreach(RectTransform holetrans in HoleSpawnRects)
				{

						Vector3 position = holetrans.TransformPoint(holetrans.position);
						//position.z = -1;
						//CircleCollider2D hole = Instantiate (PitOject) as CircleCollider2D;


						//hole.AddComponent<CircleCollider2D>();
						//hole.transform.position = position;
						//hole.SetActive (false);
						//hole.SetActive (true);
						pS.InstantiateAPS (PitName, position, Quaternion.identity);
				}
		}*/

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
				pS.InstantiateAPS ("Splosion", position, Quaternion.identity);
		}

		public void AddGhost(Vector3 position, bool player)
		{
				pS.InstantiateAPS ("DeadMooseTransform", position, Quaternion.identity);

				if(player)
				{
						//do some player shit here
				}
		}

		void Update()
		{
				if(spawn)
				{
						SpawnWaves ();
						spawn = false;
				}
		}
				
		IEnumerator SpawnWaves ()
		{
				yield return new WaitForSeconds (startWait);
				while (true)
				{
						for (int i = 0; i < hazardCount; i++)
						{
								uint whichRect = (uint)Random.Range (0, EnemySpawnRectsWorld.Length);
								//RectTransform Rtrans = EnemySpawnRects [whichRect];
								Rect spawnRect = EnemySpawnRectsWorld [whichRect];

								//Debug.Log (spawnRect);

								currentRect = spawnRect;
								float spawnX = Random.Range (spawnRect.xMin, spawnRect.xMax);
								float spawnY = Random.Range (spawnRect.yMax, spawnRect.yMin);

								Vector3 spawnPosition = new Vector3 (spawnX, spawnY, 0);
								//Debug.Log ("Spawn World to View pos: " + spawnPosition);
								//spawnPosition = Camera.main.ViewportToWorldPoint(spawnPosition);
								//Debug.Log ("Spawn View To world pos: " + spawnPosition);
								pS.InstantiateAPS (easyEnemy, spawnPosition, Quaternion.identity);
								yield return new WaitForSeconds (spawnWait);
						}
						yield return new WaitForSeconds (waveWait);
				}
		}


		public void spawnPlayer()
		{
				uint whichRect = (uint)Random.Range (0, PlayerSpawnRectsWorld.Length);
				//RectTransform Rtrans = EnemySpawnRects [whichRect];
				Rect spawnRect = PlayerSpawnRectsWorld [whichRect];

				//Debug.Log (spawnRect);

				currentRect = spawnRect;
				float spawnX = Random.Range (spawnRect.xMin, spawnRect.xMax);
				float spawnY = Random.Range (spawnRect.yMax, spawnRect.yMin);

				Vector3 spawnPosition = new Vector3 (spawnX, spawnY, 0);
				//Debug.Log ("Spawn World to View pos: " + spawnPosition);
				//spawnPosition = Camera.main.ViewportToWorldPoint(spawnPosition);
				//Debug.Log ("Spawn View To world pos: " + spawnPosition);
				Player.transform.position = spawnPosition;

				//pS.InstantiateAPS (Player, spawnPosition, Quaternion.identity);
		}
		/*
		/// <summary>
		/// Fucking hacking to get the normal positions
		/// </summary>
		void OnDrawGizmos()
		{
				for(int i = 0; i < EnemySpawnRects.Length; i++)
				{
						RectTransform tempRect = EnemySpawnRects [i];
						//Vector2 rectposition = tempRect.position;

						Vector3[] corners = new Vector3[4];


						tempRect.GetWorldCorners (corners);

						Rect spawnRect = new Rect(corners[0].x,corners[2].y, corners[2].x - corners[0].x, corners[0].y - corners[2].y);

						//spawnRect.
						Vector2 topleft = new Vector2 (spawnRect.xMin, spawnRect.yMin);
						Vector2 topRight = new Vector2 (spawnRect.xMax, spawnRect.yMin);
						Vector2 bottomLeft = new Vector2 (spawnRect.xMin, spawnRect.yMax);
						Vector2 bottomRight = new Vector2 (spawnRect.xMax, spawnRect.yMax);

						Gizmos.DrawLine (topleft, topRight);
						Gizmos.DrawLine (topRight, bottomRight);
						Gizmos.DrawLine (bottomRight, bottomLeft);
						Gizmos.DrawLine (bottomLeft, topleft);
						//EnemySpawnRectsWorld[i] = spawnRect;
				}
		}
		*/

}