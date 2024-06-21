using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	[SerializeField] private Vector3 _baseScale;
	private Action<Card> _onHoverEnter;
	private Action<Card> _onHoverExit;

	private Tween _scaleTween;
	private Tween _positionTween;

	public void Register(Action<Card> onHoverEnter, Action<Card> onHoverExit)
	{
		_onHoverEnter = onHoverEnter;
		_onHoverExit = onHoverExit;
	}

	private void OnMouseEnter()
	{
		_onHoverEnter?.Invoke(this);
	}
	private void OnMouseExit()
	{
		_onHoverExit?.Invoke(this);
	}

	public void TweenScale(float scale, float duration)
	{
		if (_scaleTween.IsActive())
		{
			_scaleTween.Kill();
		}

		_scaleTween = transform.DOScale(scale * _baseScale, duration);
	}
	public void TweenPosition(Vector3 position, float duration)
	{
		if (_positionTween.IsActive())
		{
			_positionTween.Kill();
		}

		_positionTween = transform.DOMove(position, duration);
	}
}
