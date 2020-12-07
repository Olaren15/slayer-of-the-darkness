using System.Collections;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
	private SpriteRenderer swordSpriteRenderer;
	private Rigidbody2D swordRigidBody;

	private Material effectMaterial;
	private static readonly int TransparencyProperty = Shader.PropertyToID("Transparency_");

	private void Start()
	{
		swordSpriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
		swordRigidBody = transform.GetComponentInChildren<Rigidbody2D>();
		effectMaterial = transform.GetComponentInChildren<MeshRenderer>().material;

		StartCoroutine(DoEffect());
	}

	private IEnumerator DoEffect()
	{
		// fade in
		for (float transparency = 1.0f; transparency > 0.0f; transparency -= 1.0f * Time.deltaTime)
		{
			effectMaterial.SetFloat(TransparencyProperty, transparency);
			swordSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1 - transparency);
			yield return null;
		}

		yield return new WaitForSeconds(0.2f);

		// drop sword
		swordRigidBody.bodyType = RigidbodyType2D.Dynamic;
		swordRigidBody.AddForce(new Vector2(0.0f, -40.0f), ForceMode2D.Impulse);
		
		yield return new WaitForSeconds(1.0f);
		
		// fade out
		for (float transparency = 0.0f; transparency < 1.0f; transparency += 1.0f * Time.deltaTime)
		{
			effectMaterial.SetFloat(TransparencyProperty, transparency);
			swordSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1 - transparency);
			yield return null;
		}

		Destroy(gameObject);
	}
}
