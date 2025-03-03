using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public string PlayerStatus { get; private set; } //此变量应当唯一，没有重复。可用于查找在树上所处的位置。
    public int PotionID { get; private set; }
    public string PotionCom { get; private set; } //脑图上的组合，如AB。。。
    public List<TreeNode> Children { get; private set; }
    private bool ComesToTheEnd;

    public TreeNode(string playerStatus, int potionID, string potionCom, bool comesToTheEnd)
    {
        PlayerStatus = playerStatus;
        PotionID = potionID;
        PotionCom = potionCom;
        Children = new List<TreeNode>();
        this.ComesToTheEnd = comesToTheEnd;
    }
    
    public void AddChild(TreeNode Child)
    {
        Children.Add(Child);
    }
}


public class CompoundTree : MonoBehaviour
{
    private TreeNode root;
    private TreeNode currentNode;

    //单例
    private static CompoundTree instance;
    public static CompoundTree getInstance()
    {
        return instance;
    }
        
    //初始化根节点
    public void Initialize()
    {
        root = new TreeNode("InitialState", -1, "", false); //或许这个root的status可以有其它命名。再说吧。
        currentNode = root;
    }

    //初始化子节点
    public void InitializeChildren(string parentStatus, List<(string playerStatus, int potionID, string potionCom, bool comesToTheEnd)> childrenValues)
    {
        TreeNode parentNode = FindNodeByPlayerStatus(root, parentStatus);
        if (parentNode != null) 
        {
            foreach(var (playerStatus, potionID, potionCom, comesToTheEnd) in childrenValues)
            {
                parentNode.AddChild(new TreeNode(playerStatus, potionID, potionCom, comesToTheEnd));
            }
        }
    }

    //搜索喝的药是否是现在所处节点的下一级子节点，如是，将现在状态移至下一节点
    public void SearchAndMove(int potionID)
    {
        foreach(TreeNode child in currentNode.Children)
        {
            if (child.PotionID == potionID)
            {
                currentNode = child;
            }
        }
    }

    //递归查找某一亲节点的所有子节点
    private TreeNode FindNodeByPlayerStatus(TreeNode node, string playerStatus)
    {
        if (node.PlayerStatus == playerStatus)
        {
            return node;
        }

        foreach (TreeNode child in node.Children)
        {
            TreeNode found = FindNodeByPlayerStatus(child, playerStatus);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

    private void Awake()
    {
        //初始化单例
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}