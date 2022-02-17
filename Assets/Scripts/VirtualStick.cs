using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImg;
    private Image joyStickImg;
    public Vector3 inputVector;

    public static VirtualStick instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        bgImg = GetComponent<Image>();
        joyStickImg = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);
            inputVector = new Vector3(pos.x * 2 - 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            joyStickImg.rectTransform.anchoredPosition = 
                new Vector3(inputVector.x * bgImg.rectTransform.sizeDelta.x / 2, inputVector.z * bgImg.rectTransform.sizeDelta.y / 2);
        }
    }
    public virtual void OnPointerUp(PointerEventData pod)
    {
        inputVector = Vector3.zero;
        joyStickImg.rectTransform.anchoredPosition = Vector3.zero;
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {

    }
    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }
    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }
}
