using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    // 깎일 나무 조각들
    [SerializeField]
    private GameObject[] go_treePieces;
    [SerializeField]
    private GameObject go_treeCenter;

    // 나무토막 프리팹
    [SerializeField]
    private GameObject go_log_prefabs;

    // 쓰러질 때 랜덤으로 가해질 힘의 세기
    [SerializeField]
    private float force;
    [SerializeField]
    private GameObject go_childTree;

    // 부모 트리 파괴시 콜라이더 제거
    [SerializeField]
    private CapsuleCollider parentCol;

    // 자식 트리 콜라이더 및 중력 활성화
    [SerializeField]
    private CapsuleCollider childCol;
    [SerializeField]
    private Rigidbody childRigid;

    // 도끼 파편튀는 이펙트
    [SerializeField]
    private GameObject go_hit_effect_prefab;

    // 파편 제거시간
    [SerializeField]
    private float debrisDestroyTime;

    // 나무 제거시간
    [SerializeField]
    private float destroyTime;

    // 사운드
    [SerializeField]
    private string chop_sound;
    [SerializeField]
    private string falldown_sound;
    [SerializeField]
    private string logChange_sound;

    public void Chop(Vector3 _pos, float _angleY)
    {

        Hit(_pos);

        AngleCalc(_angleY);

        if (CheckTreePieces())
            return;

        FallDown_Tree();
    }

    private void FallDown_Tree()
    {
        SoundManager.instance.PlaySE(falldown_sound);

        Destroy(go_treeCenter);

        parentCol.enabled = false;
        childCol.enabled = true;
        childRigid.useGravity = true;

        childRigid.AddForce(UnityEngine.Random.Range(-force, force), 0f, UnityEngine.Random.Range(-force, force));

        StartCoroutine(LogCoroutine());
    }

    IEnumerator LogCoroutine()
    {
        yield return new WaitForSeconds(destroyTime);

        SoundManager.instance.PlaySE(logChange_sound);

        Instantiate(go_log_prefabs, go_childTree.transform.position + (go_childTree.transform.up * 3f), Quaternion.LookRotation(go_childTree.transform.up));
        Instantiate(go_log_prefabs, go_childTree.transform.position + (go_childTree.transform.up * 6f), Quaternion.LookRotation(go_childTree.transform.up));
        Instantiate(go_log_prefabs, go_childTree.transform.position + (go_childTree.transform.up * 9f), Quaternion.LookRotation(go_childTree.transform.up));

        Destroy(go_childTree.gameObject);
    }

    private bool CheckTreePieces()
    {
        for (int i = 0; i < go_treePieces.Length; i++)
        {
            if (go_treePieces[i] != null)
                return true;
        }
        return false;
    }

    private void AngleCalc(float _angleY)
    {
        if (0 <= _angleY && _angleY <= 70)
            DestroyPiece(2);
        else if (70 <= _angleY && _angleY <= 140)
            DestroyPiece(3);
        else if (140 <= _angleY && _angleY <= 210)
            DestroyPiece(4);
        else if (210 <= _angleY && _angleY <= 280)
            DestroyPiece(0);
        else if (280 <= _angleY && _angleY <= 360)
            DestroyPiece(1);
    }

    private void DestroyPiece(int _piece)
    {
        if (go_treePieces[_piece].gameObject != null)
        {
            GameObject clone = Instantiate(go_hit_effect_prefab, go_treePieces[_piece].transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(clone, debrisDestroyTime);

            Destroy(go_treePieces[_piece]);
        }
    }

    private void Hit(Vector3 _pos)
    {
        SoundManager.instance.PlaySE(chop_sound);
        
        GameObject clone = Instantiate(go_hit_effect_prefab, _pos, Quaternion.Euler(Vector3.zero));
        Destroy(clone, debrisDestroyTime);
        
    }

    public Vector3 GetTreeCenterPos()
    {
        return go_treeCenter.transform.position;
    }
}
