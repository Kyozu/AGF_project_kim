using UnityEngine;
using System.Collections;

public class FacebookIntegrationWG : MonoBehaviour {

	public static FacebookIntegrationWG instance;
	public bool LoggenIn;
	
	// Use this for initialization
	void Awake () 
	{
		if(instance == null)
		{
			DontDestroyOnLoad (gameObject);
			instance = this;
			FB.Init(SetInit, OnHideUnity);
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
		
	}

	#region facebook
	private void SetInit()                                                                       
	{                                                                                            
		//Util.Log("SetInit");                                                                  
		enabled = true; // "enabled" is a property inherited from MonoBehaviour                  
		if (FB.IsLoggedIn)                                                                       
		{                                                                                        
			//Util.Log("Already logged in");                                                    
			OnLoggedIn();                                                                        
		}                                                                                        
	}                                                                                            
	
	private void OnHideUnity(bool isGameShown)                                                   
	{                                                                                            
		//Util.Log("OnHideUnity");                                                              
		if (!isGameShown)                                                                        
		{                                                                                        
			// pause the game - we will need to hide                                             
			Time.timeScale = 0;                                                                  
		}                                                                                        
		else                                                                                     
		{                                                                                        
			// start the game back up - we're getting focus again                                
			Time.timeScale = 1;                                                                  
		}                                                                                        
	}
	
	private void setupLogin()
	{
		if (!FB.IsLoggedIn)                                                                                              
		{   
			//change this
			FB.Login("email,publish_actions", LoginCallback);                                                                                                          
		} 
	}
	
	void LoginCallback(FBResult result)                                                        
	{                                                                                          
		//Util.Log("LoginCallback");                                                          
		
		if (FB.IsLoggedIn)                                                                     
		{                                                                                      
			OnLoggedIn();                                                                      
		}                                                                                      
	}                                                                                          
	
	void OnLoggedIn()                                                                          
	{                                                                                          
		//Util.Log("Logged in. ID: " + FB.UserId); 
		//enable buttons;
		//Util.Log("Logged in. ID: " + FB.UserId);
		
		// Reqest player info and profile picture                                                                           
		FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);  
		//LoadPicture(Util.GetPictureURL("me", 128, 128),MyPictureCallback);  
	} 
	
	void APICallback(FBResult result)                                                                                              
	{                                                                                                                              
		//Util.Log("APICallback");                                                                                                
		if (result.Error != null)                                                                                                  
		{                                                                                                                          
			//Util.LogError(result.Error);                                                                                           
			// Let's just try again                                                                                                
			FB.API("/me?fields=id,first_name,friends.limit(100).fields(first_name,id)", Facebook.HttpMethod.GET, APICallback);     
			return;                                                                                                                
		}                                                                                                                          
		
		//profile = Util.DeserializeJSONProfile(result.Text);                                                                        
		//GameStateManager.Username = profile["first_name"];                                                                         
		//friends = Util.DeserializeJSONFriends(result.Text);                                                                        
	}                                                                                                                              
	
	void MyPictureCallback(Texture texture)                                                                                        
	{                                                                                                                              
		//Util.Log("MyPictureCallback");                                                                                          
		
		if (texture == null)                                                                                                  
		{                                                                                                                          
			// Let's just try again
			//LoadPicture(Util.GetPictureURL("me", 128, 128),MyPictureCallback);                               
			return;                                                                                                                
		}                                                                                                                          
		//GameStateManager.UserTexture = texture;                                                                             
	}
	#endregion
}
