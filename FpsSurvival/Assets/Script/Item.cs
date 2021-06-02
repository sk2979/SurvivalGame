using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "New Item", menuName = " New Item/item")]
public class Item : ScriptableObject // 게임 오브젝트에 붙이지 을때
{

    public string itemName; // 아이템의 이름.
    public ItemType itemType;
    public Sprite itemImage; // 아이템의 이미지.
    public GameObject itemPrefab; // 아이템 프리팹.

    public string weaponType; // 

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
