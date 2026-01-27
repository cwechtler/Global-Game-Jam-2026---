using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//namespace Realm.Characters
//{
	[CreateAssetMenu(menuName = "Skill/Magic Missile")]
	public class MagicMissileConfig : SkillsConfig
	{

		[Header("Magic Missile Specific")]
		[SerializeField] private float damage = 10f;

		//public override SpecialAbilityBehavior GetAbilityBehavior(GameObject gameObjectToUse)
		//{
			//return gameObjectToUse.AddComponent<MagicMissileBehavior>();
		//}

		public float Damage()
		{
			return damage;
		}
	}
//}
