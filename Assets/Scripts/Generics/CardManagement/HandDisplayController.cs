using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public abstract class HandDisplayController : MonoBehaviour
    {
        [SerializeField] protected Vector2 _cardScaleSize;
        [SerializeField] protected float _animationDuration = 0.1f;
        [SerializeField] private float _maxRotation = 20f;
        [SerializeField] private float _hoverYLocation = 1.15f;

        [SerializeField] float _maxHandSpacing = 0.1f;

        [SerializeField] Transform _minHandPosition;
        [SerializeField] Transform _maxHandPosition;

        private IDraggableCard _hovered;
        protected abstract List<IDraggableCard> _cards { get; }


        private bool _dragging;

        public void CardHovered(IDraggableCard card)
        {
            if (_dragging)
            {
                return;
            }
            _hovered = card;
            card.Scale(_cardScaleSize.y, _animationDuration);
            Resize();
        }

        public void CardUnhovered(IDraggableCard card)
        {
            if (_dragging)
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

        public void CardDragged(IDraggableCard card)
        {
            _dragging = true;
            OnCardDragged(card);
        }
        protected abstract void OnCardDragged(IDraggableCard card);
        public void CardDropped(IDraggableCard card)
        {
            _dragging = false;
            OnCardDropped(card);
        }

        protected abstract void OnCardDropped(IDraggableCard card);

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