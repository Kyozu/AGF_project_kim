using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

		AudioSource buttonSound;

	public GameObject[] allTrees;



	public int amountOffTrees = 10;
	GameObject[] forest;

	public float scaleOffset = 0.3f;
	public float startScale = 2.6f;

	// Use this for initialization
	void Start () 
	{

				buttonSound = GetComponent<AudioSource> ();

	}

	// Update is called once per frame
	void Update () 
	{

	
	}



		public void PlayButton()
		{
				buttonSound.Play ();
				Application.LoadLevel (0);



		}

		public void CreditsButton ()
		{
				buttonSound.Play ();
				Debug.Log ("credits pressed");

		}

		public void RateButton ()
		{
				buttonSound.Play ();
				Debug.Log ("rate pressed");

		}
}
