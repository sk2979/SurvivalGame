using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //스피드 변수
    [SerializeField]
    private float walkSpeed; 
    [SerializeField]
    private float runSpeed; 
    private float applySpeed;
    [SerializeField]
    private float crouchSpeed;

    [SerializeField]
    private float jumpForce;

    //상태변수
    private bool isWalk = false;
    private bool isCrouch = false;
    private bool isGround = true;
    private bool isRun = false;
    private bool pauseCameraRotation = false;

    //움직임 체크 변수
    private Vector3 lastPos;

    // 앉았을 때 얼마나 앉을지 결정하는 변수.
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    //착지 감지할 콜라이더
    private CapsuleCollider capsuleCollider;

    [SerializeField]
    private float cameraSensitivity; // 카메라 민감도

    [SerializeField]
    private float cameraRotationLimit; // 좌우각 한도
    private float currentCameraRotaionX = 0; // 카메라 상하각도

    //컴포넌트
    [SerializeField]
    private Camera myCamera;
    private Rigidbody myRigid;
    private GunController theGuncontroller;
    private Crosshair myCrosshair;
    private StatusController theStatusController;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGuncontroller = FindObjectOfType<GunController>();
        myCrosshair = FindObjectOfType<Crosshair>();
        theStatusController = FindObjectOfType<StatusController>();

        //초기화
        applySpeed = walkSpeed;
        originPosY = myCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        MoveCheck();
        cameraRotation();
        CharacterRotation();
    }


    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;
        myCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }
    // 앉기 자연스럽게하는 코루틴
    IEnumerator CrouchCoroutine()
    {
        float _posY = myCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.1f);
            myCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        myCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }
    private void IsGround()
    {   // 플레이어의 transform.position 에서, 아래방향으로, 캡슐콜라이더 Y축 크기의 절반의 +0.1f 만큼 레이캐스팅
        isGround = Physics.Raycast(this.transform.position, Vector3.down, capsuleCollider.bounds.extents.y+ 0.1f);
        myCrosshair.JumpingAnimation(!isGround);
    }
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (isCrouch)
            Crouch();
        theStatusController.DecreaseStemina(100);
        myRigid.velocity = transform.up * jumpForce;
    }
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancel();
        }
    }
    private void Running()
    {
        if (isCrouch)
            Crouch();

        theGuncontroller.CancelFineSight();

        isRun = true;
        myCrosshair.RunningAnimation(isRun);
        theStatusController.DecreaseStemina(10);
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRun = false;
        myCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }
    private void Move() // 플레이어 이동
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX; // x축 이동계산
        Vector3 _moveVertical = transform.forward * _moveDirZ; // y축 이동계산

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(this.transform.position + _velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.001f)
                isWalk = true;
            else
                isWalk = false;

            myCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }
    private void cameraRotation() // 카메라 상하 회전
    {
        if (!pauseCameraRotation)
        {
            float _xRotation = Input.GetAxisRaw("Mouse Y");
            float _cameraRotationX = _xRotation * -cameraSensitivity;

            currentCameraRotaionX += _cameraRotationX;
            currentCameraRotaionX = Mathf.Clamp(currentCameraRotaionX, -cameraRotationLimit, cameraRotationLimit); // Clmap로 Max, Min 으로 값 고정

            myCamera.transform.localEulerAngles = new Vector3(currentCameraRotaionX, 0f, 0f);
        }
    }

    // 나무벨때 시선 아래로
    public IEnumerator TreeLookCoroutine(Vector3 _target)
    {
        pauseCameraRotation = true; // 카메라 회전 멈추고

        // 시선 방향 계산
        Quaternion direction = Quaternion.LookRotation(_target - myCamera.transform.position);
        Vector3 eulurValue = direction.eulerAngles;
        float destinationX = eulurValue.x;

        while (Mathf.Abs(destinationX - currentCameraRotaionX) >= 0.5f)
        {
            eulurValue = Quaternion.Lerp(myCamera.transform.localRotation, direction, 0.3f).eulerAngles;
            myCamera.transform.localRotation = Quaternion.Euler(eulurValue.x, 0f, 0f);
            currentCameraRotaionX = myCamera.transform.localEulerAngles.x;
            yield return null;
        }
        pauseCameraRotation = false;
    }
    private void CharacterRotation() // 캐릭터 좌우 회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * cameraSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

}
