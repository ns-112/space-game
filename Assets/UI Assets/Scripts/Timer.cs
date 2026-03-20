using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI label;
    private int x, y, z;
    void Start()
    {
        label = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        x = GameManager.Instance.getGameTime().x;
        y = GameManager.Instance.getGameTime().y;
        z = GameManager.Instance.getGameTime().z;
        
        label.text = ((x < 10) ? "0" + x.ToString() : x.ToString()) + ":" + ((y < 10) ? "0" + y.ToString() : y.ToString()) + ":" + ((z < 10) ? "0" + z.ToString() : z.ToString());
    }
}
