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

		private Vector3 _lastRotationTarget = Vector3.zero;
		private float _normalZRotation = 0f;

		public virtual void Setup(TCard card, Vector3 position)//, IDeckConfiguration deckConfiguration)
		{
			_cardImage.sprite = card.Sprite;
			_name.text = card.Name;
			_description.text = card.Description;
			transform.position = position;
			TurnFaceDown(0f);

			//_cardBacking.sprite = deckConfiguration.Backing;

			// todo : card frame based on rarity?
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