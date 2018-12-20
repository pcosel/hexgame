using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexlibrary;
using GameLogic.Units;
using GameLogic.Units.Heroes;
using UnityEngine.UI;

namespace Graphics
{
    public class SpriteGraphicsHandler : MonoBehaviour
    {

        public GraphicsRegistry graphicsRegistry;

        public HexGrid hexGrid;

		public GameObject healthAndArmorPrefab;

        List<GameObject> displayedUnits = new List<GameObject>();

		List<GameObject> healthAndArmorBars = new List<GameObject>();

        /// <summary>
        /// Removes all the previously displayed units and displayes the new ones.
        /// </summary>
        /// <param name="units">The units and respective positions to be displayed.</param>
        public void displayUnits(BidirectionalDictionary<Hex, GenericUnit> units)
        {
			foreach (GameObject displayedUnit in displayedUnits)
			{
				Destroy(displayedUnit);
			}

			foreach (GameObject healthAndArmorBar in healthAndArmorBars)
			{
				Destroy(healthAndArmorBar);
			}

            foreach (KeyValuePair<Hex, GenericUnit> entry in units)
            {
                GameObject newUnit = new GameObject(entry.Value.Name);
                SpriteRenderer spriteRenderer = newUnit.AddComponent<SpriteRenderer>();

                spriteRenderer.sprite = graphicsRegistry.getUnitSprite (entry.Value.Sprite);

                Point position = hexGrid.hexToUnit(entry.Key);

                newUnit.transform.position = new Vector3((float)position.x, (float)position.y, -0.11F);

                displayedUnits.Add(newUnit);

				GameLogic.Units.GenericUnit unit = entry.Value;

				GameObject newBar = Instantiate (healthAndArmorPrefab) as GameObject;

				newBar.transform.SetParent (GameObject.Find ("HealthArmorCanvas").transform, false);
				newBar.name = entry.Value.Name + "HA";

				newBar.transform.position = GridUtility.pointToVector3(hexGrid.hexToUnit(entry.Key));

				float health = unit.Max_HP / unit.Current_HP * 0.4f;
				float armor = unit.Max_AP / unit.Current_AP * 0.4f;

				GameObject healthBar = newBar.transform.FindChild ("Health").gameObject;
				GameObject armorBar = newBar.transform.FindChild ("Armor").gameObject;

				healthBar.GetComponent<Image> ().fillAmount = health;
				armorBar.GetComponent<Image> ().fillAmount = armor;

				healthAndArmorBars.Add (newBar);
            }
        }
    }
}