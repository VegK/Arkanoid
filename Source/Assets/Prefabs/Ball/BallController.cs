using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BallController : MonoBehaviour
{
	#region Properties
	#region Public
	public static BallController Instance
	{
		get
		{
			return _balls.FirstOrDefault(b => b.enabled);
		}
	}

	public Vector2 StartSpeed = new Vector2(170f, 200f);
	#endregion
	#region Private
	private static List<BallController> _balls = new List<BallController>();

	private Vector3 _startPosition;
	private bool _peace = true;
	private Rigidbody _rigidbody;
	private SphereCollider _sphereCollider;

	private bool _catch;
	private Vector3? _saveVelocity;
	private Vector3? _pointContactPlayer;
	private Vector3? _colliderSize;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Разделить шарик на три части. Создаются два дополнительных шарика.
	/// </summary>
	public static void Divide()
	{
		if (Parameters.FixedGame)
			return;

		// Запускаем шарики и сбрасываем прилипание.
		foreach (BallController ball in _balls)
		{
			ball._catch = false;
			if (ball._peace)
				ball.GoBall();
		}

		// Запрет на повторное разделение.
		if (_balls.Count > 1)
			return;

		var firstBall = _balls.FirstOrDefault();
		var rbThis = firstBall.GetComponent<Rigidbody>();
		var pos = firstBall.transform.position;
		var rot = firstBall.transform.rotation;

		var ball1 = Instantiate(firstBall, pos, rot) as BallController;
		ball1._peace = false;
		var newVelocity = rbThis.velocity;
		newVelocity.x += (newVelocity.x > 0) ? 2 : -2;
		var rb = ball1.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.velocity = newVelocity;

		var ball2 = Instantiate(firstBall, pos, rot) as BallController;
		ball2._peace = false;
		newVelocity = rbThis.velocity;
		newVelocity.y += (newVelocity.y > 0) ? 2 : -2;
		rb = ball2.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.velocity = newVelocity;
	}
	/// <summary>
	/// Замедлить все шарики.
	/// </summary>
	public static void Slow()
	{
		if (Parameters.FixedGame)
			return;

		// Запускаем шарики и сбрасываем прилипание.
		foreach (BallController ball in _balls)
		{
			ball._catch = false;
			if (ball._peace)
				ball.GoBall();
		}

		// Замедляем все шарики на N процентов.
		var rateSlow = 20; // процент замедления
		foreach (BallController ball in _balls)
		{
			var velocity = ball._rigidbody.velocity;
			velocity.x -= velocity.x * rateSlow / 100;
			velocity.y -= velocity.y * rateSlow / 100;
			ball._rigidbody.velocity = velocity;
		}
	}
	/// <summary>
	/// При соприкосновении шарика с игроком, шарик перестанет двигатся.
	/// </summary>
	public static void Catch()
	{
		foreach (BallController ball in _balls)
			ball._catch = true;
	}
	/// <summary>
	/// Уничтожить шарик. Возвращает FALSE и не уничтожает шарик, если на сцене остался
	/// последний шарик.
	/// </summary>
	/// <param name="ball">Шарик для уничтожения.</param>
	/// <returns>Результат уничтожения.</returns>
	public static bool DestroyBall(GameObject ball)
	{
		if (_balls.Count == 1)
			return false;
		_balls.Remove(ball.GetComponent<BallController>());
		Destroy(ball);
		return true;
	}
	/// <summary>
	/// Сбросить шарик в начальное состояние.
	/// </summary>
	public void Reset()
	{
		_rigidbody.isKinematic = true;
		transform.position = _startPosition;
		_peace = true;
		_catch = false;
    }
	/// <summary>
	/// Получить размер коллайдера с учётом масштабирования (scale).
	/// </summary>
	/// <returns>Размер.</returns>
	public Vector3 GetColliderSize()
	{
		if (!_colliderSize.HasValue)
            _colliderSize = _sphereCollider.radius * transform.localScale;
		return _colliderSize.Value;
	}
	#endregion
	#region Private
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_sphereCollider = GetComponent<SphereCollider>();

		// Если уже есть шарики на сцене, то берём начальную позицию у них.
		var ball = _balls.FirstOrDefault();
		if (ball != null)
			_startPosition = ball._startPosition;
		else
			_startPosition = transform.position;

		_balls.Add(this);
    }

	private void Start()
	{
		_peace = true;
	}

	private void OnDestroy()
	{
		_balls.Remove(this);
    }

	private void Update()
	{
		if (Parameters.FixedGame)
			return;

		if (_peace && Input.GetKey(KeyCode.Space))
			GoBall();

		if (_peace)
		{
			var pos = transform.position;
			pos.x = PlayerController.Instance.transform.position.x;
			transform.position = pos;
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Player" && _catch && !_pointContactPlayer.HasValue)
		{
			// Не даём шарику прилипнуть, если точка конатка оказалась ниже платформы.
			var plrCtrl = other.gameObject.GetComponent<PlayerController>();
			var plrHeightHalf = plrCtrl.GetColliderSize().y / 2;
            var topPointPlayer = plrCtrl.transform.position.y + plrHeightHalf;
			var downPointBall = transform.position.y - GetColliderSize().y;

			if (downPointBall < topPointPlayer)
				_pointContactPlayer = null;
			else
			{
				_pointContactPlayer = other.contacts[0].point;
				_saveVelocity = _rigidbody.velocity;
				_peace = true;
				_rigidbody.isKinematic = true;
				_rigidbody.isKinematic = false;
			}
		}
	}
	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == "Player" && _pointContactPlayer.HasValue)
		{
			if (_peace)
				transform.position = _pointContactPlayer.Value;
			else
				_pointContactPlayer = null;
        }
	}
	/// <summary>
	/// Запустить шарик.
	/// </summary>
	private void GoBall()
	{
		_peace = false;
		_rigidbody.isKinematic = false;
		if (_saveVelocity.HasValue)
		{
			_rigidbody.velocity = _saveVelocity.Value;
			_saveVelocity = null;
		}
		else
			_rigidbody.AddForce(StartSpeed);
    }
	#endregion
	#endregion
}