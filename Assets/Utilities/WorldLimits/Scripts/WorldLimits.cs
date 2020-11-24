using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class WorldLimits : MonoBehaviour
{
	public bool colliderLiveUpdate;
	public Bounds bounds;

	private EdgeCollider2D edgeCollider;

	private void Start()
	{
		edgeCollider = GetComponent<EdgeCollider2D>();
		edgeCollider.points = new[]
		{
			new Vector2(bounds.min.x, bounds.min.y), new Vector2(bounds.min.x, bounds.max.y),
			new Vector2(bounds.max.x, bounds.max.y), new Vector2(bounds.max.x, bounds.min.y),
			new Vector2(bounds.min.x, bounds.min.y)
		};
	}

	private void Update()
	{
		if (colliderLiveUpdate)
		{
			edgeCollider.points = new[]
			{
				new Vector2(bounds.min.x, bounds.min.y), new Vector2(bounds.min.x, bounds.max.y),
				new Vector2(bounds.max.x, bounds.max.y), new Vector2(bounds.max.x, bounds.min.y),
				new Vector2(bounds.min.x, bounds.min.y)
			};
		}
	}

	public Vector3 GetPositionCorrectionOffset(Bounds other)
	{
		float horizontalAdjust = 0.0f;
		if (other.min.x < bounds.min.x)
		{
			horizontalAdjust = other.min.x - bounds.min.x;
		}

		if (other.max.x > bounds.max.x)
		{
			horizontalAdjust = other.max.x - bounds.max.x;
		}

		float verticalAdjust = 0.0f;
		if (other.min.y < bounds.min.y)
		{
			verticalAdjust = other.min.y - bounds.min.y;
		}

		if (other.max.y > bounds.max.y)
		{
			verticalAdjust = other.max.y - bounds.max.y;
		}

		return new Vector3(horizontalAdjust, verticalAdjust, 0.0f);
	}

    private void OnDrawGizmosSelected()
    {
		Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
