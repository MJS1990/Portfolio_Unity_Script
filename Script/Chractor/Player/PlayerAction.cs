using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Player
{
    class PlayerAction : MonoBehaviour
    {
        PlayerStatus player;
        
        public PolygonCollider2D colAttack1;
        public PolygonCollider2D colAttack2;
        public PolygonCollider2D colDashAttack;

        bool isAttack;
        float a1, a2, da;
        
        float h;

        public bool GetAttackCondition() { return isAttack; }
        
        private void Awake()
        {
            player = GetComponent<PlayerStatus>();
            isAttack = false;

            colAttack1.enabled = false;
            colAttack2.enabled = false;
            colDashAttack.enabled = false;
        }

        void Update()
        {
            InputMove();
            InputJump();

            if ((Input.GetButtonDown("Fire3") && player.GetAnim().GetFloat("DashDuration") <= 1.0f))
            {
                InputDash();
            }
            else if (Input.GetButtonUp("Fire3") || player.GetAnim().GetFloat("DashDuration") > 1.0f)
            {
                player.GetAnim().SetBool("IsDash", false);
                player.GetRigid().velocity = new Vector2(0.0f, player.GetRigid().velocity.y);
                player.DashSpeed = 0.0f;
            }

            //��ð���
            if (Input.GetButtonDown("Fire1") && player.GetAnim().GetBool("IsDash") == true)
            {
                player.GetAnim().SetBool("IsDashAttack", true);
            }
            else
            {
                player.GetAnim().SetBool("IsDashAttack", false);
            }

            InputAttack();
            CalcAttack();

            if (Input.GetButtonDown("TestButton1")) //TODO : �׽�Ʈ �� ����
            {
                StartCoroutine(Dead());
                //Dead();
            }
        }

        void FixedUpdate()
        {
            //�ִ�ӵ� ����
            if (player.GetRigid().velocity.x > player.MaxSpeed + player.DashSpeed)
                player.GetRigid().velocity = new Vector2(player.MaxSpeed + player.DashSpeed, player.GetRigid().velocity.y);
            else if (player.GetRigid().velocity.x < -player.MaxSpeed + player.DashSpeed)
                player.GetRigid().velocity = new Vector2(-player.MaxSpeed + player.DashSpeed, player.GetRigid().velocity.y);

            //if (player.HP <= 0) Dead();
            if (player.HP <= 0) StartCoroutine(Dead());
        }

        //���� �浹�� ���Ͱ��ݿ� ���� �ǰ�ó��
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Terrain")
            {
                player.GetAnim().SetBool("IsJumping", false);
            }

            if(collision.gameObject.tag == "Monster")
            {
                if (collision.gameObject.layer == 7 && isAttack == false) //�ܼ� �浹 �ǰ�
                    OnDamaged(collision.gameObject.transform.position, 1);
            }
        }

        //���ݽ� �浹ó��
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 7)// && isAttack == true) //layer == 7�� ���� ���̾�
            {
                player.GetAnim().speed = 0.0f;

                Invoke("ReturnAnimSpeed", 0.15f);
            }

            //���� ����
            if (collision.gameObject.layer == 9)
            {
                int damage = collision.GetComponent<Monster>().GetAttackDamage();
                if (isAttack == false)
                    OnDamaged(collision.gameObject.transform.position, damage);
            }
        }

        void OnDamaged(Vector2 enemyPos, int damage)
        {
            for (int i = 1; i <= damage; i++)
            {
                if (i > player.HP) break;

                int index = player.HP - i;
                //HPUI[index].enabled = false;
            }

            player.HP -= damage;
            player.GetSprite().color = new Color(1, 1, 1, 0.4f);

            int dir = transform.position.x - enemyPos.x > 0 ? 1 : -1;
            player.GetRigid().AddForce(new Vector2(dir, 0.6f) * 0.5f, ForceMode2D.Impulse);

            gameObject.layer = 6;
            player.GetAnim().SetBool("IsHit", true);

            Invoke("OffDamaged", 0.8f);
        }

        void OffDamaged()
        {
            player.GetSprite().color = new Color(1, 1, 1, 1);
            player.GetAnim().SetBool("IsHit", false);
            gameObject.layer = 0;
        }

        void ReturnAnimSpeed()
        {
            player.GetAnim().speed = 1.0f;
        }

        void InputMove()
        {
            if (player.gameObject.layer == 6) return;

            h = Input.GetAxisRaw("Horizontal") * 2.0f;
            player.GetRigid().AddForce(Vector2.right * h, ForceMode2D.Impulse);

            //TODO : �ִϸ��̼� ��ȯ�κ� �ð����� �����ϴ°� ����ҵ�
            if (Input.GetButton("Horizontal"))
            {
                player.GetAnim().SetBool("IsRunning", true);
            }
            else if (Input.GetButtonUp("Horizontal"))//�̵� ����� ����
            {
                player.GetAnim().SetBool("IsRunning", false);
                player.GetRigid().velocity = new Vector2((player.GetRigid().velocity.normalized.x * 0.02f), player.GetRigid().velocity.y);
            }

            //�̵��������� ��������Ʈ ������ȯ
            if (Input.GetButton("Horizontal"))
            {
                player.GetSprite().flipX = Input.GetAxisRaw("Horizontal") == -1;
            }
        }

        void InputJump()
        {
            if (player.gameObject.layer == 6) return;

            //����
            if (Input.GetButton("Fire2") && player.GetRigid().velocity.y == 0)
            {
                player.GetAnim().SetBool("IsDash", false);
                player.GetAnim().SetBool("IsJumping", true);
                player.GetRigid().AddForce(Vector2.up * player.JumpPower, ForceMode2D.Impulse);
            }
            player.GetAnim().SetFloat("IsFalling", player.GetRigid().velocity.y);
        }

        void InputAttack()
        {
            if (player.gameObject.layer == 6) return;

            float a1 = player.GetAnim().GetFloat("Attack1Duration");
            float a2 = player.GetAnim().GetFloat("Attack2Duration");

            //���� ó��, �Է� �̿��� ó���� Attack_player.GetAnim()Behavior�� ����
            if (Input.GetButtonDown("Fire1"))
            {
                player.GetAnim().SetBool("IsAttack", true);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                player.GetAnim().SetBool("IsAttack", false);
            }
            //�޺�
            if (a1 > 0.01f && a1 <= 1.0f)
            {
                if (Input.GetButtonDown("Fire1"))
                    player.GetAnim().SetBool("IsAttack2", true);
            }

            if ((a1 > 0.01f && a1 <= 1.0f) || (a2 > 0.01f && a2 <= 1.0f))
                player.GetRigid().velocity = Vector2.zero;
        }

        void InputDash()
        {
            if (player.gameObject.layer == 6) return;

            player.GetAnim().SetBool("IsDash", true);

            if (player.GetSprite().flipX == false)
            {
                player.DashSpeed = 1.5f;
                player.GetRigid().AddForce(new Vector2(player.DashSpeed, 0.0f), ForceMode2D.Impulse);
            }
            else if (player.GetSprite().flipX == true)
            {
                player.DashSpeed = -1.5f;
                player.GetRigid().AddForce(new Vector2(player.DashSpeed, 0.0f), ForceMode2D.Impulse);
            }
        }

        void CalcAttack()
        {
            a1 = player.GetAnim().GetFloat("Attack1Duration");
            a2 = player.GetAnim().GetFloat("Attack2Duration");
            da = player.GetAnim().GetFloat("DashAttackDuration");

            if (a1 > 1.1f || a2 > 1.1f) return;

            if (a1 >= (6.0f / 9.0f) && a1 <= 1.0f)
            {
                colAttack1.enabled = true;
                isAttack = true;

                if (player.GetSprite().flipX)
                    colAttack1.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                else
                    colAttack1.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else if (a1 < (6.0f / 9.0f) || a1 > 1.0f)
            {
                colAttack1.enabled = false;
                isAttack = false;
            }

            if (a2 >= 0.00001f && a2 <= 1.0f)
            {
                colAttack2.enabled = true;
                isAttack = true;

                if (player.GetSprite().flipX)
                    colAttack2.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                else
                    colAttack2.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else if (a2 > 1.0f)
            {
                colAttack2.enabled = false;
                isAttack = false;
            }

            //DashAttack
            if (da >= (4.0f / 10.0f) && da <= 1.0f)
            {
                colDashAttack.enabled = true;
                isAttack = true;

                if (player.GetSprite().flipX)
                    colDashAttack.gameObject.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                else
                    colDashAttack.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
            else if (da < (5.0f / 10.0f) || da > 1.0f)
            {
                colDashAttack.enabled = false;
                isAttack = false;
            }
        }
        
        public void VelocityZero()
        {
            player.GetRigid().velocity = Vector2.zero;
        }

        ////�÷��̾� ���� ó��
        //void Dead()
        //{
        //    player.GetAnim().SetTrigger("IsDead");
        //    player.isDead = true;
        //    //TODO : �÷��̾� ������ ó���۾� ����ؾ���
        //    //Time.timeScale = 0;
        //}

        //�÷��̾� ���� ó��
        public IEnumerator Dead()
        {
            player.GetAnim().SetTrigger("IsDead");
            
            yield return new WaitForSeconds(0.82f);
            player.GetAnim().speed = 0.0f;
            //Time.timeScale = 0.3f;
            player.isDead = true; 
            
            yield return true;
        }
    }
}
