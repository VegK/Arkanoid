using UnityEngine;
using System.Collections;

public class TiltCamera : MonoBehaviour
{
	#region Properties
	#region Public
	public GameObject Target;

	public Vector3 MaxCameraPosition = new Vector3(2.24f, 1f, 9.56f);
	public Vector3 MaxCameraRotation = new Vector3(0f, 5f, 0f);
	#endregion
	#region Private
	private Camera _camera;
	private Vector3 _startCameraPosition;
	private Vector3 _startCameraRotation;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		_camera = GetComponent<Camera>();
		if (_camera == null)
		{
			enabled = false;
			return;
		}
		_startCameraPosition = new Vector3(
			Mathf.Abs(_camera.transform.position.x),
			Mathf.Abs(_camera.transform.position.y),
			Mathf.Abs(_camera.transform.position.z));
		_startCameraRotation = _camera.transform.rotation.eulerAngles;
	}

	private void Update()
	{
		if (_camera == null)
			return;

		var t = Target.transform.position.x / PlayerController.MAX_X;
		var camPos = Vector3.Lerp(_startCameraPosition, MaxCameraPosition, Mathf.Abs(t));
		var camRot = Vector3.Lerp(_startCameraRotation, MaxCameraRotation, Mathf.Abs(t));

		if (t < 0)
			camRot.y *= -1;
		else
			camPos.x *= -1;
		camPos.z *= -1;


		_camera.transform.position = camPos;

		var qua = new Quaternion();
		qua.eulerAngles = camRot;
		Camera.main.transform.rotation = qua;
	}
	#endregion
	#endregion
}