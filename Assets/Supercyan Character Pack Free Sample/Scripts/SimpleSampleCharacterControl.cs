using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSampleCharacterControl : MonoBehaviour
{
    private enum ControlMode
    {
        /// <summary>
        /// Up moves the character forward, left and right turn the character gradually and down moves the character backwards
        /// </summary>
        Tank,
        /// <summary>
        /// Character freely moves in the chosen direction from the perspective of the camera
        /// </summary>
        Direct
    }

    [SerializeField] private float m_moveSpeed = 2;
    public float m_turnSpeed = 200;
    [SerializeField] private float m_jumpForce = 9;

    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;

    [SerializeField] private ControlMode m_controlMode = ControlMode.Direct;
    public VirtualStick joyStick;
    public bool isImmunity = false;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    //private readonly float m_backwardsWalkScale = 0.16f;
    //private readonly float m_backwardRunScale = 0.66f;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;

    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    private bool m_jumpInput = false;

    private bool m_isGrounded;

    private Vector3 moveDir;
    private Quaternion prevRot;
    private float joyX;
    private float joyZ;
    private bool isReadySound;
    private bool isWaitJumping = true;

    private List<Collider> m_collisions = new List<Collider>();

    public static SimpleSampleCharacterControl instance;

    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Rigidbody>(); }
        instance = this;
    }

    private void Start()
    {
        Teleport();
        joyX = joyStick.inputVector.x;
        joyZ = joyStick.inputVector.z;
        prevRot = transform.rotation;
    }

    private void Teleport()
    {
        int stage = CryptoPlayerPrefs.GetInt("stageNum");
        switch (stage)
        {
            case 1:
                transform.position = new Vector3(3, -0.18f, 3);
                break;
            case 2:
                transform.position = new Vector3(-16.08f, -0.18f, 6.42f);
                break;
            case 3:
                transform.position = new Vector3(16.08f, -0.18f, 35.3f);
                break;
            case 4:
                transform.position = new Vector3(16.08f, 0.25f, -23.45f);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            GameManager.instance.fadeTxt.StartFade(1, 0, 0, 0);
            Vector3 dir = new Vector3(3, 3, 3) - transform.position;
            dir.Normalize();
            CharacterDamamged(dir, 1.0f);
        }

        if (other.name == "DownBorder")
        {
            gameObject.transform.position = new Vector3(3, 1, 3);
            BGMManager.instance.PlaySfx(other.transform.position, BGMManager.instance.recallSound, 0, 1);
            m_rigidBody.velocity = Vector3.zero;
        }

        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.getItemSound, 0, 1);
            switch (item.itemNum)
            {
                case 0 :
                    Debug.Log("0번 아이템 사용");
                    GameManager.instance.BuffUI_On(0);
                    Item0_SpeedUp((GameManager.instance.StageNumber * 1.3f) + 1.5f);
                    break;
                case 1:
                    Debug.Log("1번 아이템 사용");
                    GameManager.instance.BuffUI_On(1);
                    Item1_JumpPowerUp((GameManager.instance.StageNumber) * 0.8f);
                    break;
                case 2:
                    Debug.Log("2번 아이템 사용");
                    Item2_Heal();
                    break;
                default:
                    break;
            }
            other.gameObject.SetActive(false);
            SpawnManager.compareValue.Remove(other.GetComponent<Spawn>().SpawnNum);
        }

        if (other.tag == "ClearItem")
        {
            BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.getItemSound, 0, 1);
            GameManager.instance.GetStageClearItem();
            other.gameObject.SetActive(false);
        }

    }
    public void OnJumpBtn()
    {
        if (!m_jumpInput)
        {
            if (isWaitJumping)
            {
                isWaitJumping = false;
                BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.jumpSound, 0, 0.65f);
            }
            m_jumpInput = true;
        }
    }

    /*public void OnJumpBtn()
    {
        if (!m_jumpInput)
        {
            StartCoroutine(JumpSoundStart());
            m_jumpInput = true;
        }
    }

    private IEnumerator JumpSoundStart()
    {
        if (isWaitJumping)
        {
            isWaitJumping = false;
            BGMManager.instance.PlaySfx(transform.position, BGMManager.instance.jumpSound, 0, 0.65f);
            yield return new WaitForSeconds(0);
        }
    }*/

    private void FixedUpdate()
    {
        m_animator.SetBool("Grounded", m_isGrounded);

        switch (m_controlMode)
        {
            case ControlMode.Direct:
                DirectUpdate();
                break;

            case ControlMode.Tank:
                TankUpdate();
                break;

            default:
                Debug.LogError("Unsupported state");
                break;
        }

        m_wasGrounded = m_isGrounded;
        m_jumpInput = false;
    }

    private void TankUpdate()
    {
        float v = joyStick.Vertical();
        float h = joyStick.Horizontal();

        float magV = Mathf.Abs(v);
        float magH = Mathf.Abs(h);

        moveDir = new Vector3(h, 0, v);
        moveDir *= m_moveSpeed;

        //bool walk = Input.GetKey(KeyCode.LeftShift);

        /*if (v < 0)
        {
            if (walk) { v *= m_backwardsWalkScale; }
            else { v *= m_backwardRunScale; }
        }
        else if (walk)
        {
            v *= m_walkScale;
        }*/

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        transform.position += moveDir * Time.deltaTime;
        //transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
        //m_rigidBody.velocity = moveDir;
        //transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);
        if (joyX != 0 || joyZ != 0)
        {
            if(m_isGrounded)
                StartCoroutine(StepSound(BGMManager.instance.footStep));
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(joyX, joyZ) * Mathf.Rad2Deg, 0);
        }
        else
            transform.rotation = prevRot;
        
        prevRot = transform.rotation;

        joyX = joyStick.inputVector.x;
        joyZ = joyStick.inputVector.z;

        if (magV >= magH)
            m_animator.SetFloat("MoveSpeed", Mathf.Abs(m_currentV));
        else                
            m_animator.SetFloat("MoveSpeed", Mathf.Abs(m_currentH));

        JumpingAndLanding();
    }

    IEnumerator StepSound(AudioClip aud)
    {
        if (!isReadySound)
        {
            isReadySound = true;
            BGMManager.instance.PlaySfx(transform.position, aud, 0, 1.0f);
            yield return new WaitForSeconds(aud.length - 0.15f);
            isReadySound = false;
        }
    }

    private void DirectUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Transform camera = Camera.main.transform;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            v *= m_walkScale;
            h *= m_walkScale;
        }

        m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
        m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

        Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

        float directionLength = direction.magnitude;
        direction.y = 0;
        direction = direction.normalized * directionLength;

        if (direction != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", direction.magnitude);
        }

        JumpingAndLanding();
    }

    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

        if (jumpCooldownOver && m_isGrounded && m_jumpInput)
        {
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
            isWaitJumping = true;
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }

    public void CharacterDamamged(Vector3 dir, float power)
    {
        if(GameManager.instance.healthCnt <= 0)
        {
            m_animator.SetTrigger("Die");
            Debug.Log("꽥");
        }

        isImmunity = true;
        m_animator.SetTrigger("Damaged");
        dir.y = 0.4f;
        m_rigidBody.AddForce(dir * power, ForceMode.Impulse);
        Invoke("ImmunityOff", 1.0f);
    }

    private void ImmunityOff()
    {
        isImmunity = false;
    }

    // 0번 아이템
    public void Item0_SpeedUp(float speed)
    {
        m_moveSpeed += speed;
        StartCoroutine(Item0_SpeedDown(speed));
    }

    IEnumerator Item0_SpeedDown(float speed)
    {
        yield return new WaitForSeconds(10.0f);
        m_moveSpeed -= speed;
    }

    // 1번 아이템
    public void Item1_JumpPowerUp(float power)
    {
        m_jumpForce += power;
        StartCoroutine(Item1_JumpPowerDown(power));
    }

    IEnumerator Item1_JumpPowerDown(float power)
    {
        yield return new WaitForSeconds(10.0f);
        m_jumpForce -= power;
    }
    public void Item2_Heal()
    {
        GameManager.instance.CalcHealthCnt(1);
    }

}
