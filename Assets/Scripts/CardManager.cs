using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] private int _handSize = 5;
    public Hand Hand;
    public Deck Deck;
    public Trash Trash;

    [SerializeField] private List<Target> _targets;

    void Start()
    {
        Hand.SetManager(this);
        Deck.SetManager(this);
        Trash.SetManager(this);
        foreach (var target in _targets)
		{
            target.SetManager(this);
		}
        FillHand();
    }

    private async void FillHand()
    {
        for (var i = Hand.Count; i < _handSize; i++)
        {
            await Hand.AddCard();
        }
    }

    public void OnCardDragged(Card card)
	{
        foreach (var target in _targets)
		{
            target.OnCardDragged(card);
		}
	}

    /// <summary>
    /// Returns true if valid target
    /// </summary>
    public bool OnCardDropped()
    {
        var foundTarget = false;
        // this approach will definitely bug out if targets are overlapped
        foreach (var target in _targets)
        {
			if (target.OnCardDropped())
			{
                foundTarget = true;
			}
        }
        return foundTarget;
    }
}
