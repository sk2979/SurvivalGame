﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private float destroyTime;
    [SerializeField]
    private float explosionForce;

    [SerializeField]
    private GameObject go_hit_effect_prefab;

    private Rigidbody[] rigidbodys;
    private BoxCollider[] boxColliders;

    [SerializeField]
    private string hit_Sound;
    private void Start()
    {
        rigidbodys = this.transform.GetComponentsInChildren<Rigidbody>();
        boxColliders = this.transform.GetComponentsInChildren<BoxCollider>();
    }

    public void Damage()
    {
        hp--;

        Hit();

        if(hp <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        for (int i = 0; i < rigidbodys.Length; i++)
        {
            rigidbodys[i].useGravity = true;
            rigidbodys[i].AddExplosionForce(explosionForce, transform.position, 1f);
            boxColliders[i].enabled = true;
        }

        Destroy(this.gameObject, destroyTime);
    }

    private void Hit()
    {
        SoundManager.instance.PlaySE(hit_Sound);

        var clone = Instantiate(go_hit_effect_prefab, transform.position + Vector3.up, Quaternion.identity);
        Destroy(clone, destroyTime);
    }
}
