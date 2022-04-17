using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimation : MonoBehaviour
{
	private float time = 0.5f;
	private Vector3 cardSet;
	private Vector3 cardSetAce;

	void Start()
	{
		cardSetAce = new Vector3(0.0f, 0.0f, -0.01f);
	}

	public void MovingAnimation(GameObject card, Transform destination,Vector3 cardSet)
	{
		LeanTween.move(card, destination.position + cardSet, time).setEase(LeanTweenType.easeOutQuad);
		//LeanTween.move(card, destination.position + cardSet, time);
		time = 0.5f;
		card.transform.parent = destination;
	}
}