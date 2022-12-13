using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

//자식노드의 결과값을 반전, 하나의 자식만 가져야함
public class InverseNode : Node
{
    public InverseNode(float tick = 0.0f)
    {
        children = new List<Node>();
        attribute = NodeAttribute.Inverse;
        isSuccesed = true;
        this.tick = tick;
    }

    override public void Execute()
    {
        isSuccesed = !children[0].isSuccesed;
    }
}