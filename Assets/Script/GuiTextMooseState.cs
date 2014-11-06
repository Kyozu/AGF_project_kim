using UnityEngine;
using System.Collections;

public class GuiTextMooseState : MonoBehaviour 
{
	[HideInInspector]
	public Transform myTransform;
	public Transform mooseTransform;

	private Vector3 pos = Vector3.zero;

	void Awake()
	{
		myTransform = transform;
		mooseTransform = myTransform.parent;
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		pos = Camera.main.WorldToViewportPoint(mooseTransform.position);
		pos = new Vector3(pos.x,pos.y+0.1f,pos.z);
		myTransform.position = pos;
	}
}
