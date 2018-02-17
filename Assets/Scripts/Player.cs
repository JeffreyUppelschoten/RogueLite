using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

	public int wallDamage = 1;
	public int pointssPerFood = 10;
	public int pointsPerSoda = 20;
	public float restartLevelDelay = 1f;

	private Animator animator;
	private int food;
	// Use this for initialization
	protected override void Start () 
	{
		animator = GetComponent<Animator>();

		food = GameManager.instance.playerFoodPoints;

		base.Start();
	}

	private void OnDisable() 
	{
		GameManager.instance.playerFoodPoints = food;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!GameManager.instance.playersTurn) return;
		
		int horizontal = 0;
		int vertical = 0;

		horizontal = (int) Input.GetAxisRaw("Horizontal");
		vertical = (int) Input.GetAxisRaw("Vertical");

		if (horizontal != 0)
		{
			vertical = 0;
		}

		if (horizontal != 0 || vertical != 0)
		{
			AttemptMove<Wall> (horizontal, vertical);
		}
	}

	//Attempt to move
	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		food--;

		base.AttemptMove <T> (xDir, yDir);

		RaycastHit2D hit;

		CheckIfGameOver();

		GameManager.instance.playersTurn = false;
	}

	//Check for interactable triggers
	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag == "Exit")
		{
			Invoke("Restart", restartLevelDelay);
			enabled = false;
		}
		else if (other.tag == "Food")
		{
			food += pointssPerFood;
			other.gameObject.SetActive(false);
		}
		else if (other.tag == "Soda")
		{
			food += pointsPerSoda;
			other.gameObject.SetActive(false);
		}
	}

	//If cant move means there is a wall we can destroy
	protected override void OnCantMove<T>(T component)
	{
		Wall hitWall = component as Wall;
		hitWall.DamageWall(wallDamage);
		animator.SetTrigger("isAttacking");
	}

	//Restart the scene
	private void Restart()
	{
		SceneManager.LoadScene(0);
	}

	//Lose food which can be called when hit
	public void LoseFood (int loss)
	{
		animator.SetTrigger("isHit");
		food -= loss;
		CheckIfGameOver();
	}
	//Function to check if the player is game over
	private void CheckIfGameOver()
	{
		if (food <= 0f)
		{
			GameManager.instance.GameOver();
		}
	}
}
