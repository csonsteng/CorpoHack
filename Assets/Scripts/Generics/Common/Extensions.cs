using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.Common
{
	public static class Extensions
	{
		public static void Shuffle<T>(this List<T> list)
		{
			if (list == null)
			{
				return;
			}

			for (var i = list.Count - 1; i > 0; i--)
			{
				var swapIndex = Random.Range(0, i + 1);
				var thisItem = list[i];
				list[i] = list[swapIndex];
				list[swapIndex] = thisItem;
			}
		}

		public static T Pop<T>(this List<T> list)
		{
			if (list == null || list.Count == 0)
			{
				return default;
			}

			var result = list[^1];
			list.RemoveAt(list.Count - 1);
			return result;
		}
	}
}