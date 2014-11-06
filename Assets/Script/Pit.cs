using UnityEngine;
using System.Collections;

public class Pit : MonoBehaviour 
{
	public int pScore = 0;
	public int eScore = 0;
	public Rect rect1 = new Rect(0,0,100,50);
	public Rect rect2 = new Rect(10,100,100,50);
	public string playerScoreText;
	public string enemyScoreText;

	void Awake()
	{
		playerScoreText = "Player "+pScore;
		enemyScoreText = "Enemy "+eScore;
	}

	void OnGUI()
	{
		GUI.Label(rect1,playerScoreText);
		GUI.Label(rect2,enemyScoreText);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.name == "Player")
		{
			eScore++;
			enemyScoreText = "Enemy "+eScore;
						//Destroy (col.gameObject);
		}
		else if(col.name == "BadMoose")
		{
			pScore++;
			playerScoreText = "Player "+pScore;
		}
		//col.GetComponent<BaseObject>().Death();
	}
}
