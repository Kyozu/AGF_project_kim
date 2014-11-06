using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlaceAtBorders : MonoBehaviour {

    public Camera MainCam;
    public GameObject BorderObject;
    public RectTransform areaSize;
    public Vector2 offset;
    public Vector2 startPos;
    public Vector2 randomRange;
    Vector2 currentPos;



	// Use this for initialization
	void Start () 
    {

				float vertExtent = MainCam.orthographicSize;
				float horzExtent = vertExtent * MainCam.aspect; //Screen.width / Screen.height;

				Vector2 camSize = new Vector2 (MainCam.transform.position.x + horzExtent, MainCam.transform.position.y + vertExtent);
				string LAYER_NAME = "forestOnTop";
				Vector2 startingCamPos = new Vector2(MainCam.transform.position.x - horzExtent, MainCam.transform.position.y - vertExtent);
				currentPos = startingCamPos;//MainCam.ScreenToWorldPoint(startingCamPos);
				SpriteRenderer tempRend = BorderObject.GetComponent<SpriteRenderer>();
				float xPosOffset = tempRend.bounds.size.x * 0.5f;
				float yPosOffset = tempRend.bounds.size.y * 0.5f;

        //SpriteRenderer objectWidhts = BorderObject.GetComponent<SpriteRenderer>();

				while(currentPos.y < camSize.y)
				{

						float randomXoff = Random.Range (randomRange.x, randomRange.y);

						GameObject tempTree1 =   Instantiate(BorderObject, new Vector3(MainCam.transform.position.x  + ( horzExtent + offset.x + randomXoff), currentPos.y, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
						tempTree1.renderer.sortingLayerName = LAYER_NAME;
						GameObject tempTree2 = Instantiate(BorderObject, new Vector3(MainCam.transform.position.x - (horzExtent + offset.x + randomXoff), currentPos.y, 0), Quaternion.Euler(Vector3.zero)) as GameObject;
						tempTree2.renderer.sortingLayerName = LAYER_NAME;
						currentPos.y += yPosOffset;
				}


		while (currentPos.x < camSize.x) // + objectWidhts.bounds.size.x)
        {
						float randomYoff = Random.Range (randomRange.x, randomRange.y);

						GameObject tempTree1 =  Instantiate(BorderObject, new Vector3(currentPos.x, MainCam.transform.position.y - (vertExtent + randomYoff), -2), Quaternion.Euler(Vector3.zero)) as GameObject;
						tempTree1.renderer.sortingLayerName = LAYER_NAME;
						GameObject tempTree2 = Instantiate(BorderObject, new Vector3(currentPos.x, MainCam.transform.position.y + (vertExtent - offset.y + randomYoff), 0), Quaternion.Euler(Vector3.zero)) as GameObject;
						tempTree2.renderer.sortingLayerName = LAYER_NAME;
						currentPos.x += xPosOffset;

        }


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
