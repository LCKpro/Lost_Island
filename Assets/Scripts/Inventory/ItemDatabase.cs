using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
	private List<ActiveItem> database = new List<ActiveItem>();
	private ActiveItemBook itemData;

	void Start()
	{
		itemData = GetComponent<ActiveItemBook>();
		ConstructItemDatabase();
	}

	public ActiveItem FetchItemById(int id)
	{
		for (int i = 0; i < database.Count; i++)
		{
			if (database[i].Id == id)
			{
				return database[i];
			}
		}

		return null;
	}

	void ConstructItemDatabase()
	{
		for (int i = 0; i < itemData.activeItems.Length; i++)
		{
			ActiveItem newItem = new ActiveItem();
			newItem.Id = itemData.CastActiveItem(i).Id;
			newItem.Title = itemData.CastActiveItem(i).Title;
			newItem.Value = itemData.CastActiveItem(i).Value;
			newItem.Power = itemData.CastActiveItem(i).Power;
			newItem.Defense = itemData.CastActiveItem(i).Defense;
			newItem.Vitality = itemData.CastActiveItem(i).Vitality;
			newItem.Description = itemData.CastActiveItem(i).Description;
			newItem.Stackable = itemData.CastActiveItem(i).Stackable;
			newItem.Rarity = itemData.CastActiveItem(i).Rarity;
			newItem.Slug = itemData.CastActiveItem(i).Slug;
			newItem.Sprite = itemData.CastActiveItem(i).Sprite;

			database.Add(newItem);
		}
	}
}
