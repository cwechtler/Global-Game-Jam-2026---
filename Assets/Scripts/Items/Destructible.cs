using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MyBox;

public enum DestructibleState 
{ 
	Fixed,
	Cracked,
	Broken
}

public class Destructible : MonoBehaviour
{
	[SerializeField] private skillElementType skillRequiredToDestroy;
	[Space]
	[Tooltip("Check box to select a prefab item to be dropped on destroy.")]
	[SerializeField] private bool dropItem = true;
	//[ConditionalField("dropItem")]
	[ConditionalHide("dropItem", true)]
	[SerializeField] private GameObject prefabToDrop;
	[Space]
	[SerializeField] private AudioClip[] sfxClips = new AudioClip[1];
	//[Separator("Destructable State Options", true)]
	[Tooltip("Leave Unchecked to destroy Item immediately on hit.")]
	[SerializeField] private bool hasDestructableStates = false;
	//[ConditionalField(nameof(hasDestructableStates))]
	[ConditionalHide("hasDestructableStates", true)]
	[SerializeField] private DestructibleState state = DestructibleState.Fixed;
	[Tooltip("Fixed to broken order.")]
	//[ConditionalField(nameof(hasDestructableStates))]
	//public CollectionWrapper<Sprite> destructibleSpriteStates;
	[ConditionalHide("hasDestructableStates", true)]
	[SerializeField] private Sprite[] destructibleSpriteStates = new Sprite[0];

	private bool dropped;
	private SpriteRenderer spriteRenderer;
	private CapsuleCollider2D capsuleCollider2D;

	private void Start()
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Skill") && !dropped) {
			SkillConfig skill = collision.GetComponentInParent<SkillConfig>();
			if (skill.SkillElementType == skillRequiredToDestroy) {
				float skillDuration = skill.CoolDownTime;		
				StartCoroutine(UpdateSprite(skillDuration));		
			}
		}
	}

	private void OnParticleCollision(GameObject particle)
	{
		SkillConfig particleParent = particle.GetComponentInParent<SkillConfig>();
		if (particleParent.SkillElementType == skillRequiredToDestroy && !dropped) {
			float skillDuration = particleParent.CoolDownTime;
			StartCoroutine(UpdateSprite(skillDuration));
		}
	}

	private IEnumerator UpdateSprite(float time)
	{
		dropped = true;
		int randomClip = 0;
		if (sfxClips.Length > 0) {
			randomClip = Random.Range(0, sfxClips.Length - 1);
		}
		SoundManager.instance.PlayDestructibleSound(sfxClips[randomClip]);

		if (hasDestructableStates) {
			switch (state) {
				case DestructibleState.Fixed:
					//spriteRenderer.sprite = destructibleSpriteStates.Value[0];
					spriteRenderer.sprite = destructibleSpriteStates[0];
					state = DestructibleState.Cracked;
					break;
				case DestructibleState.Cracked:
					//spriteRenderer.sprite = destructibleSpriteStates.Value[1];
					spriteRenderer.sprite = destructibleSpriteStates[1];
					state = DestructibleState.Broken;
					capsuleCollider2D.enabled = false;
					if (dropItem) {
						GameObject itemDrop = Instantiate(prefabToDrop, transform.position, Quaternion.identity);
					}
					break;
				case DestructibleState.Broken:
					break;
				default:
					break;
			}

			yield return new WaitForSeconds(time);

			if (state != DestructibleState.Broken) {
				dropped = false;
			}
		} else {
			if (dropItem) {
				GameObject itemDrop = Instantiate(prefabToDrop, transform.position, Quaternion.identity);		
			}
			state = DestructibleState.Broken;
			Destroy(gameObject);
			yield return null;
		}
	}
}
