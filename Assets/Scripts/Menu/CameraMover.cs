using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{

	[SerializeField] private PositionConfiguration _screenView;
	[SerializeField] private PositionConfiguration _rigView;
	[SerializeField] private CanvasGroup _canvas;
	[SerializeField] private Transform _cameraTransform;

	[System.Serializable]
	public class PositionConfiguration
	{
		public Vector3 MyPosition;
		public Vector3 MyRotation;
		public Vector3 CameraPosition;
		public Vector3 CameraRotation;
		public bool CanvasOn;
	}

	private void Start()
	{
		ShowConfiguration(_screenView, 0f);
	}

	public void ShowRig() => ShowConfiguration(_rigView);

	public void ShowScreen() => ShowConfiguration(_screenView);

	private void ShowConfiguration(PositionConfiguration configuration, float duration = 0.5f)
	{
		transform.DOLocalMove(configuration.MyPosition, duration);
		_cameraTransform.DOLocalMove(configuration.CameraPosition, duration);
		transform.DOLocalRotate(configuration.MyRotation, duration);
		_cameraTransform.DOLocalRotate(configuration.CameraRotation, duration);

		_canvas.interactable = configuration.CanvasOn;
		_canvas.blocksRaycasts = configuration.CanvasOn;
		_canvas.DOFade(configuration.CanvasOn ? 1f : 0f, duration);

	}
}
