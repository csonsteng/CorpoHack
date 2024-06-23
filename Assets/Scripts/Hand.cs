using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Threading.Tasks;

public class Hand : AbstractCardManagerComponent
{
    [SerializeField] private Card _cardTemplate;
    [SerializeField] private Vector2 _cardScaleSize;
    [SerializeField] private float _animationDuration = 0.1f;
    [SerializeField] private float _maxRotation = 20f;
    [SerializeField] private float _hoverYLocation = 1.15f;

    [SerializeField] float _maxHandSpacing = 0.1f;

    [SerializeField] Transform _minHandPosition;
    [SerializeField] Transform _maxHandPosition;

    private Card _hovered;
    private List<Card> _cards = new();

    private float _maxHandWidth;

    private bool _dragging;

    public int Count => _cards.Count;

    private void Start()
    {
        _cardTemplate.gameObject.SetActive(false);
        _maxHandWidth = _maxHandPosition.position.x - _minHandPosition.position.x;
    }

    [Button]
    public async Task AddCard()
	{
        var card = Instantiate(_cardTemplate, transform);
        card.gameObject.SetActive(true);
        card.transform.position = _manager.Deck.transform.position;
        card.Register(this);
        card.TweenScale(_cardScaleSize.x, 0f);
        _cards.Add(card);
        Resize();

        await Task.Delay(Mathf.RoundToInt(_animationDuration * 1000));
    }

    public void CardHovered(Card card)
	{
        if (_dragging)
        {
            return;
        }
        _hovered = card;
        card.TweenScale(_cardScaleSize.y, _animationDuration);
        Resize();
	}

    public void CardUnhovered(Card card)
    {
        if (_dragging)
        {
            return;
        }
        if (_hovered == card)
		{
            _hovered = null;
            card.TweenScale(_cardScaleSize.x, _animationDuration);
            Resize();
        }
    }

    public void CardDragged(Card card)
	{
        _dragging = true;
        _manager.OnCardDragged(card);
	}
    public void CardDropped(Card card)
    {
        _dragging = false;
		if (!_manager.OnCardDropped())
		{
            card.ReturnToHand();
            return;
		}

        Play(card);
    }

    public void Play(Card card)
	{
        card.transform.DOMove(_manager.Trash.transform.position, _animationDuration);
        _cards.Remove(card);
        Resize();
	}

    private void Resize()
	{
		if (_dragging)
		{
            return;
		}
        var n = _cards.Count;
        var minCardSize = _cardTemplate.BaseWidth * _cardScaleSize.x;
        var maxCardSize = _cardTemplate.BaseWidth * _cardScaleSize.y;

        // todo: handle rotation and position when hovering with 1 or 2 cards
        switch (n)
		{
            case 0: 
                return;
            case 1:
                _cards[0].TweenPosition(Vector3.zero, _animationDuration);
                return;
            case 2:
                var twoCardPosition = (maxCardSize - minCardSize + _maxHandSpacing) / 2f;
                _cards[0].TweenPosition(new Vector3(-twoCardPosition, 0f, 0f), _animationDuration);
                _cards[1].TweenPosition(new Vector3(twoCardPosition, 0f, 0f), _animationDuration);
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

        var finalWidth = Mathf.Min(_maxHandWidth, desiredWidth);
        var handLeftSide = -finalWidth / 2f;

        var spacing = _hovered == null ? (finalWidth - n * minCardSize) / (n - 1) :
            (finalWidth - maxCardSize - 2 * _maxHandSpacing - minCardSize * (n - 1)) / (n - 2);


        var hoveredLocation = n;
        var lastXPosition = 0f;
        var thickness = _cardTemplate.CardThickness;
        
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

                card.TweenRotation(0f, _animationDuration);
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
                card.TweenRotation(-Mathf.Sign(i - (n - 1) / 2f) * angle, _animationDuration);
            }
            var xPosition = lastXPosition;
            if(i == 0)
			{
                xPosition = handLeftSide + width / 2f;
                card.TweenPosition(new Vector3(xPosition, yPosition, i * thickness), _animationDuration);
                lastXPosition = xPosition + width / 2f;
                continue;
			}

            if(hoveredLocation == i || hoveredLocation == i - 1)
			{
                xPosition += _maxHandSpacing + width / 2f;
                card.TweenPosition(new Vector3(xPosition, yPosition, i * thickness), _animationDuration);
                lastXPosition = xPosition + width / 2f;
                continue;
            }

            xPosition += spacing + width / 2f;
            card.TweenPosition(new Vector3(xPosition, yPosition, i * thickness), _animationDuration);
            lastXPosition = xPosition + width / 2f;

        }
	}
    
}
