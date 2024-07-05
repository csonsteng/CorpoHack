using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.CardManagement
{
    public interface ICardManager
    {
		public bool DrawCard();
		public void ShuffleTrashIntoDeck();
		public void ShuffleHandIntoDeck();
		public void ShuffleAll();
		public void Reset();

	}
}