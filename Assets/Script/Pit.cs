using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour 
{
		public RectTransform rectPos;


		void OnStart()
		{
				transform.position = rectPos.position;

		}
}
