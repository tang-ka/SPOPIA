using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerMove : MonoBehaviour
{
    public Transform player;
    public Transform camPivot;
    CharacterController cc;

    float speed;
    public float walkSpeed = 10;
    public float runSpeed = 15;

    Vector3 dir;

    public float jumpPower = 5;
    float gravity = -8;
    float yVelocity = 0;

    public int maxJumpCount = 1;
    int jumpCount = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        dir = player.forward * v + player.right * h;
        dir.Normalize();

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

        dir.y = yVelocity;

        speed = walkSpeed;

        cc.Move(dir * speed * Time.deltaTime);

        //else
        //{
        //    // Lerp를 이용해서 목적지, 목적방향까지 이동 및 회전
        //    transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
        //    transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
        //}
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
