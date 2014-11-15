using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.GameCenter;
using PlayFab;
using PlayFab.ClientModels;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

using System.Security.Cryptography;
using System.Text;

public class AutoLogin : MonoBehaviour {

		public static AutoLogin instance;
		#region playFab
		public bool CreateUser = true;
		public bool PlayFabUserCreated = false;
		public string userName;
		public string password;
		public string playFabID;
		string errorLabel;
		public string TitleID = "DE16";
		#endregion

		/// <summary>
		/// The console.
		/// shows debug output for now
		/// </summary>
		public Text console;

		void Awake () 
		{
				if(instance == null)
				{
						DontDestroyOnLoad (gameObject);
						instance = this;
						Load ();

				}
				else if (instance != this)
				{
						Destroy (gameObject);
				}

		}


	// Use this for initialization
		void Start ()
		{
				PlayFabSettings.UseDevelopmentEnvironment = false;
				PlayFabSettings.TitleId = TitleID;
				// Authenticate and register a ProcessAuthentication callback
				// This call needs to be made before we can proceed to other calls in the Social API


				Social.localUser.Authenticate (ProcessAuthentication);

				if(PlayFabData.SkipLogin && PlayFabData.AuthKey != null)
				{
						//PlayFabGameBridge.gameState = 3;
						console.text = "already authenticated with playfab and have authkey";
						Time.timeScale = 1.0f; // unpause
				} 
				else
				{
						if (PlayFabUserCreated) 
						{
								console.text = "loging in to playfab user";
								loginPlayFab ();
						}
				}

		}

		// This function gets called when Authenticate completes
		// Note that if the operation is successful, Social.localUser will contain data from the server. 
		void ProcessAuthentication (bool success) 
		{
				if (success) 
				{
						// Request loaded achievements, and register a callback for processing them
						console.text = "User authed";
						registerPlayFab ();
				}
				else
						console.text = "Failed";
		}





		#region PlayFab

		void loginPlayFab()
		{
				LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
				request.Username = userName;
				request.Password = password;
				request.TitleId = PlayFabData.TitleId;
				PlayFabClientAPI.LoginWithPlayFab(request,OnLoginResult,OnPlayFabError);
				//PlayFabClientAPI.LinkGameCenterAccount


		}

		public void linkGameCenter(LinkGameCenterAccountResult result)
		{
				//LinkGameCenterAccountRequest linkrequest = new LinkGameCenterAccountRequest ();
				//linkrequest.GameCenterId = Social.localUser.id;
				//PlayFabClientAPI.LinkGameCenterAccount (linkrequest, linkGameCenter, OnPlayFabError);
				//PlayFabGameBridge.gameState = 3;	// switch to playing the game; hide this dialog
				//Time.timeScale = 1.0f;	// unpause...
				//PlayFabData.AuthKey = result.;
				//if(!PlayFabData.AngryBotsModActivated)Application.LoadLevel (nextScene);

		}

		public void OnLoginResult(LoginResult result)
		{
				//PlayFabGameBridge.gameState = 3;	// switch to playing the game; hide this dialog
				Time.timeScale = 1.0f;	// unpause...
				PlayFabData.AuthKey = result.SessionTicket;
				//if(!PlayFabData.AngryBotsModActivated)Application.LoadLevel (nextScene);

		}

		void registerPlayFab()
		{
#if UNITY_IOS
				LoginWithGameCenterRequest request = new LoginWithGameCenterRequest();
				request.TitleId = TitleID;//PlayFabSettings.TitleId;
				request.PlayerId = Social.localUser.id;
				request.CreateAccount = CreateUser;
				Debug.Log("TitleId : "+request.TitleId);
				PlayFabClientAPI.LoginWithGameCenter(request,OnRegisterResult,OnPlayFabError);
#else

				LoginWithGameCenterRequest request = new LoginWithGameCenterRequest();
				request.TitleId = TitleID;//PlayFabSettings.TitleId;
				request.PlayerId = Social.localUser.id;
				userName = Social.localUser.userName;

				request.CreateAccount = CreateUser;
				Debug.Log("TitleId : "+request.TitleId);
				PlayFabClientAPI.LoginWithGameCenter(request,OnRegisterResult,OnPlayFabError);

				LoginWithAndroidDeviceIDRequest requestG = new LoginWithAndroidDeviceIDRequest();
				requestG.TitleId = TitleID;
				requestG.OS = SystemInfo.operatingSystem;
				requestG.AndroidDevice = SystemInfo.deviceModel;
				requestG.AndroidDeviceId = SystemInfo.deviceUniqueIdentifier;
				requestG.CreateAccount = CreateUser;
				PlayFabClientAPI.LoginWithAndroidDeviceID (requestG, OnRegisterResult, OnPlayFabError);
#endif
		}

		public void OnRegisterResult(LoginResult result)
		{
				// now need to store a title-specific display name for this game
				// this is the name that will show up in the leaderboard
				UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest ();
				request.DisplayName = result.PlayFabId;
				PlayFabData.AuthKey = result.SessionTicket;
				console.text = console.text + " user created:" + result.PlayFabId;


				if(CreateUser )
				{
						playFabID = result.PlayFabId;
						CreateUser = false;
						password = GetUniqueKey (8);
						console.text = console.text + " password: " + password + " ; ";
						Save ();
				}


				//PlayFabClientAPI.UpdateUserTitleDisplayName (request, NameUpdated, OnPlayFabError);
		}

		public static string GetUniqueKey(int maxSize)
		{
				char[] chars = new char[62];
				chars =
						"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
				byte[] data = new byte[1];
				RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
				crypto.GetNonZeroBytes(data);
				data = new byte[maxSize];
				crypto.GetNonZeroBytes(data);
				StringBuilder result = new StringBuilder(maxSize);
				foreach (byte b in data)
				{
						result.Append(chars[b % (chars.Length)]);
				}
				return result.ToString();
		}

		void OnPlayFabError(PlayFabError error)
		{
				Debug.Log ("Got an error: " + error.Error);
				if ((error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Password")) || (error.Error == PlayFabErrorCode.InvalidPassword))
				{
						errorLabel = "invalidPassword";
				}
				else if ((error.Error == PlayFabErrorCode.InvalidParams && error.ErrorDetails.ContainsKey("Username")) || (error.Error == PlayFabErrorCode.InvalidUsername))
				{
						errorLabel = "invalidUsername";
				}
				else if (error.Error == PlayFabErrorCode.EmailAddressNotAvailable)
				{
						errorLabel = "emailNotAvailable";
				}
				else if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
				{
						errorLabel = "usernameNotAvailable";
				}

				console.text = errorLabel;
		}
		#endregion


		#region SaveLoad
		public void Save()
		{
				if (!CreateUser) 
				{
						BinaryFormatter bf = new BinaryFormatter ();
						FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

						PlayerPlayFabData data = new PlayerPlayFabData ();
						data.CreateUser = CreateUser;
						data.playFabID = playFabID;
						data.TitleID = TitleID;

						if (userName != null)
								data.userName = userName;

						data.password = password;

						bf.Serialize (file, data);
						file.Close ();
						console.text = "    data saved...    ";
				}
				else
				{
						Debug.Log ("shit happens, user not created, not saving");
				}
		}

		public void Load()
		{
				if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
				{
						console.text = "data loaded...    ";
						BinaryFormatter bf = new BinaryFormatter ();
						FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

						PlayerPlayFabData data = (PlayerPlayFabData)bf.Deserialize(file);
						file.Close ();
						CreateUser = data.CreateUser;
						userName = data.userName;
						password = data.password;
						playFabID = data.playFabID;
						TitleID = data.TitleID;
				}

		}
		#endregion




}

[Serializable]
class PlayerPlayFabData
{
		public bool CreateUser;
		public string userName;
		public string password;
		public string playFabID;
		public string TitleID;
}