using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

abstract class Monster : MonoBehaviour
{
    protected int HP { get; set; }
    protected int attackDamage { get; set; }
    public int GetAttackDamage() { return attackDamage; }

    protected Rigidbody2D rigid { get; set; }

    protected Vector3 originPos;

    //플레이어 발견시 최대로 추적하는 거리
    protected float maxRightChaseRange;
    protected float maxLeftChaseRange;

    //추적 성공 후 공격 시작 범위
    public float attackStartRange;

    //죽음
    protected bool iscDead; //hp가 0이하면 true
    //이동
    protected bool iscMove; //플레이어가 시야안에 들어왔다면 false
    //추적
    protected bool iscChase; //최대 추적거리를 벗어나면 false, 공격범위안에 플레이어가 들어오면 true
    //공격
    protected bool iscAttack; //공격범위안에 플레이어가 들어오고, 피격상태가 아니라면 true
    //피격
    protected bool iscDamaged;

    //멤버변수 기본값 초기화
    protected void Initialize()//int hp, int attackDamage)
    {
        originPos = new Vector2(transform.position.x, transform.position.y);
        
        rigid = GetComponent<Rigidbody2D>();

        attackStartRange = 0.02f;

        //maxRightChaseRange = transform.position.x + 0.7f;
        //maxLeftChaseRange = transform.position.x - 0.7f;
        
        iscDead = false;
        iscMove = true;
        iscChase = false;
        iscAttack = false;
        iscDamaged = false;
    }
    
    //private void Awake()
    //{
    //    originPos = new Vector2(transform.position.x, transform.position.y);
    //
    //    rigid = GetComponent<Rigidbody2D>();
    //
    //    attackStartRange = 0.35f;
    //
    //    iscDead = false;
    //    iscMove = true;
    //    iscChase = false;
    //    iscAttack = false;
    //    iscDamaged = false;
    //}

    //Condition노드용 함수///////////////////////////////////////////

    //죽음 조건======================================================
    public bool GetDeadCondition()
    {
        if (HP <= 0)
            iscDead = true;
        else
            iscDead = false;
        
        return iscDead;
    }

    //이동 조건======================================================
    public bool GetMoveCondition()
    {
        if (iscChase == true || iscAttack == true)
            iscMove = false;
        else
            iscMove = true;

        return iscMove;
    }

    //추격 조건======================================================
    public bool GetChaseCondition()
    {
        if (transform.position.x < maxLeftChaseRange || transform.position.x > maxRightChaseRange || iscAttack == true)
        {
            iscChase = false;
            return iscChase;
        }

        int dir = rigid.velocity.x > 0 ? 1 : -1;

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.right * dir, 0.5f, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
            iscChase = true;

        //Test
        //if(iscChase)
        //    Debug.Log("Chase Condition : " + iscChase);

        return iscChase;
    }

    //공격조건===========================================================
    //원본 코드
    //public bool GetAttackCondition(Vector2 playerPos)
    //{
    //    if (Mathf.Abs(transform.position.x - playerPos.x) <= attackStartRange)
    //        iscAttack = true;
    //    else if (Mathf.Abs(transform.position.x - playerPos.x) > attackStartRange)
    //        iscAttack = false;
    //
    //    return iscAttack;
    //}
    //TODO : 테스트 해본후 수정
    public bool GetAttackCondition()
    {
        int dir = rigid.velocity.x > 0 ? 1 : -1;
        
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.right * dir * attackStartRange, 0.5f, LayerMask.GetMask("Player"));

        if (rayHit.collider != null)
            iscAttack = true;
        else 
            iscAttack = false;
        
        return iscAttack;
    }

    //피격 조건==========================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
            iscDamaged = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
            iscDamaged = false;
    }

    public bool GetDamagedCondition()
    {
        return iscDamaged;
    }

    //===================================================================

    protected void Stop()
    {
        Vector3 prev = transform.position;
        transform.position = prev;
    }

    //조건이 맞을 경우 실행할 Action함수 
    abstract public bool Dead();
    abstract public bool Move();
    abstract public bool Chase();
    abstract public bool Attack();
    abstract public bool Damaged();
}