using UnityEditor;
using UnityEngine;

class CreatePrefabFromSelected
{
	const string menuTitle = "Cards/Create Card From Selected";

	/// <summary>
	/// Creates a prefab from the selected game object.
	/// </summary>
	[MenuItem(menuTitle)]
	static void CreatePrefab()
	{
		GameObject obj = Selection.activeGameObject;
		string name = obj.name;

		Object prefab = EditorUtility.CreateEmptyPrefab("Assets/Prefabs/Cards/" + name + ".prefab");
		EditorUtility.ReplacePrefab(obj, prefab);
		AssetDatabase.Refresh();
	}

	/// <summary>
	/// Validates the menu.
	/// </summary>
	/// <remarks>The item will be disabled if no game object is selected.</remarks>
	[MenuItem(menuTitle, true)]
	static bool ValidateCreatePrefab()
	{
		return Selection.activeGameObject != null;
	}
}
