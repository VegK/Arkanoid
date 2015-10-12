using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{
	#region Properties
	#region Public
	public int HitPoint = 1;
	public int Score = 1;

	public AudioClip AudioBlow;
	public GameObject DestroyedBlock;
	#endregion
	#region Private
	private AudioSource _audioSource;
	private MeshRenderer _meshRenderer;
	private BoxCollider _boxCollider;
    private GameObject _destroydBlock;
	private Rigidbody[] _destroyedBlockPieces;
	#endregion
	#endregion

	#region Methods
	#region Public

	#endregion
	#region Private
	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_meshRenderer = GetComponent<MeshRenderer>();
		_boxCollider = GetComponent<BoxCollider>();
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

				var pos = transform.position;
				var rot = transform.rotation;
				_destroydBlock = Instantiate(DestroyedBlock, pos, rot) as GameObject;

				_destroyedBlockPieces = _destroydBlock.GetComponentsInChildren<Rigidbody>();
				foreach (Rigidbody piece in _destroyedBlockPieces)
				{
					piece.AddExplosionForce(100, other.contacts[0].point, 1f);

					var pieceMesh = piece.GetComponent<MeshRenderer>();
                    var materialCount = pieceMesh.materials.Length;
					var newMaterials = new Material[materialCount];
					for (int i = 0; i < materialCount; i++)
						newMaterials[i] = _meshRenderer.material;
					pieceMesh.materials = newMaterials;
                }
				_meshRenderer.enabled = false;
				_boxCollider.enabled = false;
				StartCoroutine(ExplosionBlock());
			}
			else
			{
				if (_audioSource != null && AudioBlow != null)
					_audioSource.PlayOneShot(AudioBlow);
			}
		}
	}

	private IEnumerator ExplosionBlock()
	{
		var step = 0.1f;
		var alpha = 1f;
		while (alpha > 0)
		{
			foreach (Rigidbody piece in _destroyedBlockPieces)
			{
				var l = piece.GetComponent<MeshRenderer>().materials.Length;
				for (int i = 0; i < l; i++)
				{
					var pieceMesh = piece.GetComponent<MeshRenderer>();
                    var color = pieceMesh.materials[i].color;
					color.a -= step;
					pieceMesh.materials[i].color = color;
				}
			}
			alpha -= step;
			yield return new WaitForSeconds(0.05f);
		}
		Destroy(_destroydBlock);
		Destroy(gameObject);
	}
	#endregion
	#endregion
}