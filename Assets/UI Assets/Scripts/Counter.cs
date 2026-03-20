using UnityEngine;
using TMPro;
using System.Reflection.Emit;

public class Counter : MonoBehaviour
{

    private TextMeshProUGUI label;
    void Start()
    {
        label = GetComponent<TMPro.TextMeshProUGUI>();
    }

    
    void Update()
    {
        if (GameManager.Instance.PointsUpdated)
        {
            label.text = GameManager.Instance.Points.ToString();
            GameManager.Instance.PointsUpdated = false;
        }
    }
}
