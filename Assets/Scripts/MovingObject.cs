﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = .1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider2D;
	private Rigidbody2D myRigidbody;
	private float inverseMoveTime;

	// Use this for initialization
	protected virtual void Start()
	{
		boxCollider2D = GetComponent<BoxCollider2D>();
		myRigidbody = GetComponent<Rigidbody2D>();
		inverseMoveTime = 1f / moveTime;
	}
	
	protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
	{
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2(xDir, yDir);
		
		boxCollider2D.enabled = false;
		hit = Physics2D.Linecast(start, end, blockingLayer);
		boxCollider2D.enabled = true;

		if (hit.transform == null)
		{
			StartCoroutine(SmoothMovement(end));
			return true;
		}
		
		return false;
	}

	protected IEnumerator SmoothMovement (Vector2 end)
	{
		float sqrRemainingDistance = ((Vector2)transform.position - end).sqrMagnitude;

		while (sqrRemainingDistance > float.Epsilon)
		{
			Vector2 newPosition = Vector2.MoveTowards(myRigidbody.position, end, inverseMoveTime * Time.deltaTime);
			myRigidbody.MovePosition(newPosition);
			sqrRemainingDistance = ((Vector2)transform.position - end).sqrMagnitude;

			yield return null;
		}
	}

	protected virtual void AttemptMove <T> (int xDir, int yDir)
	where T : Component
	{
		RaycastHit2D hit;
		bool canMove = Move(xDir, yDir, out hit);

		if (hit.transform == null)
			return;

		T hitComponent = hit.transform.GetComponent<T>();

		if (!canMove && hitComponent != null)
			OnCantMove(hitComponent);

	}

	protected abstract void OnCantMove <T> (T Component)
		where T : Component;
}
