using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
	public const float MAX_X = 3.672f;

	#region Properties
	#region Public
	public static PlayerController Instance;

	public AudioClip AudioRebound;
	public AudioClip AudioNewLive;
	public AudioClip AudioDie;
	public AudioClip AudioDestroyBlock;

	public Material[] Materials;
	public GameObject[] BaseObjects;

	public int DestroyBlocksToWin = 15;
	public float FactorMove = 0.3f;
	/// <summary>
	/// Очки.
	/// </summary>
	public int Score
	{
		get
		{
			return _score;
		}
		set
		{
			_score = value;
			InterfaceController.Instance.SetScore(_score);
		}
	}
	/// <summary>
	/// Оставшееся количество жизней.
	/// </summary>
	public int Life
	{
		get
		{
			return _life;
		}
		set
		{
			if (value < 0)
			{
				_gameOver = true;
				DestroyPlayer();
				return;
            }

			if (value > _life)
				NewLive();
			else if (value < _life)
				DestroyPlayer();

			_life = value;
			InterfaceController.Instance.SetLife(_life);
		}
	}
	#endregion
	#region Private
	private bool _gameOver;
	private int _destroyedBlocks;
	private int _score;
	[SerializeField]
	private int _life = 3;
	private AudioSource _audioSource;

	private Vector3 _startPosition;
	private Dictionary<int, Vector3> _piecesStartPosition;
	private Rigidbody[] _piecesRigidbody;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Уменьшаем количество блоков и проверяем конец игры.
	/// </summary>
	public void DestroyBlock()
	{
		_destroyedBlocks++;

		if (_audioSource != null && AudioDestroyBlock != null)
			_audioSource.PlayOneShot(AudioDestroyBlock);

		if (_destroyedBlocks >= DestroyBlocksToWin)
			GameOver();
	}
	/// <summary>
	/// Дополнительная жизнь.
	/// </summary>
	public void NewLive()
	{
		if (_audioSource != null && AudioNewLive != null)
			_audioSource.PlayOneShot(AudioNewLive);
	}
	/// <summary>
	/// Уничтожить игрока.
	/// </summary>
	public void DestroyPlayer()
	{
		if (_audioSource != null && AudioDie != null)
			_audioSource.PlayOneShot(AudioDie);

		foreach (GameObject obj in BaseObjects)
			obj.SetActive(false);

		foreach (Rigidbody piece in _piecesRigidbody)
		{
			piece.gameObject.SetActive(true);
			piece.AddExplosionForce(100, transform.position, 1f);
		}
		GetComponent<BoxCollider>().enabled = false;
		StartCoroutine(HidePieces());
	}
	#endregion
	#region Private
	private void Awake()
	{
		Instance = this;
		_audioSource = GetComponent<AudioSource>();

		_startPosition = transform.position;
		_piecesStartPosition = new Dictionary<int, Vector3>();
		_piecesRigidbody = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody pieces in _piecesRigidbody)
		{
			var pos = pieces.gameObject.transform.position;
            _piecesStartPosition.Add(pieces.GetInstanceID(), pos);
			pieces.gameObject.SetActive(false);
		}

		foreach (Material m in Materials)
		{
			var color = m.color;
			color.a = 1f;
			m.color = color;
		}
	}

	private void Start()
	{
		InterfaceController.Instance.SetScore(Score);
		InterfaceController.Instance.SetLife(Life);
	}

	private void Update()
	{
		if (Parameters.Instance.FixedGame)
			return;

		var pos = transform.position;
		pos.x += Input.GetAxis("Horizontal") * FactorMove;
		if (Mathf.Abs(pos.x) > MAX_X)
			pos.x = ((pos.x < 0) ? -1 : 1) * MAX_X;
		transform.position = pos;
	}

	private void OnApplicationQuit()
	{
#if (UNITY_EDITOR)
		foreach (Material m in Materials)
		{
			var color = m.color;
			color.a = 1f;
			m.color = color;
		}
#endif
	}

    private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Ball")
		{
			// Отражаем шарик в обратном направлении.
			var reflection = false;
			var pointContact = other.contacts[0].point;
			var velocity = other.rigidbody.velocity;
			var pos = other.gameObject.transform.position;

			if (pointContact.x > transform.position.x)
			{
				if (velocity.x < 0)
				{
					velocity.x *= -1;
					pos.x += (pointContact.x - pos.x) * 2;
					reflection = true;
				}
			}
			else if (pointContact.x < transform.position.x)
			{
				if (velocity.x > 0)
				{
					velocity.x *= -1;
					pos.x -= Mathf.Abs(pointContact.x - pos.x) * 2;
					reflection = true;
				}
			}

			if (reflection)
			{
				other.gameObject.transform.position = pos;
				other.rigidbody.velocity = velocity;
			}
		}
		else if (other.gameObject.tag == "Bonus")
		{
			var ctrl = other.gameObject.GetComponent<BonusController>();
			switch (ctrl.Bonus)
			{
				case BonusType.Divide:
					BallController.Instance.Divide();
					break;
				case BonusType.Slow:
					BallController.Instance.Slow();
					break;
			}
			Destroy(other.gameObject);
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == "Ball")
		{
			if (_audioSource != null && AudioRebound != null)
				_audioSource.PlayOneShot(AudioRebound);
		}
	}

	private void Reset()
	{
		transform.position = _startPosition;
		foreach (GameObject obj in BaseObjects)
			obj.SetActive(true);

		foreach (Rigidbody pieces in _piecesRigidbody)
		{
			pieces.isKinematic = true;
			pieces.isKinematic = false;
			var id = pieces.GetInstanceID();
			if (_piecesStartPosition.ContainsKey(id))
				pieces.transform.position = _piecesStartPosition[id];
			pieces.gameObject.SetActive(false);
		}

		foreach (Material m in Materials)
		{
			var color = m.color;
			color.a = 1f;
			m.color = color;
		}
	}
	/// <summary>
	/// Остновить игру и вывести на экран панель GameOver.
	/// </summary>
	private void GameOver()
	{
		InterfaceController.Instance.GameOver(_score);
    }

	private IEnumerator HidePieces()
	{
		Parameters.Instance.FixedGame = true;
		var step = 0.05f;
		var alpha = 1f;
		while (alpha > 0)
		{
            foreach (Material m in Materials)
			{
				var color = m.color;
				color.a -= step;
				m.color = color;
			}
			alpha -= step;
			yield return new WaitForSeconds(0.05f);
		}

		if (_gameOver)
			GameOver();
		else
		{
			Reset();
			BallController.Instance.Reset();
		}

		// Очищаем сцену от бонусов.
		var bonuses = FindObjectsOfType<BonusController>();
		foreach (BonusController bonus in bonuses)
			Destroy(bonus.gameObject);

		GetComponent<BoxCollider>().enabled = true;
		Parameters.Instance.FixedGame = false;
	}
	#endregion
	#endregion
}