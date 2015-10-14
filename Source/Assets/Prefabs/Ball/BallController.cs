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
	#endregion
	#endregion

	#region Methods
	#region Public
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
	}
	/// <summary>
	/// Разделить шарик на три части. Создаются два дополнительных шарика.
	/// </summary>
	public void Divide()
	{
		if (Parameters.Instance.FixedGame)
			return;

		if (_peace)
			GoBall();

		var rbThis = GetComponent<Rigidbody>();
		var pos = transform.position;
		var rot = transform.rotation;

        var ball1 = Instantiate(this, pos, rot) as BallController;
		ball1._peace = false;
		var newVelocity = rbThis.velocity;
		newVelocity.x += (newVelocity.x > 0) ? 2 : -2;
		var rb = ball1.GetComponent<Rigidbody>();
		rb.isKinematic = false;
		rb.velocity = newVelocity;

		var ball2 = Instantiate(this, pos, rot) as BallController;
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
	public void Slow()
	{
		if (Parameters.Instance.FixedGame)
			return;

		if (_peace)
			GoBall();

		var rateSlow = 20; // проценты

		foreach (BallController ball in _balls)
		{
			var velocity = ball._rigidbody.velocity;
            velocity.x -= velocity.x * rateSlow / 100;
			velocity.y -= velocity.y * rateSlow / 100;
			ball._rigidbody.velocity = velocity;
        }
	}
	#endregion
	#region Private
	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();

		// Если уже есть шарики на сцене, то берём начальную позицию у них.
		var ball = _balls.FirstOrDefault();
		if (ball != null)
			_startPosition = ball._startPosition;
		else
			_startPosition = transform.position;

		_balls.Add(this);
    }

	private void OnDestroy()
	{
		_balls.Remove(this);
    }

	private void Update()
	{
		if (Parameters.Instance.FixedGame)
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
	/// <summary>
	/// Запустить шарик.
	/// </summary>
	private void GoBall()
	{
		_peace = false;
		_rigidbody.isKinematic = false;
		_rigidbody.AddForce(StartSpeed);
	}
	#endregion
	#endregion
}