using Runner.Target;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerTargetIndicator : MonoBehaviour
{
	[SerializeField] private List<GameObject> _lights = new List<GameObject>();
	[SerializeField] private Material _offMaterial;
	[SerializeField] private Material _redMaterial;
	[SerializeField] private Material _blueMaterial;
	[SerializeField] private Material _greenMaterial;


	private RunnerTargetData _data;
	private List<MeshRenderer> _lightMeshes = new List<MeshRenderer>();

	public void Setup(RunnerTargetData data)
	{
		_data = data;
		_data.RegisterListener(OnStrengthChanged);
		for (var i = 0; i < _lights.Count; i++)
		{
			if(_data.OriginalStrength > i)
			{
				_lights[i].SetActive(true);
				_lightMeshes.Add(_lights[i].transform.GetChild(0).GetComponent<MeshRenderer>());
				continue;
			}
			_lights[i].SetActive(false);
		}
		OnStrengthChanged();
	}

	private void OnStrengthChanged()
	{
		var material = GetTargetColorMaterial();
		for (var i = 0; i < _lightMeshes.Count; i++)
		{
			if (_data.Strength > i)
			{
				_lightMeshes[i].material = material;
				continue;
			}
			_lightMeshes[i].material = _offMaterial;
		}
	}

	private Material GetTargetColorMaterial()
	{
		return _data.Color switch
		{
			RunnerTargetColor.Blue => _blueMaterial,
			RunnerTargetColor.Green => _greenMaterial,
			RunnerTargetColor.Red => _redMaterial,
			_ => _offMaterial,
		};
	}
}
