using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform target;
    private int enemeyCount = 0;

    public GameObject[] enemyPref;
    public GameObject[] itemPref;
    public GameObject[] stageClearItemPref;

    private GameObject[] enemy_001_Green;
    private GameObject[] enemy_002_Purple;

    private GameObject[] item_001_IceCream;
    private GameObject[] item_002_Cake;
    private GameObject[] item_003_HealthPack;

    private GameObject[] item_StageClearBox;

    private GameObject[] targetPool;

    public Transform[] spawnPos;

    private float delayTime = 1.0f;

    public static List<int> compareValue;

    void Start()
    {
        compareValue = new List<int>();

        enemy_001_Green = new GameObject[10];
        enemy_002_Purple = new GameObject[10];

        item_001_IceCream = new GameObject[10];
        item_002_Cake = new GameObject[10];
        item_003_HealthPack = new GameObject[10];

        item_StageClearBox = new GameObject[10];

        targetPool = new GameObject[10];

        Generate();

        ItemSpawn();
    }

    void Generate()
    {
        for (int index = 0; index < enemy_001_Green.Length; index++)
        {
            enemy_001_Green[index] = Instantiate(enemyPref[0]);
            enemy_001_Green[index].SetActive(false);
        }

        for (int index = 0; index < enemy_002_Purple.Length; index++)
        {
            enemy_002_Purple[index] = Instantiate(enemyPref[1]);
            enemy_002_Purple[index].SetActive(false);
        }

        for (int index = 0; index < item_001_IceCream.Length; index++)
        {
            item_001_IceCream[index] = Instantiate(itemPref[0]);
            item_001_IceCream[index].SetActive(false);
        }

        for (int index = 0; index < item_002_Cake.Length; index++)
        {
            item_002_Cake[index] = Instantiate(itemPref[1]);
            item_002_Cake[index].SetActive(false);
        }

        for (int index = 0; index < item_003_HealthPack.Length; index++)
        {
            item_003_HealthPack[index] = Instantiate(itemPref[2]);
            item_003_HealthPack[index].SetActive(false);
        }

        for (int index = 0; index < item_StageClearBox.Length; index++)
        {
            item_StageClearBox[index] = Instantiate(stageClearItemPref[0]);
            item_StageClearBox[index].SetActive(false);
        }
    }
    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "Enemy000":
                targetPool = enemy_001_Green;
                break;
            case "Enemy001":
                targetPool = enemy_002_Purple;
                break;
            case "Item000":
                targetPool = item_StageClearBox;
                break;
            case "Item001":
                targetPool = item_001_IceCream;
                break;
            case "Item002":
                targetPool = item_002_Cake;
                break;
            case "Item003":
                targetPool = item_003_HealthPack;
                break;
            default:
                Debug.Log("SpawnManager.MakeObj함수 디폴트값");
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }

        return null;
    }

    private void ItemSpawn()
    {
        if (compareValue.Count > 15)
        {
            Debug.Log("필드에 깔린 인수가 15보다 높음");
            return;
        }

        int randSpawnNum = Random.Range(3, 4 + GameManager.instance.StageNumber);
        
        for (int i = 0; i < randSpawnNum; i++)
        {
            int randSpawnTr = Random.Range(0, spawnPos.Length);

            if (!compareValue.Contains(randSpawnTr))
                i--;

            int randItemNum = Random.Range(0, itemPref.Length + 1);
            string type = "";
            switch (randItemNum)
            {
                case 0:
                    type = "Item000";
                    break;
                case 1:
                    type = "Item001";
                    break;
                case 2:
                    type = "Item002";
                    break;
                case 3:
                    type = "Item003";
                    break;
                default:
                    Debug.Log("아이템 스폰 디폴트값");
                    break;
            }

            if (!compareValue.Contains(randSpawnTr))
            {
                GameObject gameObj = MakeObj(type);
                gameObj.GetComponent<Spawn>().SpawnNum = randSpawnTr;
                gameObj.transform.position = spawnPos[randSpawnTr].position;
                compareValue.Add(randSpawnTr);
            }
        }
    }
    private void EnemySpawn(string type)
    {
        GameObject gameObj = MakeObj(type);
        int randNum = Random.Range(0, 4);
        switch (randNum)
        {
            case 0:
                gameObj.transform.position = target.position - new Vector3(0, -2, -10);
                break;
            case 1:
                gameObj.transform.position = target.position - new Vector3(0, -2, 10);
                break;
            case 2:
                gameObj.transform.position = target.position - new Vector3(-10, -2, 0);
                break;
            case 3:
                gameObj.transform.position = target.position - new Vector3(10, -2, 0);
                break;
            default:
                break;
        }
        gameObj.SetActive(true);
    }

    void Update()
    {
        delayTime -= Time.deltaTime;

        if (delayTime <= 0 && enemeyCount <= 5)
        {
            delayTime = 10.0f;
            EnemySpawn("Enemy000");
            enemeyCount++;
            Debug.Log("적소환");
            ItemSpawn();
        }
    }
}
