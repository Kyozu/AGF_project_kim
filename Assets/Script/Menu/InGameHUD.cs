using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public class InGameHUD : MonoBehaviour {


		#region TextPool
		public static InGameHUD Instance;

		[System.Serializable]
		public class PoolingItems
		{
				public GameObject prefab;
				public int amount;
		}

		/// <summary>
		/// These fields will hold all the different types of assets you wish to pool.
		/// </summary>
		public PoolingItems[] poolingItems;

		public List<GameObject>[] pooledItems;
		/// <summary>
		/// The default pooling amount for each object type, in case the pooling amount is not mentioned or is 0.
		/// </summary>
		public int defaultPoolAmount = 10;

		/// <summary>
		/// Do you want the pool to expand in case more instances of pooled objects are required.
		/// </summary>
		public bool poolExpand = true;

		public GameObject ScoreText;
		#endregion

		#region TweenScoreShit
		public float timeto = 0.8f;
		Vector2 startPos;
		public RectTransform finishRect;

		public float duration = 0.8f;
		public float ScaleDuration = 0.2f;
		public int PunchVibrato = 10;
		public float PunchElasticity = 0.9f;
		public bool snap = false;
		public Vector3 ScaleVector;
		public float Amplitude= 0.4f;
		#endregion
		public Text CurrentScoreText;
		int currentScore = 0;
		int incrementScoreTo = 0;

		AudioSource TickSound; 
		public float positionUP = 50f;

		void Awake ()
		{
				Instance = this;
		}
	
		void Start () 
		{
				CurrentScoreText.text = "0";
				TickSound = GetComponentInParent<AudioSource> ();

				//ScoreText = ;
				pooledItems = new List<GameObject>[poolingItems.Length];

				for(int i=0; i<poolingItems.Length; i++)
				{
						pooledItems[i] = new List<GameObject>();

						int poolingAmount;
						if(poolingItems[i].amount > 0) poolingAmount = poolingItems[i].amount;
						else poolingAmount = defaultPoolAmount;

						for(int j=0; j<poolingAmount; j++)
						{
								GameObject newItem = (GameObject) Instantiate(poolingItems[i].prefab);
								newItem.SetActive(false);
								pooledItems[i].Add(newItem);
								newItem.transform.SetParent (transform);
						}
				}

		}
	

		public void addScore(int score, Vector3 position)
		{
				// Things before the tween...
				// ...

            Vector3 newPosition = new Vector3(position.x, position.y + positionUP, position.z);
            GameObject TextGO = InstantiateAPS("ScoreText", newPosition, Quaternion.Euler(Vector3.zero));// as GameObject;
				TextGO.transform.SetParent (transform);
				Text myText = TextGO.GetComponent<Text> ();

				myText.transform.localScale = new Vector3 (0.1f, 0.1f, 1f);
				myText.text =  score.ToString();
				myText.color = Color.black;
				Vector3 moveupText = myText.rectTransform.position;
				//Debug.Log ("moveup " + moveupText);
				Vector3 normalUPPos = new Vector3(moveupText.x, moveupText.y + positionUP, moveupText.z);
				//Debug.Log ("moveNormal " + normalUPPos);
				// Tween
				// Sequence with onComplete callback
				Sequence seq = DOTween.Sequence()
						.OnComplete(()=>finishScore(TextGO,score));
                seq.Append(myText.rectTransform.DOScale(new Vector3(1f, 1f, 1f), duration).SetEase(DG.Tweening.Ease.OutElastic, Amplitude));//.SetDelay(0.3f));
				seq.Join(myText.rectTransform.DOMove(normalUPPos, duration, false));
				seq.Join (DOTween.ToAlpha (() => myText.color, x => myText.color = x, 0, seq.Duration (false)));
				seq.Append(myText.rectTransform.DOScale(Vector3.zero, ScaleDuration));
				seq.Append(CurrentScoreText.rectTransform.DOScale(ScaleVector, ScaleDuration));
				incrementScoreTo += score;
				seq.Join(DOTween.To(()=> currentScore, x=> currentScore = x, incrementScoreTo, 0.5f)
						.OnUpdate(()=> CurrentScoreText.text = currentScore.ToString()).OnStart(playTickerSound).OnComplete(stopTickerSound));

				seq.Append(CurrentScoreText.rectTransform.DOScale(new Vector3 (1f, 1f, 1f), ScaleDuration));
		}

		void playTickerSound()
		{
				TickSound.Play ();
		}
		void stopTickerSound()
		{
				TickSound.Stop ();
		}

		void finishScore(GameObject destroy, int Score)
		{
				DestroyAPS (destroy);
		}


		#region PoolMethods
		public GameObject InstantiateAPS (string itemType, Vector3 itemPosition, Quaternion itemRotation)
		{
				GameObject newObject = GetPooledItem(itemType);
				newObject.SetActive(true);
				newObject.transform.position = itemPosition;
				newObject.transform.rotation = itemRotation;
				newObject.transform.localScale = new Vector3 (1f, 1f, 0f);
				return newObject;
		}

		public GameObject GetPooledItem (string itemType)
		{
				for(int i=0; i<poolingItems.Length; i++)
				{
						if(poolingItems[i].prefab.name == itemType)
						{
								for(int j=0; j<pooledItems[i].Count; j++)
								{
										if(!pooledItems[i][j].activeInHierarchy)
										{
												return pooledItems[i][j];
										}
								}

								if(poolExpand)
								{
										GameObject newItem = (GameObject) Instantiate(poolingItems[i].prefab);
										newItem.SetActive(false);
										pooledItems[i].Add(newItem);
										newItem.transform.SetParent(transform);
										return newItem;
								}

								break;
						}
				}

				return null;
		}

		public static void DestroyAPS(GameObject myObject)
		{
				myObject.SetActive(false);
		}

		#endregion
}
