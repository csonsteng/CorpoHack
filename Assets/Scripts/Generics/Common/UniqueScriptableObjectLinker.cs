using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LogicPuddle.Common {
    public class UniqueScriptableObjectLinker : MonoBehaviour
    {
        [SerializeField] private List<IUniqueScriptableObjectList> _lists = new List<IUniqueScriptableObjectList>();

		private Dictionary<System.Type, IUniqueScriptableObjectList> _listDictionary;

		private static UniqueScriptableObjectLinker _instance;

		public static bool TryGetUniqueObject<T>(string id, out T foundObject) where T: UniqueScriptableObject
		{
			if(_instance == null)
			{
				_instance = FindObjectOfType<UniqueScriptableObjectLinker>();
			}
			if (!_instance._listDictionary.TryGetValue(typeof(T), out var list))
			{
				foundObject = default;
				return false;
			}
			return list.TryGetUniqueObject<T>(id, out foundObject);
		}

		private void Awake()
		{
			DontDestroyOnLoad(this.gameObject);

			foreach(var list in _lists)
			{
				list.BuildDictionary();
				_listDictionary.Add(list.GetTargetType(), list);
			}
		}
	}
}