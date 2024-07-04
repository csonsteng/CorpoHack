using LogicPuddle.CardManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runner
{
	public class RunnerHandDisplayController : HandDisplayController
	{
		protected override List<IDraggableCard> _cards => throw new System.NotImplementedException();

		protected override void OnCardDragged(IDraggableCard card)
		{
			throw new System.NotImplementedException();
		}

		protected override void OnCardDropped(IDraggableCard card)
		{
			throw new System.NotImplementedException();
		}
	}
}
