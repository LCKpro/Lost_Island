﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{

	GameObject inventoryPanel;
	GameObject slotPanel;
	ItemDatabase database;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	private int slotAmount;
	public List<ActiveItem> items = new List<ActiveItem>();
	public List<GameObject> slots = new List<GameObject>();

	void Start()
	{
		database = GetComponent<ItemDatabase>();
		slotAmount = 4;
		inventoryPanel = GameObject.Find("InventoryFrame");
		slotPanel = inventoryPanel.transform.Find("SlotFrame").gameObject;
		for (int i = 0; i < slotAmount; i++)
		{
			items.Add(new ActiveItem());
			slots.Add(Instantiate(inventorySlot));
			slots[i].GetComponent<Slot>().id = i;
			slots[i].transform.SetParent(slotPanel.transform);
		}

		AddItem(0);
		AddItem(1);
		AddItem(1);
		AddItem(1);
		/*AddItem(1);
		AddItem(1);
		AddItem(1);
		AddItem(1);
		AddItem(1);
		AddItem(2);*/
	}

	public void AddItem(int id)
	{
		ActiveItem itemToAdd = database.FetchItemById(id);
		if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].Id == id)
				{
					ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
					data.amount++;
					data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
					break;
				}
			}
		}
		else
		{
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].Id == -1)
				{
					items[i] = itemToAdd;
					GameObject itemObj = Instantiate(inventoryItem);
					itemObj.GetComponent<ItemData>().item = itemToAdd;
					itemObj.GetComponent<ItemData>().slotId = i;
					itemObj.transform.SetParent(slots[i].transform);
					itemObj.transform.position = Vector2.zero;
					itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
					itemObj.name = "Item: " + itemToAdd.Title;
					slots[i].name = "Slot: " + itemToAdd.Title;
					break;
				}
			}
		}
	}

	bool CheckIfItemIsInInventory(ActiveItem item)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].Id == item.Id)
			{
				return true;
			}
		}

		return false;
	}

}