using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO : 이 클래스에서는 몬스터의 행동트리를 만들고 ActionManager에서 액션의 결과를 받아와서 노드에 전달 후 몬스터의 행동트리를 업데이트 -> 전체적인 수정 필요
public class MonsterManager : MonoBehaviour
{
    //public Player player;
    //public BehaviorTree bt;
    //public List<Goblin> Goblins;
    //
    //RootNode root;
    //    InverseNode invDead;
    //        SequenceNode seqDead;
    //            ConditionNode cDead; //hp가 0이하라면
    //            ActionNode aDead; //죽음 애니메이션 재생
    //    SelectorNode selBranch;
    //        SequenceNode seqHit; //공격 행동
    //            ConditionNode cHit; //플레이어가 공격 범위안에 있는 상태라면
    //            ActionNode aHit; //공격애니메이션 재생
    //        SequenceNode seqAttack; //공격 행동
    //            ConditionNode cAttack; //플레이어가 공격 범위안에 있는 상태라면
    //            ActionNode aAttack; //공격애니메이션 재생
    //        SequenceNode seqChase; //정찰 행동
    //            ConditionNode cChase; //시야안에 플레이어 존재한다면
    //            ActionNode aChase; //플레이어를 향해 이동
    //        SequenceNode seqMove;
    //            ConditionNode cMove;
    //            ActionNode aMove; //기본 이동
    //
    ////컨디션, 액션노드 삽입용 bool변수
    //List<bool> iscAlive;
    //List<bool> iscMove;
    //List<bool> iscChase;
    //List<bool> iscAttack;
    //List<bool> iscHit;
    //
    //List<bool> isaAlive;
    //List<bool> isaMove;
    //List<bool> isaChase;
    //List<bool> isaAttack;
    //List<bool> isaHit;
    //
    //
    //private void Awake()
    //{
    //    //InitGoblinTree();
    //
    //    iscAlive = new List<bool>();
    //    iscMove = new List<bool>();
    //    iscChase = new List<bool>();
    //    iscAttack = new List<bool>();
    //    iscHit = new List<bool>();
    //
    //    isaAlive = new List<bool>();
    //    isaMove = new List<bool>();
    //    isaChase = new List<bool>();
    //    isaAttack = new List<bool>();
    //    isaHit = new List<bool>();
    //}
    //
    //void Update()
    //{
    //}
    //
    //private void FixedUpdate()
    //{
    //    if (bt.GetTreeEnable() == true)
    //    {
    //        for(int i = 0; i < Goblins.Count; i++)
    //        {
    //            //StartCoroutine(bt.CoBTUpdate(root)); //코루틴 업데이트
    //            bt.BTUpdate(root); //코루틴X 업데이트
    //        }
    //    }
    //}
    //
    ////컨디션 노드용 함수/////////////////////////////////////////////////
    //bool GetDeadCondition()
    //{
    //    bool temp = false;
    //
    //    foreach(Goblin goblin in Goblins)
    //    {
    //        //iscAlive.Add(goblin.GetDeadCondition());
    //        temp = goblin.GetDeadCondition();
    //    }
    //
    //    return temp;
    //}
    //
    //bool GetMoveCondition()
    //{
    //    bool iscMove = false;
    //
    //    //foreach (Goblin goblin in Goblins)
    //    //{
    //    //    iscMove = goblin.GetMoveCondition();
    //    //}
    //
    //    for (int i = 0; i < Goblins.Count; i++)
    //    {
    //        iscMove = Goblins[i].GetMoveCondition();
    //    }
    //
    //    return iscMove;
    //}
    //
    //bool GetChaseCondition()
    //{
    //    bool iscChase = false;
    //
    //    //foreach (Goblin goblin in Goblins)
    //    //{
    //    //    iscChase = goblin.GetChaseCondition();
    //    //}
    //
    //    for (int i = 0; i < Goblins.Count; i++)
    //    {
    //        iscChase = Goblins[i].GetChaseCondition();
    //    }
    //
    //    return iscChase;
    //}
    //
    //bool GetAttackCondition()
    //{
    //    bool iscAttack = false;
    //
    //    //foreach (Goblin goblin in Goblins)
    //    //{
    //    //    iscAttack = goblin.GetAttackCondition(player.transform.position);
    //    //}
    //
    //    for (int i = 0; i < Goblins.Count; i++)
    //    {
    //        iscAttack = Goblins[i].GetAttackCondition(player.transform.position);
    //    }
    //
    //    return iscAttack;
    //}
    //
    //bool GetHitCondition()
    //{
    //    bool iscHit = false;
    //
    //    //foreach (Goblin goblin in Goblins)
    //    //{
    //    //    if (player.GetMonsterHit() == false)
    //    //        iscHit = false;
    //    //    else if (player.GetMonsterHit() == true)
    //    //        iscHit = true;
    //    //}
    //
    //    //for (int i = 0; i < Goblins.Count; i++)
    //    //{
    //    //    if (player.GetMonsterHit() == false)
    //    //        iscHit = false;
    //    //    else if (player.GetMonsterHit() == true)
    //    //        iscHit = true;
    //    //
    //    //}
    //
    //    return iscHit;
    //}
    //
    ////액션노드용 함수////////////////////////////////////////////////
    //bool DeadAction()
    //{
    //    bool isaDead = false;
    //
    //    foreach (Goblin goblin in Goblins)
    //    {
    //        isaDead = goblin.Dead();
    //    }
    //
    //    return isaDead;
    //}
    //
    //bool MoveAction()
    //{
    //    bool isaMove = false;
    //
    //    //foreach (Goblin goblin in Goblins)
    //    //{
    //    //    isaMove = goblin.Move();
    //    //}
    //
    //    for (int i = 0; i < Goblins.Count; i++)
    //    {
    //        isaMove = Goblins[i].Move();
    //    }
    //
    //    return isaMove;
    //}
    //
    //bool ChaseAction()
    //{
    //    bool isaChase = false;
    //
    //    //foreach (Goblin goblin in Goblins)
    //    //{
    //    //    isaChase = goblin.Chase(player.transform.position);
    //    //}
    //
    //    for (int i = 0; i < Goblins.Count; i++)
    //    {
    //        isaChase = Goblins[i].Chase(player.transform.position);
    //    }
    //
    //    return isaChase;
    //}
    //
    //bool AttackAction()
    //{
    //    bool isaAttack = false;
    //
    //    //foreach (Goblin goblin in Goblins)
    //    //{
    //    //    isaAttack = goblin.Attack();
    //    //}
    //
    //    for (int i = 0; i < Goblins.Count; i++)
    //    {
    //        isaAttack = Goblins[i].Attack();
    //    }
    //
    //    return isaAttack;
    //}
    //
    //bool HitAction()
    //{
    //    bool isaHit = false;
    //
    //    foreach (Goblin goblin in Goblins)
    //    {
    //        isaHit = goblin.Hit(player.AttackDamage);
    //        //StartCoroutine(goblin.Hit(player.AttackDamage));
    //        isaHit = true;
    //    }
    //
    //    return isaHit;
    //}
    //
    ////void InitGoblinTree()
    ////{
    ////    //노드 선언
    ////    root = new RootNode();
    ////    invDead = new InverseNode();
    ////    seqDead = new SequenceNode();
    ////    cDead = new ConditionNode(GetDeadCondition);
    ////    aDead = new ActionNode(DeadAction);
    ////    selBranch = new SelectorNode();
    ////    seqHit = new SequenceNode();
    ////    cHit = new ConditionNode(GetHitCondition);
    ////    aHit = new ActionNode(HitAction);
    ////    seqAttack = new SequenceNode();
    ////    cAttack = new ConditionNode(GetAttackCondition);
    ////    aAttack = new ActionNode(AttackAction);
    ////    seqChase = new SequenceNode();
    ////    cChase = new ConditionNode(GetChaseCondition);
    ////    aChase = new ActionNode(ChaseAction);
    ////    seqMove = new SequenceNode();
    ////    cMove = new ConditionNode(GetMoveCondition);
    ////    aMove = new ActionNode(MoveAction);
    ////    
    ////    //노드 연결
    ////    //Goblin Tree
    ////    root.SetChild(invDead);
    ////        invDead.SetChild(seqDead);
    ////            seqDead.SetChild(cDead);
    ////            seqDead.SetChild(aDead);
    ////    root.SetChild(selBranch);
    ////        selBranch.SetChild(seqHit);
    ////            seqHit.SetChild(cHit);
    ////            seqHit.SetChild(aHit);
    ////        selBranch.SetChild(seqAttack);
    ////            seqAttack.SetChild(cAttack);
    ////            seqAttack.SetChild(aAttack);
    ////        selBranch.SetChild(seqChase);
    ////            seqChase.SetChild(cChase);
    ////            seqChase.SetChild(aChase);
    ////        selBranch.SetChild(seqMove);
    ////            seqMove.SetChild(cMove);
    ////            seqMove.SetChild(aMove);
    ////}
}