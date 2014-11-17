using UnityEngine;
using System.Collections;

public class SplosionDestroy : MonoBehaviour {

	
		public void finishSplosion()
		{
				//destroying parent, because parent is the actual pool object.
				PoolingSystem.DestroyAPS (this.gameObject);
		}
}
