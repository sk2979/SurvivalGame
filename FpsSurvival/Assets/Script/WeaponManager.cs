using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 환성화 여부
    public static bool isActivate = true;

    // 정적변수. 무기 교체 중복실행 방지
    public static bool isChangeWeapon = false;

    // 현재 무기와 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    // 현재 무기 타입
    [SerializeField]
    private string currentWeaponType;

    // 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;
    [SerializeField]
    private float changeWeaponEndDelayTime;


    // 무기 종류들 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private MeleeWeapon[] meleeWeapons;

    // 딕셔너리로 무기접근
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, MeleeWeapon> meleeWeaponDictionary = new Dictionary<string, MeleeWeapon>();

    // 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;

    // Start is called before the first frame update
    void Start()
    {
        // 딕셔너리 키.맵추가
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < meleeWeapons.Length; i++)
        {
            meleeWeaponDictionary.Add(meleeWeapons[i].MeleeWeaponName, meleeWeapons[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                StartCoroutine(ChangeWeaponCoroutine("HAND", "HAND"));// 무기교체 실행(맨손)
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));// 무기교체 실행(서브머신건)
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));// 무기교체 실행(도끼)
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));// 무기교체 실행(곡괭이)
        }
    }
    // 무기교체 코루틴
    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }
    // 무기교체 함수
    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
        {
            theGunController.GunChange(gunDictionary[_name]);
        }
        else if(_type == "HAND")
        {
            theHandController.MeleeWeaponChange(meleeWeaponDictionary[_name]);
        }
        else if (_type == "AXE")
        {
            theAxeController.MeleeWeaponChange(meleeWeaponDictionary[_name]);
        }
        else if (_type == "PICKAXE")
        {
            thePickaxeController.MeleeWeaponChange(meleeWeaponDictionary[_name]);
        }
    }
    // 이전 무기 액션 캔슬
    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickaxeController.isActivate = false;
                break;
        }
    }
}
