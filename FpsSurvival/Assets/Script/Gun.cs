using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string gunName;
    public float range;
    public float accruacy;
    public float fireRate;
    public float reloadTime;
    
    public int damage;

    public int reloadBulletCount; // 총알 재장전 개수
    public int currentBulletCount; // 현재 탄창에 남은 총알 개수
    public int maxBulletCount;
    public int carryBulletCount;

    public float retroActionForce; // 반동세기
    public float retroActionFineSightForce; // 정조준시 반동 세기

    public Vector3 fineSightOriginPos; // 정조준시 위치
    public Animator anim;
    public ParticleSystem muzzleFlash; // 파티클

    public AudioClip fire_Sound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
