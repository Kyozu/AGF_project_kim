using UnityEngine;
using System.Collections;

public class AddDynamicCollider2D : MonoBehaviour {

	// Use this for initialization
	void Start () {

				RectTransform parentRect = GetComponent<RectTransform> ();

				BoxCollider2D collider2D = this.gameObject.AddComponent<BoxCollider2D> ();
				collider2D.size = parentRect.rect.size;
				collider2D.center = new Vector2(0.0f,0.0f);


	}
	
	// Update is called once per frame
}
