using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
	#region Properties
	#region Public
	public int HitPoint = 1;
	public int Score = 1;

	public AudioClip AudioBlow;
	#endregion
	#region Private
	private AudioSource _audioSource;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Ball")
		{
			HitPoint--;
			if (HitPoint == 0)
			{
				PlayerController.Instance.Score += Score;
				PlayerController.Instance.DestroyBlock();
				Destroy(gameObject);
			}
			else
			{
				if (_audioSource != null && AudioBlow != null)
					_audioSource.PlayOneShot(AudioBlow);
			}
		}
	}
	#endregion
	#endregion
}