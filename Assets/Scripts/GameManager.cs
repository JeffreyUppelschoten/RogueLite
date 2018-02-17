﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public float turnDelay = .2f;
	public static GameManager instance = null;
	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	public bool playersTurn = true;

	private int level = 3;
	private List<Enemy> enemies;
	private bool enemiesMoving;

	private void Awake() 
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
		enemies = new List <Enemy>();
		boardScript = GetComponent<BoardManager>();
		InitGame();
	}
	
	private void InitGame()
	{
		enemies.Clear();

		boardScript.SetupScene(level);

	}

	public void GameOver()
	{
		enabled = false;
	}

	private void Update() 
	{
		if (playersTurn || enemiesMoving)
		{
			return;
		}

		StartCoroutine(MoveEnemies());
	}

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add (script);
	}

	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);

		if (enemies.Count == 0)
		{
			yield return new WaitForSeconds(turnDelay);
		}

		//Move enemies and let the player wait for the moving
		for (int i = 0; i < enemies.Count; i++)
		{
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}
}
