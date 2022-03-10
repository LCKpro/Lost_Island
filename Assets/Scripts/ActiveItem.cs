using System;
using UnityEngine;

[Serializable]
public class ActiveItem
{
	/*public int damage;
    public Sprite sprite;
    public int activeNum;
    public int amount;*/

	[SerializeField]
	private int id;
	[SerializeField]
	private string title;
	[SerializeField]
	private int value;
	[SerializeField]
	private int power;
	[SerializeField]
	private int defense;
	[SerializeField]
	private int vitality;
	[SerializeField]
	private string description;
	[SerializeField]
	private bool stackable;
	[SerializeField]
	private int rarity;
	[SerializeField]
	private int slug;
	[SerializeField]
	private Sprite sprite;

	/*public int Id { get; set; }
	public string Title { get; set; }
	public int Value { get; set; }
	public int Power { get; set; }
	public int Defense { get; set; }
	public int Vitality { get; set; }
	public string Description { get; set; }
	public bool Stackable { get; set; }
	public int Rarity { get; set; }
	public string Slug { get; set; }*/
    
    public int Id { get => id; set => id = value; }
    public string Title { get => title; set => title = value; }
    public int Value { get => value; set => this.value = value; }
    public int Power { get => power; set => power = value; }
    public int Defense { get => defense; set => defense = value; }
    public int Vitality { get => vitality; set => vitality = value; }
    public string Description { get => description; set => description = value; }
    public bool Stackable { get => stackable; set => stackable = value; }
    public int Rarity { get => rarity; set => rarity = value; }
    public int Slug { get => slug; set => slug = value; }
	public Sprite Sprite { get => sprite; set => sprite = value; }

	public ActiveItem()
	{
		this.Id = -1;
	}
}
