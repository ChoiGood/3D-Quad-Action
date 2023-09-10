using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;

    bool isJump;
    bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;

    Rigidbody rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();  // Animator ������ GetComponentInChildren() ���� �ʱ�ȭ.
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;  // normalized : ���Ⱚ�� 1�� ������ ����.

        if (isDodge)
            moveVec = dodgeVec;

        /*  if(wDown)
              transform.position += moveVec * speed * 0.3f * Time.deltaTime;
          else
              transform.position += moveVec * speed * Time.deltaTime;*/

        // ==> ���ٷ� ���� ������ ? ���׿�����

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if(jDown && moveVec == Vector3.zero &&!isJump && !isDodge)   // bool ������ ���� �������� Ȱ�� ,,, 1. �������� �������� �߰��ؼ� ������ ȸ�Ƿ� ������.
        {                                                                                             // 2. �׼� ���߿� �ٸ� �׼��� ������� �ʵ��� ���� �߰�.              
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)   // bool ������ ���� �������� Ȱ�� 
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;


            Invoke("DodgeOut", 0.6f); // Invoke() �Լ��� �ð��� �Լ� ȣ��
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}
