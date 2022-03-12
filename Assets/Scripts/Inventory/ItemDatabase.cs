using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour
{
	private List<Item> database = new List<Item>();
	private JsonData itemData;

	private string[] descriptionTxt;
	private string[] titleTxt;

	void Awake()
	{
		itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
		descriptionTxt = new string[] { "투척용 나이프다.", "Here's Jorney!",
			"1+1 세일 중", "민트초코맛이다.\n몬스터들이 동요하고 있다.",
			"용감한 쿠키", "An Idiot Sandwich",
			"내 드릴은..", "훌륭한 대화수단.",
			"1세트 더!!", "넌 못 지나간다" };
		titleTxt = new string[] { "투척용 나이프", "불도끼",
			"막대 아이스크림", "빅 아이스크림",
			"쿠키", "샌드위치",
			"드릴", "대형 전기톱",
			"덤벨", "전경 방패" };

		ConstructItemDatabase();
	}

	public Item FetchItemById(int id)
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
		for (int i = 0; i < itemData.Count; i++)
		{
			Item newItem = new Item();
			newItem.Id = (int)itemData[i]["id"];
			//newItem.Title = itemData[i]["title"].ToString();
			newItem.Title = titleTxt[i];
			newItem.Value = (int)itemData[i]["value"];
			newItem.Power = (int)itemData[i]["stats"]["power"];
			newItem.Defense = (int)itemData[i]["stats"]["defense"];
			newItem.Vitality = (int)itemData[i]["stats"]["vitality"];
			//newItem.Description = itemData[i]["description"].ToString();
			newItem.Description = descriptionTxt[i];
			newItem.Stackable = (bool)itemData[i]["stackable"];
			newItem.Rarity = (int)itemData[i]["rarity"];
			newItem.Slug = itemData[i]["slug"].ToString();
			newItem.Sprite = Resources.Load<Sprite>("Sprites/Items/" + newItem.Slug);

			database.Add(newItem);
		}
	}
}

public class Item
{
	public int Id { get; set; }
	public string Title { get; set; }
	public int Value { get; set; }
	public int Power { get; set; }
	public int Defense { get; set; }
	public int Vitality { get; set; }
	public string Description { get; set; }
	public bool Stackable { get; set; }
	public int Rarity { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }

	public Item()
	{
		this.Id = -1;
	}
}