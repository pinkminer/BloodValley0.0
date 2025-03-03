using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode
{
    public string PlayerStatus { get; private set; } //�˱���Ӧ��Ψһ��û���ظ��������ڲ���������������λ�á�
    public int PotionID { get; private set; }
    public string PotionCom { get; private set; } //��ͼ�ϵ���ϣ���AB������
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

    //����
    private static CompoundTree instance;
    public static CompoundTree getInstance()
    {
        return instance;
    }
        
    //��ʼ�����ڵ�
    public void Initialize()
    {
        root = new TreeNode("InitialState", -1, "", false); //�������root��status������������������˵�ɡ�
        currentNode = root;
    }

    //��ʼ���ӽڵ�
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

    //�����ȵ�ҩ�Ƿ������������ڵ����һ���ӽڵ㣬���ǣ�������״̬������һ�ڵ�
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

    //�ݹ����ĳһ�׽ڵ�������ӽڵ�
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
        //��ʼ������
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