using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class ShowLeaderboard : MonoBehaviour {

		/// <summary>
		/// The user object.
		/// 
		/// set this with your prefab for the leaderboard item.
		/// </summary>
		public GameObject userObject;

		/// <summary>
		/// The height of the field.
		/// 
		/// set this to the preferred Height of the LeaderboardPlayerPrefab + padding from the layout group
		/// </summary>
		public float FieldHeight = 64.0f;

		/// <summary>
		/// The leaderboard loaded.
		/// if leaderboard is loaded destroy it first then create the new one.
		/// </summary>
		public bool leaderboardLoaded = false;

		/// <summary>
		/// The leaderboard for debug.
		/// </summary>
		public userEntryForDebug[] LeaderboardForDebug;

		/// <summary>
		/// All current items in the leaderboard.
		/// so that we could destroy this later on.
		/// </summary>
		List<GameObject> allCurrentUsers;


	// Use this for initializationLis
	void Start () 
	{
	
	}
	
		/// <summary>
		/// Constructs the leaderboard.
		/// 
		/// TODO: clean this up.
		/// </summary>
		/// <param name="result">Result.</param>
		private void ConstructLeaderboard (PlayFab.ClientModels.GetLeaderboardResult result)
		{
				//LeaderboardHighScores.Clear ();
				foreach (PlayFab.ClientModels.PlayerLeaderboardEntry entry in result.Leaderboard) 
				{
						if (entry.DisplayName != null)
						{
								//UserLayoutObject place = (UserLayoutObject) Instantiate (userObject.transform);

								//userObject.UserRank.text = entry.Position.ToString();
								//userObject.UserName.text = entry.DisplayName;
								//userObject.UserScore.text = entry.StatValue.ToString();
						}
								//LeaderboardHighScores.Add (entry.DisplayName, (uint)entry.StatValue); 
						else
						{
								//Instantiate (userObject.transform);
								//userObject.UserRank.text = entry.Position.ToString();
								//userObject.UserName.text = entry.PlayFabId;
								//userObject.UserScore.text = entry.StatValue.ToString();
						}
								//LeaderboardHighScores.Add (entry.PlayFabId, (uint)entry.StatValue); 
				}
				leaderboardLoaded = true;
		}



		public void DebugConstruct()
		{
				if(!leaderboardLoaded)
				{
						if(allCurrentUsers == null)
						{
								// setting lenght is more performance wise, but leaderboards can change in lenght.
								allCurrentUsers = new List<GameObject>();
							
						}

						foreach ( userEntryForDebug entry in LeaderboardForDebug)
						{
								if (entry.DisplayName != null)
								{
										GameObject playerField = (GameObject) Instantiate(userObject);
										playerField.transform.SetParent (transform);
										UserLayoutObject player = playerField.GetComponent<UserLayoutObject>();
										player.setObject (entry.Position, entry.DisplayName, entry.StatValue);

										//for reference.
										allCurrentUsers.Add (playerField);
								}
								else
								{
										GameObject playerField = (GameObject)Instantiate (userObject);
										playerField.transform.SetParent (transform);
										UserLayoutObject player = playerField.GetComponent<UserLayoutObject>();
										player.setObject (entry.Position, entry.PlayFabId, entry.StatValue);

										//for reference.
										allCurrentUsers.Add (playerField);
								}

								//expands the rect transform to have nice scrolling
								RectTransform myTransform = (RectTransform) transform;
								myTransform.sizeDelta = new Vector2( myTransform.rect.width, myTransform.rect.height + FieldHeight);

						}
						leaderboardLoaded = true;
				}
				else
				{
						foreach(GameObject playerobject in allCurrentUsers)
						{
								Destroy (playerobject);
						}

						allCurrentUsers.Clear ();

						RectTransform myTransform = (RectTransform) transform;
						myTransform.sizeDelta = new Vector2( myTransform.rect.width, 0);

						leaderboardLoaded = false;
				}



		}



		void OnPlayFabError(PlayFabError error)
		{
				Debug.Log ("Got an error: " + error.ErrorMessage);
		}
}

[System.Serializable]
public class userEntryForDebug
{
		public int Position;
		public string DisplayName;
		public string PlayFabId;
		public int StatValue;
}

