using UnityEngine;
using System.Collections;

public class buttonPress : MonoBehaviour {

		public void ButtonPress()
		{
				AudioSource sound = GetComponent<AudioSource> ();
				sound.Play ();
		}
}
