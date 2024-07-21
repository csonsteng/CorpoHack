using LogicPuddle.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runner.Target
{
	public abstract class AbstractNodeType : UniqueScriptableObject
	{
		[SerializeField] public string Name;
		[SerializeField][TextArea] public string Description;


		public virtual bool CanBeSiphoned => false;
		public virtual bool CanBeDownloaded => false;
		public virtual bool CanBeCorrupted => false;

		public virtual void Siphon()
		{

		}

		public virtual void Download()
		{

		}

		public virtual void Corrupt()
		{

		}
		public virtual void WhenBroken()
		{

		}
	}
}