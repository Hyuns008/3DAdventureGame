using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WearItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventoryManger inventoryManger;
    private WearItemManager wearItemManager;

    [Header("������ ����")]
    [SerializeField, Tooltip("������ �̹���")] private List<Sprite> itemSprite;
    private Image itemImage; //������ �̹���

    private RectTransform itemRectTrs; //�������� ��ƮƮ������
    private Transform itemParenTrs; //�������� �θ���ġ
    private CanvasGroup canvasGroup; //ĵ�����׷�

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        itemParenTrs = transform.parent;

        transform.SetParent(inventoryManger.GetCanvas().transform);
        transform.SetAsLastSibling();

        canvasGroup.blocksRaycasts = false;
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
    }

    private void Awake()
    {
        itemRectTrs = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        inventoryManger = InventoryManger.Instance;

        wearItemManager = WearItemManager.Instance;
    }


    public void SetItemImage(int _itemIndex, float _weaponDamage, float _weaponAttackSpeed)
    {
        itemImage = GetComponent<Image>();

        wearItemManager.SetWearItem(_itemIndex, _weaponDamage, _weaponAttackSpeed);

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
        }
    }
}
