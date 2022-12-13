using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player;

class Goblin : Monster
{
    SpriteRenderer spriteRenderer;

    public int goblinHP;
    public int goblinAttackDamage;

    PolygonCollider2D colAttack;
    float attackTime;

    public PlayerStatus player;

    Animator anim;
    SpriteRenderer sprite;

    public float MoveSpeed;
    public float ChaseSpeed;

    public float rightMoveRange;
    public float leftMoveRange;
    
    void Awake()
    {
        Initialize();
        
        HP = goblinHP;
        attackDamage = goblinAttackDamage;

        rightMoveRange = originPos.x + 0.5f;
        leftMoveRange = originPos.x - 0.5f;

        maxRightChaseRange = rightMoveRange + 0.25f;
        maxLeftChaseRange = leftMoveRange - 0.25f;

        spriteRenderer = GetComponent<SpriteRenderer>();

        colAttack = GetComponent<PolygonCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        CalcAttack();

        ////����Ȯ�ο� ��ο�(�׽�Ʈ��)
        //int dir = rigid.velocity.x > 0 ? 1 : -1;
        //Debug.DrawRay(transform.position, Vector3.right * dir * attackStartRange, new Color(1, 0, 0));
    }

    private void FixedUpdate()
    {
        ////����Ȯ�ο� ��ο�(�׽�Ʈ��)
        //int dir = rigid.velocity.x > 0 ? 1 : -1;
        //Debug.DrawRay(transform.position, Vector3.right * dir * attackRange, new Color(1, 0, 0));
        //
        ////�ǰ� �̺�Ʈ ����Ȯ�ο� ��ο�(�׽�Ʈ��)2
        //Vector2 leftRayPos = new Vector2(rigid.position.x - 0.38f, rigid.position.y + 0.5f);
        //Vector2 rightRayPos = new Vector2(rigid.position.x + 0.38f, rigid.position.y + 0.5f);
        //Debug.DrawRay(leftRayPos, Vector3.down, new Color(1, 0, 0));
        //Debug.DrawRay(rightRayPos, Vector3.down, new Color(0, 0, 1));
    }

    void CalcAttack() //���ݱ��� �ö��̴� Ȱ��ȭ�� �ö��̴� ȸ�� ó��
    {
        attackTime = anim.GetFloat("AttackDuration");
        if (attackTime > 1.1f) return;

        if (attackTime > (6.0f / 8.0f) && attackTime <= 1.0f) //���� ����� ���� �÷��̾ �ǰݵǴ� ����
        {
            colAttack.enabled = true;
            gameObject.layer = 9;

            ////���� �ڵ�
            //if (player.GetSprite().flipX)
            //    colAttack.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            //else
            //    colAttack.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            //���� �ڵ�(�׽�Ʈ)
            if (!spriteRenderer.flipX)
                colAttack.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            else
                colAttack.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        }
        else if (attackTime < (6.0f / 8.0f) || attackTime > 1.0f)
        {
            colAttack.enabled = false;
            gameObject.layer = 7;
        }
    }
    //�׼��Լ�///////////////////////////////////////////////////////

    //Action���� �Լ�//////////////////////////////////////////////
    public bool Idle()
    {
        anim.SetBool("IsRunning", false);
        rigid.velocity.Set(0.0f, 0.0f);

        return true;
    }

    
    public IEnumerator DeadAction()
    {
        anim.SetTrigger("IsDead");
        rigid.velocity = Vector2.zero;

        yield return new WaitForSeconds(1.65f);
        gameObject.SetActive(false);

        yield return true;
    }

    override public bool Dead()
    {
        StartCoroutine(DeadAction());
        return true;
    }


    override public bool Move()
    {
        Debug.Log("Move�Լ�");

        anim.SetBool("IsAttack", false); //TODO : �� �κ� �Űܼ� ����ϰ� ���� + ���� ���� �� �̵��������� Flip�� ����
        anim.SetBool("IsRunning", true);

        if (transform.position.x > leftMoveRange && transform.position.x < rightMoveRange) //������ �̵������ȿ����� �̵�
        {
            if (sprite.flipX == true)
            {
                transform.position += new Vector3((-MoveSpeed * Time.deltaTime), 0.0f, 0.0f);
            }
            else if (sprite.flipX == false)
            {
                transform.position += new Vector3((MoveSpeed * Time.deltaTime), 0.0f, 0.0f);
            }
        }
        else if (transform.position.x <= leftMoveRange) //���� �ִ�Ÿ�
        {
            transform.position += new Vector3((MoveSpeed * Time.deltaTime), 0.0f, 0.0f);
            sprite.flipX = false;
        }
        else if (transform.position.x >= rightMoveRange) //������ �ִ�Ÿ�
        {
            transform.position += new Vector3((-MoveSpeed * Time.deltaTime), 0.0f, 0.0f);
            sprite.flipX = true;
        }

        return true;
    }


    public bool ChaseAction(Vector2 playerPos)
    {
        Debug.Log("Chase�Լ�");

        anim.SetBool("IsRunning", true);

        if ((transform.position.x < maxLeftChaseRange || transform.position.x > maxRightChaseRange)) //�����Ÿ� ����� ���� ��ġ�� �̵�
        {
            if (transform.position.x > originPos.x)
            {
                anim.SetBool("IsRunning", true);
                transform.position += new Vector3((-ChaseSpeed * Time.deltaTime), 0.0f, 0.0f);
                sprite.flipX = true;
            }
            else if (transform.position.x < originPos.x)
            {
                anim.SetBool("IsRunning", true);
                transform.position += new Vector3((ChaseSpeed * Time.deltaTime), 0.0f, 0.0f);
                sprite.flipX = false;
            }
        }

        if (transform.position.x > playerPos.x) //�÷��̾ ������ ���ʿ� �ִٸ�
        {
            sprite.flipX = true;
            transform.position += new Vector3((-ChaseSpeed * Time.deltaTime), 0.0f, 0.0f);
        }
        else if (transform.position.x < playerPos.x) //�÷��̾ ������ �����ʿ� �ִٸ�
        {
            sprite.flipX = false;
            transform.position += new Vector3((ChaseSpeed * Time.deltaTime), 0.0f, 0.0f);
        }

        return true;
    }

    public override bool Chase()
    {
        return ChaseAction(player.transform.position);
    }

    override public bool Attack()
    {
        Debug.Log("Attack�Լ�");
        anim.SetBool("IsAttack", true);

        return true;
    }

    public bool DamagedAction(int damage)
    {
        Stop();

        anim.SetTrigger("IsDamaged");
        HP -= damage;
    
        return true;
    }

    public override bool Damaged()
    {
        return DamagedAction(player.AttackDamage);
    }
}
