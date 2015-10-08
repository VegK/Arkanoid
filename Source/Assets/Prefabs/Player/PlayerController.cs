using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
				GameOver();

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
	private int _countBlock;
	private int _score;
	[SerializeField]
	private int _life = 3;
	private AudioSource _audioSource;
	#endregion
	#endregion

	#region Methods
	#region Public
	/// <summary>
	/// Уменьшаем количество блоков и проверяем конец игры.
	/// </summary>
	public void DestroyBlock()
	{
		_countBlock--;

		if (_audioSource != null && AudioDestroyBlock != null)
			_audioSource.PlayOneShot(AudioDestroyBlock);

		if (_countBlock <= 0)
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
		// TODO: анимация уничтожения игрока
		if (_audioSource != null && AudioDie != null)
			_audioSource.PlayOneShot(AudioDie);
	}
	#endregion
	#region Private
	private void Awake()
	{
		Instance = this;
		_audioSource = GetComponent<AudioSource>();

		_countBlock = GameObject.FindGameObjectsWithTag("Block").Length;
		if (_countBlock <= 0)
			GameOver();
    }

	private void Start()
	{
		InterfaceController.Instance.SetScore(Score);
		InterfaceController.Instance.SetLife(Life);
	}

	private void Update()
	{
		var pos = transform.position;
		pos.x += Input.GetAxis("Horizontal") * FactorMove;
		if (Mathf.Abs(pos.x) > MAX_X)
			pos.x = ((pos.x < 0) ? -1 : 1) * MAX_X;
		transform.position = pos;
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == "Ball")
		{
			if (_audioSource != null && AudioRebound != null)
				_audioSource.PlayOneShot(AudioRebound);
		}
	}
	/// <summary>
	/// Остновить игру и вывести на экран панель GameOver.
	/// </summary>
	private void GameOver()
	{
		InterfaceController.Instance.GameOver(_score);
    }
	#endregion
	#endregion
}