using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionSystem : MonoBehaviour
{
    public int maxAttempts = 5; // 喝药最大次数
    private int attemptsLeft;
    private string currentLine = ""; // 当前的药水线 (AB, BD等)
    private List<string> potionSequence = new List<string>(); // 记录玩家喝药的顺序
    private bool transformationComplete = false;
    public Text feedbackText; // 用来显示信息的UI组件

    // 字典存储药水线的变身结果
    private Dictionary<string, string> potionCombinations = new Dictionary<string, string>()
    {
        {"AB-CE-FG", "永动的蒸汽机器人"},
        {"BD-CE", "炫彩玻璃体"},
        {"AC-DE-FGI-HJ", "长着利爪的一轮"},
        {"AE-CD", "火彩云"},
        {"BC-DE-FG", "永动的蒸汽机器人"},
        {"BE-CD-FG-HIJ", "戴着盔甲的女神"},
        {"CD-BD", "化成一滩彩色的水"},
        {"CE-BD-FGI", "看起来很强的野兽"},
        {"DE", "主角地消失了"},
        // 其他组合可以继续添加
    };

    void Start()
    {
        attemptsLeft = maxAttempts;
    }

    public void DrinkPotion(string potion)
    {
        if (transformationComplete || attemptsLeft <= 0) return;

        if (currentLine == "")
        {
            if (IsValidFirstPotion(potion))
            {
                currentLine = potion; // 开始某一条药水线
                potionSequence.Add(potion);
                UpdateFeedback();
            }
            else
            {
                WrongPotion();
            }
        }
        else
        {
            if (!IsPotionValidForLine(potion))
            {
                WrongPotion();
            }
            else
            {
                potionSequence.Add(potion);
                UpdateFeedback();
            }
        }

        CheckTransformation();
    }

    private void WrongPotion()
    {
        attemptsLeft--;
        feedbackText.text = "@#￥%…错…&"; // 显示错误提示
        if (attemptsLeft <= 0)
        {
            feedbackText.text = "你鼠了！"; // 玩家失败
        }
    }

    private void CheckTransformation()
    {
        string sequenceKey = string.Join("-", potionSequence);
        if (potionCombinations.ContainsKey(sequenceKey))
        {
            feedbackText.text = "你变成了: " + potionCombinations[sequenceKey]; // 显示变身结果
            transformationComplete = true;
        }
        else if (potionSequence.Count >= maxAttempts)
        {
            feedbackText.text = "你鼠了！"; // 达到最大次数未能变身
        }
    }

    private bool IsPotionValidForLine(string potion)
    {
        if (currentLine == "AB")
        {
            return potion == "CE" || potion == "FG"; // AB线的正确顺序
        }
        if (currentLine == "BD")
        {
            return potion == "CE"; // BD线的正确顺序
        }
        if (currentLine == "AC")
        {
            return potion == "DE" || potion == "FGI"; // AC线的正确顺序
        }
        if (currentLine == "AE")
        {
            return potion == "CD"; // AE线的正确顺序
        }
        if (currentLine == "BC")
        {
            return potion == "DE" || potion == "FG"; // BC线的正确顺序
        }
        if (currentLine == "BE")
        {
            return potion == "CD" || potion == "FG" || potion == "HIJ"; // BE线的正确顺序
        }
        if (currentLine == "CD")
        {
            return potion == "BD"; // CD线的正确顺序
        }
        if (currentLine == "CE")
        {
            return potion == "BD" || potion == "FGI"; // CE线的正确顺序
        }
        if (currentLine == "DE")
        {
            return true; // DE线无后续要求
        }
        return false;
    }

    private bool IsValidFirstPotion(string potion)
    {
        return potion == "AB" || potion == "BD" || potion == "AC" || potion == "AE" ||
               potion == "BC" || potion == "BE" || potion == "CD" || potion == "CE" || potion == "DE";
    }

    private void UpdateFeedback()
    {
        feedbackText.text = "当前药水序列: " + string.Join("-", potionSequence);
    }
}
