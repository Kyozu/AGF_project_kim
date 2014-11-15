using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// User layout object.
/// this object represents one user field in the layout group.
/// you can add more stuff to this for e.g.
/// 
/// if the user is number 1, you could add a crown symbol.
/// 
/// play around anything you want to add to the user field should be here and no where else!
/// </summary>
public class UserLayoutObject : MonoBehaviour 
{
		/// <summary>
		/// The user rank in the leaderboards.
		/// </summary>
		public Text UserRank;

		/// <summary>
		/// The name of the user.
		/// </summary>
		public Text UserName;

		/// <summary>
		/// The user score.
		/// </summary>
		public Text UserScore;

		public void setObject(int Position, string Name, int Score)
		{
				UserRank.text = Position.ToString() + ".";
				UserName.text = Name;
				UserScore.text = Score.ToString ();

		}
}
