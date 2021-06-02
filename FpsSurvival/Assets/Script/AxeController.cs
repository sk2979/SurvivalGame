using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MeleeWeaponController
{
    // 활성화 여부
    public static bool isActivate = false;

    private void Start()
    {
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
            if(CheckObject())
            {
                if(hitInfo.transform.tag == "Twig")
                {
                    hitInfo.transform.GetComponent<Twig>().Damage(this.transform);
                }
                else if (hitInfo.transform.tag == "Grass")
                {
                    hitInfo.transform.GetComponent<Grass>().Damage();
                }
                else if (hitInfo.transform.tag == "Tree")
                {
                    hitInfo.transform.GetComponent<Tree>().Chop(hitInfo.point, this.transform.eulerAngles.y);
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

