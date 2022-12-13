using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

//TODO : 함수 정의가 애매함, 추후 수정하던가 삭제
public class IfNode : Node
{
    public IfNode(float tick = 0.0f)
    {
        attribute = NodeAttribute.If;
        isSuccesed = false;
        this.tick = tick;
        children = new List<Node>();
    }

    //bool
    override public void Execute()
    {
        isSuccesed = true;
    }
}
