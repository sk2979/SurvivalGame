using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    private Slot[] slots;
    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated) 
                OpenInventory();
            else
                CloseInventory();
        }
    }
    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    // 아이템 채우기
    public void AcquireItem(Item _item, int _count = 1)
    {
        // 아이템이 이미 있으면 개수만 증가
        if (Item.ItemType.Equipment != _item.itemType) // 장비아이템 예외처리
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }
        // 아이템이 없으면 아이템과 개수 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
