using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hexlibrary;

public class UIBuilder : MonoBehaviour {

	[HideInInspector]
	public GameLogic.Units.GenericUnit builtUnit;
	public GameObject healthAndArmorPrefab;

	public float radius;
	public float degrees;
	public GameObject categoryPrefab;
	public GameObject subItemPrefab;

	private UIHandler ui;
	private List<GameLogic.Actions.ActionCategory> categories;
	private List<Transform> sources;
	private List<Transform> destinations;
	private int subItemsCount;
	private int categoriesCount;

	void Awake () {

		categories = new List<GameLogic.Actions.ActionCategory> ();
		ui = GameObject.Find ("UI").GetComponent<UIHandler> ();
		sources = new List<Transform> ();
		destinations = new List<Transform> ();
	}

	public void build (GameLogic.Units.GenericUnit unit) {

		builtUnit = unit;
		categoriesCount = 0;

		foreach (GameLogic.Actions.GenericAction action in unit.Actions) {

			subItemsCount = 0;

			foreach (GameLogic.Actions.GenericAction action2 in unit.Actions) {
				if(action.ActionCategory == action2.ActionCategory) {
					subItemsCount++;
				}
			}
			if (subItemsCount == 0) {
				Debug.LogError ("No actions found in this category!");	

			} else if (subItemsCount == 1) {
				
				GameObject actionObject = Instantiate (categoryPrefab) as GameObject;
				actionObject.name = cutName(action.ToString ());
				actionObject.transform.parent = gameObject.transform;
				actionObject.transform.position = gameObject.transform.position;
				actionObject.GetComponent<UIItem> ().action = action;
				actionObject.GetComponent<SpriteRenderer> ().color = iconColor (action.ActionCategory);
					
				sources.Add (actionObject.transform);

				GameObject destination = new GameObject ();
				destination.gameObject.name = actionObject.name + "Destination";
				destination.transform.parent = gameObject.transform;
				destination.transform.position = gameObject.transform.position + Vector3.up * radius;
				destination.transform.RotateAround (gameObject.transform.position, Vector3.back, degrees * categoriesCount);
				destinations.Add (destination.transform);

				categoriesCount++;

			} else if (subItemsCount > 1) {
				
				if (!categories.Contains (action.ActionCategory)) {
					categories.Add (action.ActionCategory);

					GameObject category = Instantiate (categoryPrefab) as GameObject;
					category.name = action.ActionCategory.ToString ();
					category.transform.parent = gameObject.transform;
					category.GetComponent<SpriteRenderer> ().color = iconColor (action.ActionCategory);

					sources.Add (category.transform);

					GameObject destination = new GameObject ();
					destination.gameObject.name = category.name + "Destination";
					destination.transform.parent = gameObject.transform;
					destination.transform.position = gameObject.transform.position + Vector3.up * radius;
					destination.transform.RotateAround (gameObject.transform.position, Vector3.back, degrees * categoriesCount);
					destinations.Add (destination.transform);

					categoriesCount++;

					GameObject actionObject = Instantiate (subItemPrefab) as GameObject;
					actionObject.name = cutName (action.ToString());
					actionObject.GetComponent<UIItem> ().action = action;
					actionObject.GetComponent<SpriteRenderer> ().color = iconColor (action.ActionCategory);

					category.GetComponent<UIItem> ().successors.Add(actionObject.GetComponent<UIItem> ());
					actionObject.transform.parent = category.transform;

				} else {
					GameObject actionObject = Instantiate (subItemPrefab) as GameObject;
					actionObject.name = cutName (action.ToString());
					actionObject.GetComponent<UIItem> ().action = action;
					actionObject.GetComponent<SpriteRenderer> ().color = iconColor (action.ActionCategory);

					GameObject category = GameObject.Find (action.ActionCategory.ToString ());
					category.GetComponent<UIItem> ().successors.Add(actionObject.GetComponent<UIItem> ());
					actionObject.transform.parent = category.transform;
				}
			}
		}

		ui.sources = sources.ToArray ();
		ui.destinations = destinations.ToArray ();
	}

	public void clear () {
		categories.Clear ();
		sources.Clear ();
		destinations.Clear ();
		categoriesCount = 0;

		for(int i = 0; i < gameObject.transform.childCount; i++) {
			Destroy(gameObject.transform.GetChild (i).gameObject);
		}
	}

	private string cutName(string longString) {
		string shortString = longString;

		if(shortString.Contains(".")) {
			shortString = shortString.Remove (0, shortString.LastIndexOf(".") + 1);
		}

		if(shortString.Contains("+")) {
			shortString = shortString.Remove (0, shortString.LastIndexOf("+") + 1);
		}
		return shortString;
	}

	private Color iconColor (GameLogic.Actions.ActionCategory category) {
		
		Color iconColor = new Color ();

		switch (category) {

		case GameLogic.Actions.ActionCategory.Attack:
			iconColor = Color.red;
			break;

		case GameLogic.Actions.ActionCategory.CombatManeuvers:
			iconColor = Color.cyan;
			break;

		case GameLogic.Actions.ActionCategory.Move:
			iconColor = Color.green;
			break;

		case GameLogic.Actions.ActionCategory.Special:
			iconColor = Color.magenta;
			break;

		case GameLogic.Actions.ActionCategory.Spell:
			iconColor = Color.blue;
			break;

		case GameLogic.Actions.ActionCategory.Traps :
			iconColor = Color.yellow;
			break;
		}

		iconColor.a = 1;
		return iconColor;
	}
}
