using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class SH_PlayerMove : MonoBehaviour
{
    public Transform player;
    public Transform camPivot;
    CharacterController cc;
    Animator anim;

    float speed;
    public float walkSpeed = 10;
    public float runSpeed = 20;
    public float sprintSpeed = 30;

    Vector3 dir;

    public float jumpPower = 12;
    float gravity = -60;
    float yVelocity = 0;

    public int maxJumpCount = 1;
    int jumpCount = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        
        
    }

    public void PlayerMove()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        dir = player.forward * v + player.right * h;
        dir.Normalize();

        anim.SetBool("Walk", dir.magnitude > 0.1f);
        anim.SetBool("Idle", dir.magnitude < 0.1f);

        yVelocity += gravity * Time.deltaTime;

        if (cc.isGrounded)
        {
            yVelocity = 0;
            jumpCount = 0;
        }

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            yVelocity = jumpPower;
            jumpCount++;
        }

        SetSpeed();

        dir *= speed;
        dir.y = yVelocity;

        cc.Move(dir * Time.deltaTime);
        //else
        //{
        //    // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
        //    transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        //}
    }

    float time = 0;
    float returnSpeed;
    public void SetSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // 2. shift누르면 빠르게 sprintSpeed에 도달하고 싶다.
            speed = Mathf.Lerp(speed, sprintSpeed, Time.deltaTime * 10);
        }
        else if (dir.magnitude > 0.1f)
        {
            time += Time.deltaTime;
            //float preSpeed
            // 1. 1.5초 동안 서서히 증가해 runSpeed에 도달하고 싶다.
            if (time > 1)
                speed = Mathf.Lerp(walkSpeed, runSpeed, time / 2.5f);
        }
        else
        {
            speed = walkSpeed;
            time = 0;
        }

        anim.SetFloat("Speed", speed);
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    // 데이터 보내기
    //    if (stream.IsWriting) // isMine == true
    //    {
    //        // position, rotation
    //        stream.SendNext(transform.position);
    //        stream.SendNext(transform.rotation);
    //    }
    //    // 데이터 받기
    //    else if (stream.IsReading) // isMine == false
    //    {
    //        receivePos = (Vector3)stream.ReceiveNext();
    //        receiveRot = (Quaternion)stream.ReceiveNext();
    //    }
    //}
}
