using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MeleeWeaponController
{
    // 활성화 여부
    public static bool isActivate = true;

    private void Start()
    {
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;
    }
    void Update()
    {
        if (isActivate)
            TryAttack();
    }
    protected override IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
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
