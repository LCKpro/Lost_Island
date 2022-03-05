using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveItemBook : MonoBehaviour
{
    public ActiveItem[] activeItems;

    public ActiveItem CastActiveItem(int index)
    {
        return activeItems[index];
    }
}
