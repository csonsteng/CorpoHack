using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Threading.Tasks;

public class Hand : MonoBehaviour
{
    [SerializeField] private int _handSize;
    [SerializeField] private Card _cardTemplate;
    [SerializeField] private Vector2 _cardScaleSize;
    [SerializeField] private float _animationDuration = 0.1f;
    [SerializeField] private float _maxRotation = 20f;
    [SerializeField] private float _hoverYLocation = 1.15f;

    [SerializeField] float _maxHandSpacing = 0.1f;

    [SerializeField] Transform _minHandPosition;
    [SerializeField] Transform _maxHandPosition;
    [SerializeField] Transform _deckLocation;

    private Card _hovered;
    private List<Card> _cards = new();

    private float _maxHandWidth;

    private void Start()
    {
        _cardTemplate.gameObject.SetActive(false);
        _maxHandWidth = _maxHandPosition.position.x - _minHandPosition.position.x;
        SpawnHand();
    }

    [Button]
    private async void SpawnHand()
	{
        CleanUp();
        for (var i = 0; i < _handSize; i++)
        {
            AddCard();
            await Task.Delay(Mathf.RoundToInt(_animationDuration * 1000));
        }
        Resize();
    }

    [Button]
    public void AddCard()
	{
        var card = Instantiate(_cardTemplate, transform);
        card.gameObject.SetActive(true);
        card.transform.localPosition = _deckLocation.localPosition;
        card.Register(CardHovered, CardUnhovered);
        card.TweenScale(_cardScaleSize.x, 0f);
        _cards.Add(card);
        Resize();
    }

    private void CleanUp()
	{
        foreach(var card in _cards)
		{
            Destroy(card.gameObject);
		}
        _cards.Clear();
        _hovered = null;
	}

    private void CardHovered(Card card)
	{
        _hovered = card;
        card.TweenScale(_cardScaleSize.y, _animationDuration);
        Resize();
	}

    private void CardUnhovered(Card card)
    {
        if(_hovered == card)
		{
            _hovered = null;
            card.TweenScale(_cardScaleSize.x, _animationDuration);
            Resize();
        }
    }

    private void Resize()
	{

        var n = _cards.Count;
        var minCardSize = _cardTemplate.BaseWidth * _cardScaleSize.x;
        var maxCardSize = _cardTemplate.BaseWidth * _cardScaleSize.y;
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
                var angle = (((n - 1) / 2f) - i) * rotatePerCard;
                yPosition = -width * Mathf.Sin(Mathf.Deg2Rad * Mathf.Abs(angle));
                card.TweenRotation(angle, _animationDuration);
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
