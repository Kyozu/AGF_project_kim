using UnityEngine;
using System.Collections;

public class SplosionDestroy : MonoBehaviour {

	
		public void finishSplosion()
		{
				PoolingSystem.DestroyAPS (this.gameObject);
		}
}
