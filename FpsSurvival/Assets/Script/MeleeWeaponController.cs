using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 추상 클래스
public abstract class MeleeWeaponController : MonoBehaviour
{ 
    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected MeleeWeapon currentMeleeWeapon;

    // 공격중?
    protected bool isAttack = false; // 공격체크
    protected bool isSwing = false; // 휘두르는중인지 체크

    protected RaycastHit hitInfo; // 레이캐스트에 닿은 오브젝트의 정보를 얻어오는 변수

    [SerializeField]
    private PlayerControl playerControl;

    // Update is called once per frame
    protected void TryAttack()
    {
        if (Input.GetButton("Fire1")) // 좌클릭
        {
            if (!isAttack)
            {
                if (CheckObject())
                {
                    if(currentMeleeWeapon.isAxe && hitInfo.transform.tag == "Tree") // 나무 썰기
                    {
                        if (playerControl != null)
                            StartCoroutine(playerControl.TreeLookCoroutine(hitInfo.transform.GetComponent<Tree>().GetTreeCenterPos()));
                        else
                            Debug.Log("어딧노?");

                        StartCoroutine(AttackCoroutine("Chop", currentMeleeWeapon.workDelayA, currentMeleeWeapon.workDelayB, currentMeleeWeapon.workDelay));// 코루틴 실행
                        return;
                    }
                }
                StartCoroutine(AttackCoroutine("Attack", currentMeleeWeapon.attackDelayA, currentMeleeWeapon.attackDelayB, currentMeleeWeapon.attackDelay));// 코루틴 실행
            }
        }
    }
    protected IEnumerator AttackCoroutine(string _swingType, float _delayA, float _delayB, float _delayC)
    {
        isAttack = true;
        currentMeleeWeapon.anim.SetTrigger(_swingType);

        yield return new WaitForSeconds(_delayA);
        isSwing = true;

        // 공격 활성화 시점 HitCoroutine 실행
        StartCoroutine(HitCoroutine());
        yield return new WaitForSeconds(_delayB);
        isSwing = false;

        // Hitcoroutine 종료
        yield return new WaitForSeconds(_delayC - _delayA - _delayB);
        isAttack = false;
    }

    //추상 코루틴(자식에서 완성)
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentMeleeWeapon.range))
            return true;
        else
            return false;
    }
    // 가상함수(완성함수지만, 추가편집이 가능한 함수)
    public virtual void MeleeWeaponChange(MeleeWeapon _meleeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentMeleeWeapon = _meleeWeapon;
        WeaponManager.currentWeapon = currentMeleeWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentMeleeWeapon.anim;

        currentMeleeWeapon.transform.localPosition = Vector3.zero;
        currentMeleeWeapon.gameObject.SetActive(true);
    }
}
