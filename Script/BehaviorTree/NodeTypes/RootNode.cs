using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

public class RootNode : Node //트리 순회 종료조건을 가진 함수를 func에 대입
{
    public RootNode(float tick = 0.0f)
    {
        children = new List<Node>();

        attribute = NodeAttribute.Root;
        isSuccesed = true;
        this.tick = tick;
    }

    override public void Execute()
    {
        isSuccesed = true;

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].isSuccesed == false)
            {
                isSuccesed = false;
                break;
            }
        }
    }
}
