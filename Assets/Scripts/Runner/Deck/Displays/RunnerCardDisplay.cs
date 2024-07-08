using DG.Tweening;
using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
    public class RunnerCardDisplay : MonoBehaviour
	{
		[SerializeField] private Image _cardImage;

		[SerializeField] private Image _cardFrame;
		[SerializeField] private Image _cardBacking;
		[SerializeField] private TextMeshProUGUI _name;
		[SerializeField] private TextMeshProUGUI _description;

		[SerializeField] private GameObject _selectedIndicator;

		private Vector3 _baseSize;

		private Tween _scaleTween;
		private Tween _positionTween;
		private Tween _rotationTween;

		private Vector3 _mouseOffset;
		private Vector3 _dragStartPosition;

		private RunnerHandDisplay _handController;
		private RunnerCardData _data;
		public float BaseWidth => _baseSize.x;
		public float CardThickness => _baseSize.z;
		public RunnerCardData Data => _data;

		private Vector3 _lastRotationTarget = Vector3.zero;
		private float _normalZRotation = 0f;

		private bool _selected = false;

		private void Awake()
		{
			_baseSize = transform.localScale;
		}
		public void Setup(RunnerCardData card, Vector3 position)
		{
			_data = card;
			_cardImage.sprite = card.Sprite;
			_name.text = card.Name;
			_description.text = card.Description;
			transform.position = position;
			TurnFaceDown(0f);

			//_cardBacking.sprite = deckConfiguration.Backing;

			// todo : card frame based on rarity?
			_selectedIndicator.SetActive(false);

		}

		public void TurnFaceDown(float duration)
		{
			_name.gameObject.SetActive(false);
			_description.gameObject.SetActive(false);
			_cardImage.gameObject.SetActive(false);

			TweenRotation(new Vector3(0f, 180f, _normalZRotation), duration);
		}

		public void TurnFaceUp(float duration)
		{
			_name.gameObject.SetActive(true);
			_description.gameObject.SetActive(true);
			_cardImage.gameObject.SetActive(true);

			TweenRotation(new Vector3(0f, 0f, _normalZRotation), duration);
		}

		public void Register(RunnerHandDisplay controller)
		{
			_handController = controller;
		}
		private void OnMouseEnter()
		{
			_handController.CardHovered(this);
		}
		private void OnMouseExit()
		{
			_handController.CardUnhovered(this);
		}

		private void OnMouseUpAsButton()
		{
			if (_handController.CardSelected(this))
			{
				_selected = true;
				_selectedIndicator.SetActive(_selected);
			}
		}

		public void UnSelect()
		{
			_selected = false;
			_selectedIndicator.SetActive(_selected);
		}

		private void OnMouseDown()
		{
			var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
			_mouseOffset = screenPosition - Input.mousePosition;
			_dragStartPosition = transform.position;
			_handController.CardDragged(this);
		}

		private void OnMouseDrag()
		{
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + _mouseOffset);
		}

		private void OnMouseUp()
		{
			_handController.CardDropped(this);
		}

		public void ReturnToHand()
		{
			transform.position = _dragStartPosition;
		}

		public void Scale(float scale, float duration)
		{
			if (_scaleTween.IsActive())
			{
				_scaleTween.Kill();
			}

			_scaleTween = transform.DOScale(scale * _baseSize, duration);
		}
		public void MovePosition(Vector3 position, float duration)
		{
			if (_positionTween.IsActive())
			{
				_positionTween.Kill();
			}
			_positionTween = transform.DOMove(position, duration);
		}

		public void MovePositionLocal(Vector3 position, float duration)
		{
			if (_positionTween.IsActive())
			{
				_positionTween.Kill();
			}
			_positionTween = transform.DOLocalMove(position, duration);
		}
		public void RotateInPlane(float rotation, float duration)
		{
			_normalZRotation = rotation;
			TweenRotation(new Vector3(0f, _lastRotationTarget.y, rotation), duration);
		}
		private void TweenRotation(Vector3 rotation, float duration)
		{
			if (_rotationTween.IsActive())
			{
				_rotationTween.Kill();
			}

			_lastRotationTarget = IsFaceUp(rotation) ? rotation : new Vector3(0f, rotation.y, -_normalZRotation);

			_rotationTween = transform.DOLocalRotate(_lastRotationTarget, duration);
		}

		private bool IsFaceUp(Vector3 rotation) => Mathf.Abs(rotation.y) < 90f;
	}
}