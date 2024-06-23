using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	[SerializeField] private Vector3 _baseScale;
	private Hand _hand;

	private Tween _scaleTween;
	private Tween _positionTween;
	private Tween _rotationTween;

	private Vector3 _mouseOffset;
	private Vector3 _dragStartPosition;

	public float BaseWidth => _baseScale.x;
	public float CardThickness => _baseScale.z;

	public void Register(Hand hand)
	{
		_hand = hand;
	}

	private void OnMouseEnter()
	{
		_hand.CardHovered(this);
	}
	private void OnMouseExit()
	{
		_hand.CardUnhovered(this);
	}

	private void OnMouseDown()
	{
		var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
		_mouseOffset = screenPosition - Input.mousePosition;
		_dragStartPosition = transform.position;
		_hand.CardDragged(this);
	}

	private void OnMouseDrag()
	{
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + _mouseOffset);
	}

	private void OnMouseUp()
	{
		_hand.CardDropped(this);
	}

	public void ReturnToHand()
	{
		transform.position = _dragStartPosition;
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

		_positionTween = transform.DOLocalMove(position, duration);
	}
	public void TweenRotation(float rotation, float duration)
	{
		if (_rotationTween.IsActive())
		{
			_rotationTween.Kill();
		}

		_rotationTween = transform.DOLocalRotate(new Vector3(0f, 0f, rotation), duration);
	}
}
