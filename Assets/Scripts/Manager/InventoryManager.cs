using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private ActiveItemBook books;
    private List<ActiveItem> inventory;

    private ActiveItem blankItem;
    public Image[] activeItemSlot;

    private void Start()
    {
        books = GetComponent<ActiveItemBook>();
        blankItem = books.CastActiveItem(0);
        inventory = new List<ActiveItem>() { blankItem, blankItem, blankItem, blankItem };

        inventory.Add(books.CastActiveItem(0));

        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < activeItemSlot.Length; i++)
        {
            activeItemSlot[i].sprite = inventory[i].sprite;
        }
    }

    public void GetItem(int num)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].activeNum == 0)
            {
                inventory[i] = books.CastActiveItem(num);
                break;
            }
        }
        Generate();
    }

    public void UseItem(int index)
    {

    }

}
