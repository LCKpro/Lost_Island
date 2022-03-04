using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialUI;
    public GameObject skipBtn;

    public TypeEffect tutorialText;
    public TypeEffect explainText;

    public Image tutorialImg;
    public Sprite[] tutorialSprite;

    public SpawnManager spawn;
    public TimeManager timeManager;
    
    public bool isDone = false;
    private int count = 0;
    private string tutorialTalk;
    private string[] explainTalk;
    private bool isEnd = false;

    private void Start()
    {
        tutorialTalk = "여행 중 비행기가 불시착했습니다.\n이 정체모를 섬에는 어떤 일이 벌어질지 모릅니다!\n섬 주변에 있는 전리품은 꽤 도움이 될 것 같습니다.\n또한 연구자료가 담긴 가방을 반드시 모아야합니다.";
        
        explainTalk = new string[4] {"연구원은 조이스틱으로 움직일 수 있습니다!\n섬을 돌아다니며 생존해야 합니다.",
                                     "섬에는 연구원을 노리는 몬스터로 가득합니다!\n공격당하면 체력을 잃게 됩니다.",
                                     "전리품은 다양한 효과를 가지고 있습니다!",
                                     "가방을 모으면 다음 스테이지로 이동이\n가능합니다." };
        
        Generate();
    }

    private void Generate()
    {
        if (CryptoPlayerPrefs.GetString("Tutorial") != "on")
        {
            Debug.Log("튜토리얼 제거!");
            GameStart();
            return;
        }

        CryptoPlayerPrefs.SetString("Tutorial", "0");
        Debug.Log("호출");
        tutorialUI.SetActive(true);
        tutorialText.SetMsg(tutorialTalk);
        explainText.SetMsg(explainTalk[0]);
        tutorialImg.sprite = tutorialSprite[0];
        skipBtn.SetActive(true);
    }

    private void TextEnd()
    {
        TextSetting(tutorialText, tutorialTalk, true);
        TextSetting(explainText, explainTalk[count], true);
    }

    public void GameStart()
    {
        spawn.gameObject.SetActive(true);
        timeManager.gameObject.SetActive(true);
        tutorialUI.SetActive(false);
        skipBtn.SetActive(false);
        TextEnd();
        gameObject.SetActive(false);
    }

    private void TextSetting(TypeEffect type, string text, bool isAnime)
    {
        type.isAnim = isAnime;
        type.SetMsg(text);
    }

    private void Update()
    {
        /*if(Input.GetMouseButtonUp(0))
        {
            if (count >= 3)
            {
                if(isEnd)
                    GameStart();

                if (!tutorialText.isAnim || !explainText.isAnim)
                {
                    isEnd = true;
                    TextEnd();
                    return;
                }

                return;
            }

            if (!explainText.endCursor.activeSelf)
            {
                TextSetting(explainText, explainTalk[count], true);
            }
            else
            {
                count++;
                tutorialText.isAnim = false;
                explainText.SetMsg(explainTalk[count]);
                tutorialImg.sprite = tutorialSprite[count];
            }
        }*/

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Ended)
            {
                if (count >= explainTalk.Length - 1)
                {
                    if (isEnd)
                        GameStart();

                    if (!tutorialText.isAnim || !explainText.isAnim)
                    {
                        isEnd = true;
                        TextEnd();
                        return;
                    }

                    return;
                }


                if (!explainText.endCursor.activeSelf)
                {
                    TextSetting(explainText, explainTalk[count], true);
                }
                else
                {
                    count++;
                    tutorialText.isAnim = false;
                    explainText.SetMsg(explainTalk[count]);
                    tutorialImg.sprite = tutorialSprite[count];
                }
            }
        }
    }
}
