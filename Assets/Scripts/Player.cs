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

    Vector3 moveVec;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();  // Animator 변수를 GetComponentInChildren() 으로 초기화.
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;  // normalized : 방향값이 1로 보저오딘 벡터.

        /*  if(wDown)
              transform.position += moveVec * speed * 0.3f * Time.deltaTime;
          else
              transform.position += moveVec * speed * Time.deltaTime;*/

        // ==> 한줄로 쓰고 싶으면 ? 삼항연산자

        transform.position += moveVec * speed *(wDown ? 0.3f : 1f) *  Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        transform.LookAt(transform.position + moveVec);
    }
}
