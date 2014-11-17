using UnityEngine;
using System.Collections;

public class DestroySelfAPS : MonoBehaviour {

		/// <summary>
		/// Finishs the splosion.
		/// destroys self and puts this back into usable pooling objects
		/// </summary>
		public void destroySelf()
		{
				PoolingSystem.DestroyAPS (this.transform.parent.gameObject);
		}

		void OnActive()
		{

		}
}
