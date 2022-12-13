using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;

public class SequenceNode : Node
{
    public SequenceNode(float tick = 0.0f)
    {
        children = new List<Node>();

        attribute = NodeAttribute.Sequence;
        isSuccesed = true;
        this.tick = tick;
    }

    override public void Execute()
    {
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
