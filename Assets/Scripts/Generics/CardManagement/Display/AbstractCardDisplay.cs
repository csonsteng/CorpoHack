using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LogicPuddle.CardManagement
{
	public class AbstractCardDisplay<TCard, TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget> :
		MonoBehaviour, IDraggableCard
		where TCard : AbstractCardData<TRarity, TEffect, TTargetData, TTargetConfiguration, TTarget>
		where TRarity : System.Enum
		where TEffect : AbstractCardEffect<TTargetData, TTargetConfiguration, TTarget>
		where TTargetData : AbstractTargetData<TTargetConfiguration, TTarget>
		where TTargetConfiguration : AbstractTargetConfiguration<TTarget>
		where TTarget : System.Enum
	{
		[SerializeField] private Image _cardImage;

		[SerializeField] private Image _cardFrame;
		[SerializeField] private Image _cardBacking;
		[SerializeField] private TextMeshProUGUI _name;
		[SerializeField] private TextMeshProUGUI _description;

		private Vector3 _baseSize;

		private Tween _scaleTween;
		private Tween _positionTween;
		private Tween _rotationTween;

		private Vector3 _mouseOffset;
		private Vector3 _dragStartPosition;

		private HandDisplayController _handController;

		private void Awake()
		{
			_baseSize = transform.localScale;
		}


		public float BaseWidth => _baseSize.x;
		public float CardThickness => _baseSize.z;

		public void Setup(TCard card, IDeckConfiguration deckConfiguration)
		{
			_cardImage.sprite = card.Sprite;
			_name.text = card.Name;
			_description.text = card.Description;

			_cardBacking.sprite = deckConfiguration.Backing;

			// todo : card frame based on rarity?
		}

		public void Register(HandDisplayController controller)
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

		public void TweenScale(float scale, float duration)
		{
			if (_scaleTween.IsActive())
			{
				_scaleTween.Kill();
			}

			_scaleTween = transform.DOScale(scale * _baseSize, duration);
		}
		public void TweenPosition(Vector3 position, float duration)
		{
			if (_positionTween.IsActive())
			{
				_positionTween.Kill();
			}

			_positionTween = transform.DOLocalMove(position, duration);
		}

		public void TweenPositionLocal(Vector3 position, float duration)
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
}