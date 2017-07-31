using System.Collections;
using UnityEngine;

public class LaserController : MonoBehaviour
{
	public GameObject source;
	public GameObject player;
	public GameObject Explosion;
	public Color color;
	public float startWidth = 0.05f;
	public float shootWidth = 1.5f;
	public float timeToLock = 2f;
	public float timeToShoot = 1f;
	public int Damage = 900;
	
	private GameObject spriteObject;
	private float size;
	private readonly Vector3 rayExtends = new Vector3(0.05f, 10, 0.05f);
	private float time;
	private bool locked;
	private Vector3 lockedPosition;
	private Collider[] colliderBuffer = new Collider[5];

	private void Start()
	{
		var renderer = GetComponentInChildren<SpriteRenderer>();
		size = renderer.bounds.extents.y;
		renderer.color = color;
		spriteObject = renderer.gameObject;
		locked = true;
		
		
		var barrelPos = source.transform.position;
		transform.position = new Vector3(barrelPos.x, transform.position.y, barrelPos.z);
		var pos = transform.position;
		var playerPos = player.transform.position;
		lockedPosition = player.transform.position;
		playerPos.y = pos.y;
		var dir = playerPos - pos;
		var distance = dir.magnitude;
		RaycastHit hit;
		if (Physics.BoxCast(pos, rayExtends, dir, out hit, transform.rotation, distance, LayerMask.GetMask("Asteroids")))
		{
			lockedPosition = hit.collider.gameObject.transform.position;
			var colliderPos = hit.collider.gameObject.transform.position;
			colliderPos.y = pos.y;
			distance = (colliderPos - pos).magnitude;
		}
		
		spriteObject.transform.localScale = new Vector3(startWidth, distance / size * 2, 1);
		transform.LookAt(playerPos);
		
		Invoke(nameof(DoShoot), timeToShoot);
	}

	private void Update()
	{
		time += Time.deltaTime;
		var y = spriteObject.transform.localScale.y;
		spriteObject.transform.localScale = new Vector3(Mathf.Lerp(startWidth, shootWidth, time / timeToShoot), y, 1);
	}

	public bool IsLocked()
	{
		return locked;
	}

	private void LockLaser()
	{
		Debug.Log("Locked");
		Invoke(nameof(DoShoot), timeToShoot);
		locked = true;
	}

	private void DoShoot()
	{
		var explosionSize = 3f;
		Debug.Log("Shot");
		var expl = Instantiate(Explosion, GameObject.Find("Explosions").transform);
		expl.transform.position = lockedPosition;
		expl.transform.localScale *= explosionSize;
		expl.GetComponent<Explosion>().explode();
		var lower = new Vector3(lockedPosition.x, lockedPosition.y - 10, lockedPosition.z);
		var upper = new Vector3(lockedPosition.x, lockedPosition.y + 10, lockedPosition.z);
		var hits = Physics.OverlapCapsuleNonAlloc(lower, upper, explosionSize, colliderBuffer, LayerMask.GetMask("Player", "Asteroids"));
		if (hits > 0)
		{
			for (var i = 0; i < hits; i++)
			{
				var destroyable = colliderBuffer[0].GetComponent<Destroyable>();
				if (destroyable)
				{
					destroyable.Hit(Damage);
				}
			}
		}
		Destroy(gameObject);
	}
}
