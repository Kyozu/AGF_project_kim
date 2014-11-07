using System;
using UnityEngine;
namespace AssemblyCSharp
{
	public class MooseConstants 
	{
		public string apiKey  ="5487070f1541b603abe362ddab48a734ed3267f3bd09ab2e979d078b72a6d566";						// API key that you have receieved after the success of app creation from AppHQ
		public string secretKey ="db7d2ae2aed94aab414b05be8934cc34157c863c61a46174ab7ffeadb8138aa1";					// SECRET key that you have receieved after the success of app creation from AppHQ
		public string gameName ="8 moose pool";						// Name of the game which you can create from AppHQ console by clicking 
		// Business Service -> Game Service -> Game -> Add Game
		public string description  = "<Enter_the_description>";			// Enter your description
		public string userName  = "<Name of the User>"; 				// Name of the user for which you have to save score or create user etc. 
		public string sessionId  = "<Session Id of the User>";   		// Session id of the user for which you have to have invalidate his session 
		public string emailId  = "<EmailId of The User>";    			// EmailId for the user creation
		public string updateEmailId   ="<Id that has to be upadated>";  // EmailId which has to be updated in user profile.
	}  
}