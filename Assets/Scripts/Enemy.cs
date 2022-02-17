using UnityEngine;

public class Enemy : Spawn
{
    private Transform target;
    private Animator anim;
    private Rigidbody rigid;
    enum ENEMYSTATE
    {
        IDLE = 0,
        MOVE,
        MOVEBACK,
        ATTACK,
        DAMAGED,
        DEAD
    }

    ENEMYSTATE enemyState = ENEMYSTATE.IDLE;

    //Vector3 enemyOrigin;

    float hp;
    public float maxHp = 5;
    public float speed = 3;
    public float rotSpeed = 10;
    public float traceRange = 3;
    public float attackableRange = 1;

    private float gravity = 20.0f;
    private bool processAttack = false;
    
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        //enemyOrigin = transform.position;

        hp = maxHp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "DownBorder")
        {
            gameObject.transform.position = new Vector3(3, 1, 3);
            BGMManager.instance.PlaySfx(other.transform.position, BGMManager.instance.recallSound, 0, 1);
            rigid.velocity = Vector3.zero;
        }
    }

    void Update()       // 몬스터 스테이트 업데이트에 구현 - 몬스터 오브젝트 많이 생성하면 게임 겁나느려짐 조심!!
    {
        if (enemyState == ENEMYSTATE.DEAD) return;

        if (hp <= 0) enemyState = ENEMYSTATE.DEAD;

        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                {
                    anim.SetBool("Walk Forward", false);
                    float dist = Vector3.Distance(target.position, transform.position);

                    if (dist < traceRange)
                    {
                        enemyState = ENEMYSTATE.MOVE;

                        if (dist <= attackableRange)
                        {
                            enemyState = ENEMYSTATE.ATTACK;
                        }
                    }
                }
                break;
            case ENEMYSTATE.MOVE:
                {
                    anim.SetBool("Walk Forward", true);
                    float dist = Vector3.Distance(target.position, transform.position);
                    Vector3 dir = target.position - transform.position;
                    dir.y = 0;
                    dir.y -= gravity * Time.deltaTime;
                    dir.Normalize();

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
                    transform.position += dir * speed * Time.deltaTime;

                    if (dist <= attackableRange)
                    {
                        enemyState = ENEMYSTATE.ATTACK;
                    }

                    if (dist > traceRange)
                    {
                        //enemyState = ENEMYSTATE.MOVEBACK;
                        enemyState = ENEMYSTATE.IDLE;
                    }
                }
                break;
            case ENEMYSTATE.MOVEBACK:
                {
                    /*anim.SetBool("Walk Forward", true);
                    Vector3 dir = enemyOrigin - transform.position;
                    dir.y = 0;
                    dir.Normalize();

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);

                    float dist = Vector3.Distance(target.position, transform.position);

                    if (dist < traceRange)
                    {
                        enemyState = ENEMYSTATE.MOVE;

                        if (dist <= attackableRange)
                        {
                            anim.SetBool("Walk Forward", false);
                            enemyState = ENEMYSTATE.ATTACK;
                        }
                    }
                    if (Mathf.Abs(transform.position.x - enemyOrigin.x) < 2.0f)
                    {
                        anim.SetBool("Walk Forward", false);
                        enemyState = ENEMYSTATE.IDLE;
                    }*/
                }
                break;
            case ENEMYSTATE.ATTACK:
                {
                    anim.SetBool("Walk Forward", false);
                    anim.SetTrigger("Stab Attack");

                    float dist = Vector3.Distance(target.position, transform.position);
                    Vector3 dir = target.position - transform.position;
                    dir.y = 0;
                    dir.Normalize();

                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);

                    if (processAttack && !(SimpleSampleCharacterControl.instance.isImmunity))
                    {
                        processAttack = false;
                        BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.damagedSound, 0, 1);
                        GameManager.instance.CalcHealthCnt(-1);
                        Debug.Log(GameManager.instance.healthCnt);
                        SimpleSampleCharacterControl.instance.CharacterDamamged(dir, 15.0f);
                        EZCameraShake.CameraShaker.Instance.ShakeOnce(2.0f, 2.0f, 0, 0.5f);
                    }

                    if (dist > attackableRange)
                    {
                        enemyState = ENEMYSTATE.MOVE;
                    }
                }
                break;
            case ENEMYSTATE.DAMAGED:
                break;
            case ENEMYSTATE.DEAD:
                anim.SetTrigger("Take Damage");
                GetComponent<BoxCollider>().enabled = false;
                Destroy(gameObject, 1.5f);
                break;
            default:
                break;
        }
    }

    public void EnemyAttackStart()      // 이벤트 함수. 몬스터 공격 모션 시작 (딜레이 조금 남겨놔서 피하기 가능)
    {
        processAttack = true;
    }
    public void EnemyAttackEnd()    // 이벤트 함수. 몬스터 공격 모션 끝
    {
        processAttack = false;
    }
}
