using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DelegateFunc;


public class BehaviorTree : MonoBehaviour
{
    private bool isTreeEnable;
    
    public double callCount;

    void Start()
    {
        isTreeEnable = true;
        callCount = 0.0d;
    }

    //public bool GetTreeEnable() { return isTreeEnable; }
    
    //private void BTUpdate(Node node)
    //{
    //    if(node.children != null) //리프노드가 아니라면
    //    {
    //        for (int i = 0; i < node.children.Count; i++)
    //        {
    //            if(node.attribute == Node.NodeAttribute.Random)
    //            {
    //                StartCoroutine(node.Execute());
    //                RandomNode temp = (RandomNode)node;
    //                
    //                BTUpdate(node.children[temp.rValue]);
    //            }
    //
    //            BTUpdate(node.children[i]);
    //            
    //            if (node.attribute == Node.NodeAttribute.Sequence)
    //            {
    //                node.GetChildResult(i);
    //                if(node.isSuccesed == false) break;
    //            }
    //            if (node.attribute == Node.NodeAttribute.Selector)
    //            {
    //                node.GetChildResult(i);
    //                if (node.isSuccesed == true) break;
    //            }
    //            StartCoroutine(node.Execute());
    //        }
    //    }
    //    StartCoroutine(node.Execute());
    //}

    private IEnumerator BTUpdate(Node node, float tick = 0.0f)
    {
        if (node.children != null) //리프노드가 아니라면
        {
            switch (node.attribute)
            {
                case Node.NodeAttribute.Root:
                    {
                        for (int i = 0; i < node.children.Count; i++)
                            StartCoroutine(BTUpdate(node.children[i], node.children[i].tick));
                        
                        node.Execute();
                        yield return new WaitForSeconds(tick);

                        if (node.isSuccesed == false)
                            isTreeEnable = false;

                        break;
                    }
                case Node.NodeAttribute.Sequence:
                    {
                        for (int i = 0; i < node.children.Count; i++)
                        {
                            StartCoroutine(BTUpdate(node.children[i], node.children[i].tick));
                            if (node.children[i].isSuccesed == false)
                            {
                                node.isSuccesed = false;
                                break;
                            }
                        }

                        //node.Execute();
                        yield return new WaitForSeconds(tick);

                        break;
                    }
                case Node.NodeAttribute.Selector:
                    {
                        for (int i = 0; i < node.children.Count; i++)
                        {
                            StartCoroutine(BTUpdate(node.children[i], node.children[i].tick));
                        }

                        node.Execute();
                        yield return new WaitForSeconds(tick);

                        break;
                    }
                case Node.NodeAttribute.Inverse:
                    {
                        StartCoroutine(BTUpdate(node.children[0], node.children[0].tick));

                        node.Execute();
                        yield return new WaitForSeconds(tick);

                        break;
                    }
            }//case(Node.NodeAttribute)
        }//if (node.children != null) 
        else
        {
            node.Execute();
            yield return new WaitForSeconds(tick);
        }
    }

    public void ExecuteBT(RootNode root)
    {
        if (isTreeEnable == true)
        {
            callCount++;
            Debug.Log("트리 콜 : " + callCount);
            //BTUpdate(root);
            StartCoroutine(BTUpdate(root, root.tick));
        }
    }
}