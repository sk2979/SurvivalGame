using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeController : MeleeWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;

    void Update()
    {
        if (isActivate)
            TryAttack();
    }
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject()) // 충돌체크
            {
                if(hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void MeleeWeaponChange(MeleeWeapon _meleeWeapon)
    {
        base.MeleeWeaponChange(_meleeWeapon);
        isActivate = true;
    }
}
