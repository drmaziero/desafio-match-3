using TMPro;
using UnityEngine;

namespace Views
{
    public class ScoreView : MonoBehaviour
    {
        [field: SerializeField] private TextMeshProUGUI scoreLabel;

        public void UpdateScore(int score)
        {
            Debug.LogWarning("Here 2");
            scoreLabel.SetText($"{score}");
        }
        
    }
}