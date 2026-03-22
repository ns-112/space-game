using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    
    void Start()
    {
        GameObject.Find("[UI] Canvas").transform.GetChild(5).GetComponent<Button>().onClick.AddListener(SwitchToMenuScene);
        GameObject.Find("[UI] Canvas").transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = GameData.time;
        GameObject.Find("[UI] Canvas").transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = GameData.points.ToString();
        
    }

    

    public void SwitchToMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
