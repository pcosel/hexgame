using System;
using System.Collections.Generic;

using UnityEngine;


namespace Graphics
{
	public class GraphicsRegistry : MonoBehaviour
	{
		Dictionary<UnitSprite, Sprite> unitSprites = new Dictionary<UnitSprite, Sprite>();

		Dictionary<ActionSprite, Sprite> actionSprites = new Dictionary<ActionSprite, Sprite>();

		Sprite error;

		void Awake() {
			unitSprites.Add (UnitSprite.FIGHTER, Resources.Load<Sprite> ("fighter"));
			unitSprites.Add (UnitSprite.ROGUE, Resources.Load<Sprite> ("rogue"));
			unitSprites.Add (UnitSprite.MAGE, Resources.Load<Sprite> ("mage"));
			unitSprites.Add (UnitSprite.ASSASSIN, Resources.Load<Sprite> ("assassin"));
			error = Resources.Load<Sprite> ("error");
		}

		public Sprite getUnitSprite(UnitSprite unitSprite_)
		{
			if (unitSprites.ContainsKey (unitSprite_)) {
				return unitSprites[unitSprite_];
			} else {
				return error;
			}
		}

		public Sprite getActionSprite(ActionSprite actionSprite_)
		{
			if (actionSprites.ContainsKey (actionSprite_)) {
				return actionSprites[actionSprite_];
			} else {
				return error;
			}
		}
	}
}

