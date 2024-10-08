using LogicPuddle.CardManagement;
using Runner.Deck;
using Runner.Deck.Effects;
using Runner.Target;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner
{
    public class RunnerHandDisplay : MonoBehaviour
	{
		[SerializeField] private RunnerCardDisplay _cardTemplate;
		[SerializeField] private Transform _cardHolder;

		[SerializeField] protected Vector2 _cardScaleSize;
		[SerializeField] protected float _animationDuration = 0.1f;
		[SerializeField] private float _maxRotation = 20f;
		[SerializeField] private float _hoverYLocation = 1.15f;

		[SerializeField] float _maxHandSpacing = 0.1f;

		[SerializeField] Transform _minHandPosition;
		[SerializeField] Transform _maxHandPosition;

		private RunnerHand _data;
		private RunnerCardDisplay _hovered;

		protected List<RunnerCardDisplay> _cards = new List<RunnerCardDisplay>();
		private RunnerCardDisplayManager _manager;
		private RunnerCardDisplay _processingCard;
		private Vector3 _drawPoint;
		private bool _dragging;

		public RunnerCardDisplay ProcessingCard => _processingCard;

		private void Awake()
		{
			_cardTemplate.gameObject.SetActive(false);
		}

		public void Setup(RunnerHand hand, Transform drawPoint, RunnerCardDisplayManager manager)
		{
			_data = hand;
			_manager = manager;
			_drawPoint = drawPoint.position;
			//_data.RegisterListeners(OnCardAdded, OnCardRemoved);
			_cards.Clear();
		}

		private void OnCardAdded(RunnerCardData cardData)
		{
			var card = SpawnCard(cardData);
			card.TurnFaceUp(_animationDuration);
			Resize();
		}
		public void OnCardAdded(RunnerCardDisplay card)
		{
			card.transform.SetParent(_cardHolder);
			card.Register(this);
			_cards.Add(card);
			card.TurnFaceUp(_animationDuration);
			Resize();
		}

		private void OnCardRemoved(RunnerCardData card)
		{
			// logic should probably happen here rather than called AnimateToTrash
			
		}

		public void UnableToPlayCard()
		{
			_processingCard.ReturnToHand();
			_processingCard = null;
		}

		private bool TryGetCardDisplay(RunnerCardData cardData,bool ignoreProcessingCard, out RunnerCardDisplay cardDisplay)
		{
			foreach (var card in _cards)
			{
				if(card == _processingCard)
				{
					continue;
				}
				if (card.Data == cardData)
				{
					cardDisplay = card;
					return true;
				}
			}
			cardDisplay = null;
			return false;
		}

		public bool TryRemoveCard(RunnerCardData cardData, out RunnerCardDisplay card)
		{
			if (!TryGetCardDisplay(cardData, true, out card))
			{
				return false;
			}
			if (card != null)
			{
				card.DeRegister();
				card.UnSelect();
				_cards.Remove(card);
			}
			return true;
		}

		public RunnerCardDisplay RemoveProcessingCard()
		{
			var card = _processingCard;
			if (card != null)
			{
				card.DeRegister();
				card.UnSelect();
				_cards.Remove(_processingCard);
			}
			_processingCard = null;
			_hovered = null;
			Resize();
			return card;

		}

		private RunnerCardDisplay SpawnCard(RunnerCardData cardData)
		{
			var card = Instantiate(_cardTemplate, _cardTemplate.transform.parent);
			card.gameObject.SetActive(true);
			card.Register(this);
			card.Setup(cardData, _drawPoint);
			card.Scale(_cardScaleSize.x, 0f);
			_cards.Add(card);
			return card;
		}

		public void CardHovered(RunnerCardDisplay card)
		{
			if (HasSelection)
			{
				return;
			}
			_hovered = card;
			card.Scale(_cardScaleSize.y, _animationDuration);
			Resize();
		}

		public void CardUnhovered(RunnerCardDisplay card)
		{
			if (HasSelection)
			{
				return;
			}
			if (_hovered == card)
			{
				_hovered = null;
				card.Scale(_cardScaleSize.x, _animationDuration);
				Resize();
			}
		}

		public bool HasSelection => _processingCard != null;

		public bool CardSelected(RunnerCardDisplay card)
		{
			if (HasSelection)
			{
				_processingCard.UnSelect();
				_processingCard = null;
				_manager.OnCardDeselected();
				if (card == _hovered)
				{
					Resize();
					return false;
				}
				CardUnhovered(_hovered);
				CardHovered(card);
			}
			_processingCard = card;
			_manager.OnCardSelected(card);
			return true;
		}

		public void CardDragged(RunnerCardDisplay card)
		{
			//_dragging = true;
			//_processingCard = card;
			//_onDragCallback.Invoke(card.Data);
		}
		public void CardDropped(RunnerCardDisplay card)
		{
			//_dragging = false;
			//_onDropCallback.Invoke(card.Data);
		}

		protected void Resize()
		{
			var n = _cards.Count;
			if (_dragging || n == 0)
			{
				return;
			}
			var minCardSize = _cards[0].BaseWidth * _cardScaleSize.x;
			var maxCardSize = _cards[0].BaseWidth * _cardScaleSize.y;

			var maxHandWidth = _maxHandPosition.position.x - _minHandPosition.position.x;
			// todo: handle rotation and position when hovering with 1 or 2 cards
			switch (n)
			{
				case 0:
					return;
				case 1:
					_cards[0].MovePositionLocal(Vector3.zero, _animationDuration);
					return;
				case 2:
					var twoCardPosition = (maxCardSize - minCardSize + _maxHandSpacing) / 2f;
					_cards[0].MovePositionLocal(new Vector3(-twoCardPosition, 0f, 0f), _animationDuration);
					_cards[1].MovePositionLocal(new Vector3(twoCardPosition, 0f, 0f), _animationDuration);
					return;
					/*
                                case 3:
                                    var threeCardPosition = maxCardSize - minCardSize + (maxCardSize + _maxHandSpacing) / 2f;
                                    _cards[0].TweenPosition(new Vector3(-threeCardPosition, 0f, 0f), _cardPositionDuration);
                                    _cards[1].TweenPosition(Vector3.zero, _cardPositionDuration);
                                    _cards[2].TweenPosition(new Vector3(threeCardPosition, 0f, 0f), _cardPositionDuration);
                                    return; */

			}

			var rotatePerCard = 2 * _maxRotation / n;

			var desiredWidth = _hovered == null ? n * minCardSize + _maxHandSpacing * (n - 1) :
			   maxCardSize + (minCardSize + _maxHandSpacing) * (n - 1);

			var finalWidth = Mathf.Min(maxHandWidth, desiredWidth);
			var handLeftSide = -finalWidth / 2f;

			var spacing = _hovered == null ? (finalWidth - n * minCardSize) / (n - 1) :
				(finalWidth - maxCardSize - 2 * _maxHandSpacing - minCardSize * (n - 1)) / (n - 2);


			var hoveredLocation = n;
			var lastXPosition = 0f;
			var thickness = _cards[0].CardThickness;
			for (var i = 0; i < n; i++)
			{
				var card = _cards[i];
				var width = minCardSize;
				float yPosition;
				if (card == _hovered)
				{
					hoveredLocation = i;
					width = maxCardSize;
					yPosition = _hoverYLocation;

					card.RotateInPlane(0f, _animationDuration);
				} else
				{
					// todo: accumulate yPosition 
					var positionFromMiddle = Mathf.CeilToInt(Mathf.Abs(i - (n - 1) / 2f));
					yPosition = 0f;
					var angle = 0f;
					for (var j = 1; j <= positionFromMiddle; j++)
					{
						angle = j * rotatePerCard;
						yPosition -= width * Mathf.Sin(Mathf.Deg2Rad * angle);

					}
					card.RotateInPlane(-Mathf.Sign(i - (n - 1) / 2f) * angle, _animationDuration);
				}
				var xPosition = lastXPosition;
				if (i == 0)
				{
					xPosition = handLeftSide + width / 2f;
					card.MovePositionLocal(new Vector3(xPosition, yPosition, i * thickness), _animationDuration);
					lastXPosition = xPosition + width / 2f;
					continue;
				}

				if (hoveredLocation == i || hoveredLocation == i - 1)
				{
					xPosition += _maxHandSpacing + width / 2f;
					card.MovePositionLocal(new Vector3(xPosition, yPosition, i * thickness), _animationDuration);
					lastXPosition = xPosition + width / 2f;
					continue;
				}

				xPosition += spacing + width / 2f;
				card.MovePositionLocal(new Vector3(xPosition, yPosition, i * thickness), _animationDuration);
				lastXPosition = xPosition + width / 2f;

			}
		}
	}
}