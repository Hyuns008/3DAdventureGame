using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUIData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventoryManger inventoryManger;

    [Header("������ ����")]
    [SerializeField, Tooltip("������ �̹���")] private List<Sprite> itemSprite;
    private Image itemImage; //������ �̹���
    [SerializeField, Tooltip("������ �ؽ�Ʈ")] private TMP_Text quantityText;
    private int itemIndex; //������ �����Ϳ� �޾ƿ� �ε���
    private int itemType; //������ �����Ϳ� �޾ƿ� Ÿ��
    private float weaponDamage; //������ �����Ϳ� �޾ƿ� ���� ���ݷ�
    private float weaponAttackSpeed; //������ �����Ϳ� �޾ƿ� ���� ���ݼӵ�

    private RectTransform itemRectTrs; //�������� ��ƮƮ������
    private Transform itemParenTrs; //�������� �θ���ġ
    private CanvasGroup canvasGroup; //ĵ�����׷�

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        itemParenTrs = transform.parent;

        ItemInvenDrop dropSc = itemParenTrs.GetComponent<ItemInvenDrop>();

        if (dropSc != null)
        {
            inventoryManger.ItemSwapA(dropSc.GetNumber());
        }

        inventoryManger.ItemParentA(transform.parent.gameObject);

        transform.SetParent(inventoryManger.GetCanvas().transform);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;

        quantityText.gameObject.SetActive(false);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        itemRectTrs.position = eventData.position;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (transform.parent == inventoryManger.GetCanvas().transform)
        {
            transform.SetParent(itemParenTrs);

            itemRectTrs.position = itemParenTrs.position;
        }

        canvasGroup.blocksRaycasts = true;

        quantityText.gameObject.SetActive(true);

        inventoryManger.ItemParentA(null);
    }

    private void Awake()
    {
        itemRectTrs = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventoryManger = InventoryManger.Instance;

        quantityText.gameObject.SetActive(true);
    }

    public void SetItemImage(int _itemIndex, int _itemType, int _itemQuantity, float _weaponDamage, float _weaponAttackSpeed)
    {
        itemImage = GetComponent<Image>();

        itemIndex = _itemIndex;
        itemType = _itemType;
        weaponDamage = _weaponDamage;
        weaponAttackSpeed = _weaponAttackSpeed;

        switch (_itemIndex)
        {
            case 100:
                itemImage.sprite = itemSprite[0];
                break;
            case 101:
                itemImage.sprite = itemSprite[1];
                break;
            case 102:
                itemImage.sprite = itemSprite[2];
                break;
            case 103:
                itemImage.sprite = itemSprite[3];
                break;
            case 104:
                itemImage.sprite = itemSprite[4];
                break;
            case 200:
                itemImage.sprite = itemSprite[5];
                break;
        }

        if (_itemType != 10)
        {
            quantityText.text = $"x {_itemQuantity}";
        }
        else
        {
            quantityText.text = "";
        }
    }

    public int GetItemIndex()
    {
        return itemIndex;
    }

    public float GetWeaponDamage() 
    {
        return weaponDamage;
    }

    public float GetWeaponAttackSpeed()
    {
        return weaponAttackSpeed;
    }

    public int GetItemType()
    {
        return itemType;
    }
}
