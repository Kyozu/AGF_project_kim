using UnityEngine;
using System.Collections;

public class callChargeScript : MonoBehaviour {

	// Use this for initialization
		public void setCharge()
		{
				BaseObjectFSM baseObject = this.gameObject.GetComponentInParent<BaseObjectFSM> ();
				baseObject.setCharged ();
		}
}
