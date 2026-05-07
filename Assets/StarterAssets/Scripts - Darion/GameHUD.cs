using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    public TMP_Text survivalTimeText;
    public TMP_Text tagStatusText;
    public TMP_Text difficultyText;

    public AdaptiveEnemyBotFollow botScript;

    void Update()
    {
        // Survival Time
        survivalTimeText.text =
            "Survival Time: " +
            botScript.GetSurvivalTime().ToString("F1") +
            "s";

        // Tag Status
        if (botScript.IsPlayerTagged())
        {
            tagStatusText.text = "STATUS: TAGGED";
            tagStatusText.color = Color.red;
        }
        else
        {
            tagStatusText.text = "STATUS: SAFE";
            tagStatusText.color = Color.green;
        }

        // Difficulty
        difficultyText.text =
            "Difficulty: " +
            botScript.GetDifficultyLevel();
    }
}