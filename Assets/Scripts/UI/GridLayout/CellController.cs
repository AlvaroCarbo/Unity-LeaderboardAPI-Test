using Model;
using TMPro;
using UnityEngine;

namespace UI.GridLayout
{
    public class CellController : MonoBehaviour
    {
        [SerializeField] private TMP_Text idText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text scoreText;
        
        public void SetCellText(LeaderboardEntry leaderboardEntry)
        {
            idText.text = leaderboardEntry.Id;
            nameText.text = leaderboardEntry.Name;
            scoreText.text = leaderboardEntry.Score;
        }
    }
}