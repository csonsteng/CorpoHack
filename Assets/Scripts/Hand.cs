using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Hand : MonoBehaviour
{
    [SerializeField] private int _handSize;
    [SerializeField] private Card _cardTemplate;
    [SerializeField] private Vector2 _cardScaleSize;
    [SerializeField] private float _cardScaleDuration;

    private Card _hovered;
    private List<Card> _cards = new();

    private void Start()
    {
        _cardTemplate.gameObject.SetActive(false);
        SpawnHand();
    }

    [Button]
    private void SpawnHand()
	{
        CleanUp();
        for (var i = 0; i < _handSize; i++)
        {
            var card = Instantiate(_cardTemplate, transform);
            card.gameObject.SetActive(true);
            card.Register(CardHovered, CardUnhovered);
            card.TweenScale(_cardScaleSize.x, 0f);
            _cards.Add(card);
        }
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
        card.TweenScale(_cardScaleSize.y, _cardScaleDuration);
        Resize();
	}

    private void CardUnhovered(Card card)
    {
        if(_hovered == card)
		{
            _hovered = null;
            card.TweenScale(_cardScaleSize.x, _cardScaleDuration);
            Resize();
        }
    }

    private void Resize()
	{
        var afterHovered = false;

        foreach(var card in _cards)
		{
            if(_hovered == card)
			{

                afterHovered = true;
                continue;
			}
			if (afterHovered)
			{

			}
		}
	}
    
}
