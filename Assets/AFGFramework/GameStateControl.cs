using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameStateControl : MonoBehaviour {

	public static GameStateControl instance;
		public bool userCreated = false;
		public string userName;
		public string emailId  = "guestUser@gmail.com";    			// EmailId for the user creation
		public string password = "password";
	public float health;
	public float experience;


// Use this for initialization
	void Awake () 
	{
			if(instance == null)
			{
				DontDestroyOnLoad (gameObject);
				instance = this;
				
			}
			else if (instance != this)
			{
				Destroy (gameObject);
			}

	}




	#region SaveLoad
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");

		PlayerData data = new PlayerData ();
				data.userCreated = true;
				data.userName = userName;
				data.emailId = emailId;
				data.password = password;


		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

				PlayerData data = (PlayerData)bf.Deserialize(file);
				file.Close ();
						userCreated = data.userCreated;
						userName = data.userName;
						emailId = data.emailId;
						password = data.password;
		}

	}
	#endregion

}

[Serializable]
class PlayerData
{
		public bool userCreated;
		public string userName;
		public string emailId  = "guestUser@gmail.com";    			// EmailId for the user creation
		public string password = "password";
}

[Serializable]
class LevelModel
{
	public string modelID;
	public string levelTitle;
	public uint price;
	public bool levelBought;
	public uint highscore;
}
