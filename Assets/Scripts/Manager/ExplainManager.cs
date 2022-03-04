using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainManager : MonoBehaviour
{
    public Sprite[] itemSprites;
    public GameObject itemUI;
    public Image itemImg;
    public Text nameTxt;
    public Text explainTxt;

    private string[] nameStr;
    private string[] explainStr;
    private List<int> nextInfo;

    private bool isEnd = true;

    private Animator anim;

    static public ExplainManager instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        nextInfo = new List<int>();
        anim = itemUI.GetComponent<Animator>();
        nameStr = new string[] { "특대 아이스크림", "대형 케이크", "구급상자", "[3]번 아이템", "[4]번 아이템", "[5]번 아이템", "[6]번 아이템", "[7]번 아이템", "[8]번 아이템", "[9]번 아이템", "검은 가방" };
        explainStr = new string[] { "이동속도가 빨라집니다. ", "점프력이 상승합니다.", "체력 + 1", "[3]번 아이템 + ", "[4]번 아이템 + ", "[5]번 아이템 + ", "[6]번 아이템 + ", "[7]번 아이템 + ", "[8]번 아이템 + ", "[9]번 아이템 + ", "캐릭터의 경험치를\n30% 올려줍니다." };
        
    }

    public void TextGenerate(int number)
    {
        if (!isEnd)
        {
            nextInfo.Add(number);
            return;
        }

        isEnd = false;
        itemImg.sprite = itemSprites[number];
        nameTxt.text = nameStr[number];
        explainTxt.text = explainStr[number];
        anim.SetTrigger("Go");
        Invoke("AnimEnd", 2.5f);
    }

    public void TextGenerate()
    {
        isEnd = false;
        anim.SetTrigger("Go");
        itemImg.sprite = itemSprites[nextInfo[0]];
        nameTxt.text = nameStr[nextInfo[0]];
        explainTxt.text = explainStr[nextInfo[0]];
        Invoke("InvokeEnd", 2.5f);
    }

    private void AnimEnd()
    {
        isEnd = true;

        if (nextInfo.Count != 0)
        {
            TextGenerate();
        }
    }
    private void InvokeEnd()
    {
        nextInfo.RemoveAt(0);

        if (nextInfo.Count != 0)
            TextGenerate();

        isEnd = true;
    }
}
