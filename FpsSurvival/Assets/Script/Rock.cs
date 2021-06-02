using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; // 바위체력

    [SerializeField]
    private float destroyTime; // 파편 제거 시간

    [SerializeField]
    private SphereCollider col; // 구체 콜라이더

    // 게임 오브젝트들
    [SerializeField]
    private GameObject go_rock; // 바위
    [SerializeField]
    private GameObject go_debris; // 깨진바위
    [SerializeField]
    private GameObject go_effect_perfab; // 채굴 이펙트

    [SerializeField]
    private GameObject go_rock_item_prefab; // Rock 아이템 프리팹

    [SerializeField]
    private int item_count; // Rock 아이템 등장 개수

    // 필요한 사운드 이름
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);
        var clone = Instantiate(go_effect_perfab, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);
        hp--;
        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);

        col.enabled = false;

        for (int i = 0; i < item_count; i++)
        {
            Instantiate(go_rock_item_prefab, go_rock.transform.position + new Vector3(0, 0.7f, 0), Quaternion.identity);
        }

        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
