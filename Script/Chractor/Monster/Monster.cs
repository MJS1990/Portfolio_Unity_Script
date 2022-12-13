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

    //�÷��̾� �߽߰� �ִ�� �����ϴ� �Ÿ�
    protected float maxRightChaseRange;
    protected float maxLeftChaseRange;

    //���� ���� �� ���� ���� ����
    public float attackStartRange;

    //����
    protected bool iscDead; //hp�� 0���ϸ� true
    //�̵�
    protected bool iscMove; //�÷��̾ �þ߾ȿ� ���Դٸ� false
    //����
    protected bool iscChase; //�ִ� �����Ÿ��� ����� false, ���ݹ����ȿ� �÷��̾ ������ true
    //����
    protected bool iscAttack; //���ݹ����ȿ� �÷��̾ ������, �ǰݻ��°� �ƴ϶�� true
    //�ǰ�
    protected bool iscDamaged;

    //������� �⺻�� �ʱ�ȭ
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

    //Condition���� �Լ�///////////////////////////////////////////

    //���� ����======================================================
    public bool GetDeadCondition()
    {
        if (HP <= 0)
            iscDead = true;
        else
            iscDead = false;
        
        return iscDead;
    }

    //�̵� ����======================================================
    public bool GetMoveCondition()
    {
        if (iscChase == true || iscAttack == true)
            iscMove = false;
        else
            iscMove = true;

        return iscMove;
    }

    //�߰� ����======================================================
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

    //��������===========================================================
    //���� �ڵ�
    //public bool GetAttackCondition(Vector2 playerPos)
    //{
    //    if (Mathf.Abs(transform.position.x - playerPos.x) <= attackStartRange)
    //        iscAttack = true;
    //    else if (Mathf.Abs(transform.position.x - playerPos.x) > attackStartRange)
    //        iscAttack = false;
    //
    //    return iscAttack;
    //}
    //TODO : �׽�Ʈ �غ��� ����
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

    //�ǰ� ����==========================================================
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

    //������ ���� ��� ������ Action�Լ� 
    abstract public bool Dead();
    abstract public bool Move();
    abstract public bool Chase();
    abstract public bool Attack();
    abstract public bool Damaged();
}