using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class GPL_basics : MonoBehaviour {

		public int ScoreToPost = 100;
	public Text myText;
	// Use this for initialization
	void Start () {
	
				// recommended for debugging:
				//PlayGamesPlatform.DebugLogEnabled = true;

				// Activate the Google Play Games platform

	}
		public void signIn()
		{
				Social.localUser.Authenticate((bool success) => {
						// handle success or failure
						if(success)
						{
								//do some shit camera shake?
							myText.text = "log in sucess";
						}
						else
						{
							myText.text = "log in fail";
			}
		}
			);

		}
		public void signOut()
		{
				// sign out
				//((PlayGamesPlatform) Social.Active).SignOut();
		}

		public void postScore()
		{
				//string LeaderboardToShow = "CgkI667N0YsIEAIQBg";
				// post score 12345 to leaderboard ID "Cfji293fjsie_QA")
				//Social.ReportScore(ScoreToPost, LeaderboardToShow, (bool success) => {
						// handle success or failure
				//});
		}

		public void unlockAchievement()
		{
				//
				//string AchievmentToUnlock = "CgkI667N0YsIEAIQBQ";
				//Social.ReportProgress(AchievmentToUnlock, 100.0f, (bool success) => {
						// handle success or failure
				//});
		}

		public void incrementAchievement()
		{
				//string IncrementalAchievment = "CgkI667N0YsIEAIQAQ";
				// increment achievement by steps
				//int steps = 1;

				//((PlayGamesPlatform) Social.Active).IncrementAchievement(
				//		IncrementalAchievment, steps, (bool success) => {
								// handle success or failure
				//		});
		}

		public void showLeaderBoards()
		{
				//Social.ShowLeaderboardUI();
		}

		public void showSpecificLeaderBoard()
		{
				//string LeaderboardToShow = "CgkI667N0YsIEAIQBg";
				// show leaderboard UI
				//((PlayGamesPlatform) Social.Active).ShowLeaderboardUI(LeaderboardToShow);
		}

		public void showAchievements()
		{
				//Social.ShowAchievementsUI();
		}


}
