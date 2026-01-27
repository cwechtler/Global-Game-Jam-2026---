using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace Realm.Characters
//{
	public abstract class SkillsConfig : ScriptableObject
	{

		[Header("Special Ability General")]
		[TextArea]
		[SerializeField] string skillDesctiption;
		[SerializeField] float manaCost = 10f;
		[SerializeField] float skillCoolDown = .01f;
		[Space]
		[SerializeField] GameObject particalPrefab;
		[SerializeField] Sprite skillSprite;
		[Space]
		[SerializeField] float volume = 1f;
		[SerializeField] AudioClip[] audioClips;
		[Space]
		[SerializeField] AnimationClip animationClip;
		[SerializeField] float particleWaitTime = 0;

		//protected SpecialAbilityBehavior behavior;

		public string SkillDescription
		{
			get { return skillDesctiption; }
		}

		//public abstract SpecialAbilityBehavior GetAbilityBehavior(GameObject gameObjectToUse);

		public void AddComponent(GameObject gameObjectToUse)
		{
			//SpecialAbilityBehavior behaviorComponent = GetAbilityBehavior(gameObjectToUse);
			//behaviorComponent.SetConfig(this);
			//behavior = behaviorComponent;
		}

		public void Use(GameObject target)
		{
			//behavior.Use(target);
		}

		public float GetManaCost()
		{
			return manaCost;
		}

		public float GetSkillCoolDown()
		{
			return skillCoolDown;
		}

		public GameObject GetParticlePrefab()
		{
			return particalPrefab;
		}

		public Sprite GetSkillSprite()
		{
			return skillSprite;
		}

		public AudioClip GetRandomAudioClip()
		{
			return audioClips[Random.Range(0, audioClips.Length)];
		}

		public float Volume()
		{
			return volume;
		}

		public AnimationClip GetAnimationClip()
		{
			RemoveAnimationEvents();
			return animationClip;
		}

		public float GetParticleWaitTime()
		{
			return particleWaitTime;
		}

		//Removes Events from asset packs to prevent crashes.
		private void RemoveAnimationEvents()
		{
			animationClip.events = new AnimationEvent[0];
		}
	}
//}
