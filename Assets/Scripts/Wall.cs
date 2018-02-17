using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	public Sprite damageSprite;
	public int hp = 4;

	private SpriteRenderer mySpriteRenderer;

	// Use this for initialization
	void Awake () 
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();

	}

	public void DamageWall (int loss)
	{
		mySpriteRenderer.sprite = damageSprite;
		hp -= loss;

		if (hp <= 0)
		{
			gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
