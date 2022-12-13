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

        ////레이확인용 드로우(테스트용)
        //int dir = rigid.velocity.x > 0 ? 1 : -1;
        //Debug.DrawRay(transform.position, Vector3.right * dir * attackStartRange, new Color(1, 0, 0));
    }

    private void FixedUpdate()
    {
        ////레이확인용 드로우(테스트용)
        //int dir = rigid.velocity.x > 0 ? 1 : -1;
        //Debug.DrawRay(transform.position, Vector3.right * dir * attackRange, new Color(1, 0, 0));
        //
        ////피격 이벤트 레이확인용 드로우(테스트용)2
        //Vector2 leftRayPos = new Vector2(rigid.position.x - 0.38f, rigid.position.y + 0.5f);
        //Vector2 rightRayPos = new Vector2(rigid.position.x + 0.38f, rigid.position.y + 0.5f);
        //Debug.DrawRay(leftRayPos, Vector3.down, new Color(1, 0, 0));
        //Debug.DrawRay(rightRayPos, Vector3.down, new Color(0, 0, 1));
    }

    void CalcAttack() //공격구간 컬라이더 활성화와 컬라이더 회전 처리
    {
        attackTime = anim.GetFloat("AttackDuration");
        if (attackTime > 1.1f) return;

        if (attackTime > (6.0f / 8.0f) && attackTime <= 1.0f) //공격 모션중 실제 플레이어가 피격되는 구간
        {
            colAttack.enabled = true;
            gameObject.layer = 9;

            ////원본 코드
            //if (player.GetSprite().flipX)
            //    colAttack.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            //else
            //    colAttack.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            //수정 코드(테스트)
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
    //액션함수///////////////////////////////////////////////////////

    //Action노드용 함수//////////////////////////////////////////////
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
        Debug.Log("Move함수");

        anim.SetBool("IsAttack", false); //TODO : 이 부분 옮겨서 깔끔하게 수정 + 공격 끝난 후 이동방향으로 Flip값 조정
        anim.SetBool("IsRunning", true);

        if (transform.position.x > leftMoveRange && transform.position.x < rightMoveRange) //정해진 이동범위안에서의 이동
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
        else if (transform.position.x <= leftMoveRange) //왼쪽 최대거리
        {
            transform.position += new Vector3((MoveSpeed * Time.deltaTime), 0.0f, 0.0f);
            sprite.flipX = false;
        }
        else if (transform.position.x >= rightMoveRange) //오른쪽 최대거리
        {
            transform.position += new Vector3((-MoveSpeed * Time.deltaTime), 0.0f, 0.0f);
            sprite.flipX = true;
        }

        return true;
    }


    public bool ChaseAction(Vector2 playerPos)
    {
        Debug.Log("Chase함수");

        anim.SetBool("IsRunning", true);

        if ((transform.position.x < maxLeftChaseRange || transform.position.x > maxRightChaseRange)) //추적거리 벗어나면 기존 위치로 이동
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

        if (transform.position.x > playerPos.x) //플레이어가 몬스터의 왼쪽에 있다면
        {
            sprite.flipX = true;
            transform.position += new Vector3((-ChaseSpeed * Time.deltaTime), 0.0f, 0.0f);
        }
        else if (transform.position.x < playerPos.x) //플레이어가 몬스터의 오른쪽에 있다면
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
        Debug.Log("Attack함수");
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
